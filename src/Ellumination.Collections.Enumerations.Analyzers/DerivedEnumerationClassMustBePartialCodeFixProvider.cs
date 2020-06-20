using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;

namespace Ellumination.Collections
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using static Descriptors;

    /// <summary>
    /// Flags Enumeration derivations must be partial in order for the Generators to function
    /// properly.
    /// </summary>
    /// <inheritdoc />
    [ExportCodeFixProvider(LanguageNames.CSharp
         , Name = nameof(DerivedEnumerationClassMustBePartialCodeFixProvider)), Shared]
    public class DerivedEnumerationClassMustBePartialCodeFixProvider : CodeFixProvider
    {
        /// <inheritdoc />
        public sealed override ImmutableArray<string> FixableDiagnosticIds
            => ImmutableArray.Create(X1000_DerivedEnumerationMustBePartial.Id);

        /// <inheritdoc />
        public sealed override FixAllProvider GetFixAllProvider()
            => WellKnownFixAllProviders.BatchFixer;

        /// <inheritdoc />
        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var classDeclaration = root.FindNode(context.Span).FirstAncestorOrSelf<ClassDeclarationSyntax>();
            var document = context.Document;
            context.RegisterCodeFix(Actions.MakePartial(document, root, classDeclaration), context.Diagnostics);
        }
    }
}
