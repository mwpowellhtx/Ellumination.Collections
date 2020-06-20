using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Ellumination.Collections
{
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using static Descriptors;
    using static Diagnostic;
    using static SyntaxKind;

    /// <summary>
    /// Flags Enumeration derivations must be partial in order or the Generators to function
    /// properly.
    /// </summary>
    /// <inheritdoc />
    // ReSharper disable once UnusedMember.Global
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DerivedEnumerationClassMustBePartial : DiagnosticAnalyzer
    {
        /// <inheritdoc />
        /// <see cref="ImmutableArray.Create{T}(T)"/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(X1000_DerivedEnumerationMustBePartial);

        /// <inheritdoc />
        public override void Initialize(AnalysisContext context)
        {
            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterSyntaxNodeAction(FlagsEnumerationClassDeclCallback, ClassDeclaration);
        }

        private static void FlagsEnumerationClassDeclCallback(SyntaxNodeAnalysisContext context)
        {
            var symbol = context.SemanticModel.GetDeclaredSymbol(context.Node) as INamedTypeSymbol;

            // Precluding non-Enumeration Class Declarations.
            if (!DerivesFromEnumeration(symbol, symbol))
            {
                return;
            }

            var classDecl = (ClassDeclarationSyntax) context.Node;

            // TODO: TBD: does it make sense to visit the Semantic Model to examine for FlagsEnumerationAttribute?
            // If no Flags Enumeration Attribute then stop the diagnostic.
            if (!HasAnyFlagsEnumerationAttributes(classDecl))
            {
                return;
            }

            // TODO: TBD: may need/want to check for not-abstract as well... ?

            // The Enumeration in question is already partial.
            if (classDecl.Modifiers.Any(PartialKeyword))
            {
                return;
            }

            context.ReportDiagnostic(CreateDiagnostic(classDecl));
        }

        //// TODO: TBD: it turns out the original Syntax-based approach is unnecessary; and/or that it just works out better with the Semantic Model in mind...
        //// ReSharper disable once SuggestBaseTypeForParameter
        //private static bool DerivesFromEnumeration(ClassDeclarationSyntax classDecl)
        //    => classDecl.AncestorsAndSelf().Any(IsDerivedFromEnumeration);

        //private static bool IsDerivedFromEnumeration(SyntaxNode node)
        //{
        //    ClassDeclarationSyntax s;
        //    return node is ClassDeclarationSyntax syntax
        //           && syntax.Identifier.ValueText.EndsWith("Enumeration");
        //}

        //// TODO: TBD: there are a couple of instances of these sort of hard-coded references
        //// TODO: TBD: may need/want to (re-)consider whether it makes sense to make an actual assembly reference...
        /// <summary>
        /// Ellumination.Collections.Enumeration
        /// </summary>
        private const string BaseClassFullName = "Ellumination.Collections.Enumeration";

        /// <summary>
        /// Returns whether the <paramref name="current"/> <see cref="ITypeSymbol"/> derives from
        /// the <see cref="BaseClassFullName"/>. Also uses the <paramref name="root"/> as a frame
        /// of reference. Mind, however, the recursive nature of the method. We will traverse the
        /// class hierarchy until we have either reached the terminal point or a match has been
        /// discovered. Terminal condition is considered to be when either or both of
        /// <paramref name="root"/> or <paramref name="current"/> are null, meaning that we have
        /// exhausted the <see cref="ITypeSymbol.BaseType"/> instances in which to traverse.
        /// </summary>
        /// <param name="root">The Root symbol. Initially starts as <paramref name="current"/> and
        /// is expected to remain the same throughout the traversal.</param>
        /// <param name="current">The Current symbol. Initially starts out as
        /// <paramref name="root"/> and is expected to traverse the
        /// <see cref="ITypeSymbol.BaseType"/> until the terminal condition is reached or a match
        /// is discovered.</param>
        /// <returns></returns>
        /// <see cref="BaseClassFullName"/>
        private static bool DerivesFromEnumeration(ITypeSymbol root, ITypeSymbol current)
            => !(root == null || current == null)
               // It really cannot have derived from the Base Base Type, but rather this one.
               && ($"{current}" == $"{BaseClassFullName}<{root}>"
                   || DerivesFromEnumeration(root, current.BaseType));

        // ReSharper disable once SuggestBaseTypeForParameter
        /// <summary>
        /// Returns whether the <paramref name="classDecl"/> HasAnyFlagsEnumerationAttributes
        /// by examining the <see cref="ClassDeclarationSyntax.AttributeLists"/> across the
        /// breadth of specified <see cref="System.Attribute"/> usages.
        /// </summary>
        /// <param name="classDecl"></param>
        /// <returns></returns>
        private static bool HasAnyFlagsEnumerationAttributes(ClassDeclarationSyntax classDecl)
            => classDecl.AttributeLists.SelectMany(l => l.Attributes
                .Where(a => a.IsFlagsEnumerationAttributeSyntax())).Any();

        // ReSharper disable once SuggestBaseTypeForParameter
        /// <summary>
        /// Returns the Created <see cref="Diagnostic"/>.
        /// </summary>
        /// <param name="classDecl"></param>
        /// <returns></returns>
        /// <see cref="Create(DiagnosticDescriptor,Location,object[])"/>
        private static Diagnostic CreateDiagnostic(ClassDeclarationSyntax classDecl)
            => Create(X1000_DerivedEnumerationMustBePartial
                , classDecl.Identifier.GetLocation(), classDecl.Identifier.ValueText);
    }
}
