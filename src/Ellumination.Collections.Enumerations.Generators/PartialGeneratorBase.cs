using System.Linq;
using System.Threading;

namespace Ellumination.Collections
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
    using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

    internal abstract class PartialGeneratorBase
    {
        protected FlagsEnumerationDescriptor Descriptor { get; }

        protected CancellationToken CancellationToken { get; }

        // TODO: TBD: could reconsider CodeGeneration.Roslyn dependency here...
        // TODO: TBD: perhaps in favor of a fit-for-purpose code generation?
        // TODO: TBD: or shift gears and sort out our Code.Generation.Roslyn package...
        protected PartialGeneratorBase(FlagsEnumerationDescriptor descriptor
            , CancellationToken cancellationToken)
        {
            Descriptor = descriptor;
            CancellationToken = cancellationToken;
        }

        // TODO: TBD: must also include some comprehension of the container namespace as well I think...
        public virtual MemberDeclarationSyntax GenerateMemberDeclaration(FlagsEnumerationDescriptor descriptor)
            => ClassDeclaration(GenerateTypeIdentifier())
                .WithTypeParameterList(GenerateTypeParameterList())
                .WithBaseList(GenerateBaseList())
                .WithModifiers(GenerateModifiers(descriptor))
                .WithMembers(GenerateMembers());
            //// TODO: TBD: not sure that I need to enclose this in Namespace Declaration after all...
            //// TODO: TBD: additionally, also, the CodeGeneration.Roslyn apparently already Normalizes...
            //=> descriptor.NamespaceDecl == null
            //    ? (MemberDeclarationSyntax) ClassDeclaration(GenerateTypeIdentifier())
            //        .WithTypeParameterList(GenerateTypeParameterList())
            //        .WithBaseList(GenerateBaseList())
            //        .WithModifiers(GenerateModifiers(descriptor))
            //        .WithMembers(GenerateMembers())
            //    : GenerateNamespace(descriptor)
            //        .WithMembers(new SyntaxList<MemberDeclarationSyntax>(
            //            ClassDeclaration(GenerateTypeIdentifier())
            //                .WithTypeParameterList(GenerateTypeParameterList())
            //                .WithBaseList(GenerateBaseList())
            //                .WithModifiers(GenerateModifiers(descriptor))
            //                .WithMembers(GenerateMembers())
            //        )).NormalizeWhitespace();

        protected NamespaceDeclarationSyntax GenerateNamespace(FlagsEnumerationDescriptor descriptor)
            => descriptor.NamespaceDecl.WithoutTrivia();

        protected virtual TypeParameterListSyntax GenerateTypeParameterList()
            => Descriptor.TypeDecl.TypeParameterList?.WithoutTrivia();

        protected virtual SyntaxToken GenerateTypeIdentifier()
            => Descriptor.TypeIdentifier.WithoutTrivia();

        protected virtual SyntaxTokenList GenerateModifiers(FlagsEnumerationDescriptor descriptor)
        {
            /* Start from the Given Modifiers. Class Declaration goes without saying, but we will
             * account for the conversion regardless. */

            var modifiers = (descriptor.TypeDecl as ClassDeclarationSyntax)
                            ?.Modifiers.ToArray() ?? new SyntaxToken[0];

            var partialToken = $"{Token(PartialKeyword)}";

            /* If Partial is already among them, as it should be, no worries. The Analyzer Code Fix
             * should take care of this instance, but in the event that is does not, or has not yet,
             * then go ahead and account for Partial correctly, regardless. */

            if (modifiers.All(token => $"{token}" != partialToken))
            {
                // If not then go ahead and append Partial.
                modifiers = modifiers.Concat(new[] {Token(PartialKeyword)}).ToArray();
            }

            // But which should include any other Modifiers of interest: i.e. internal, public, etc.

            return TokenList(modifiers);
        }

        protected virtual SyntaxList<MemberDeclarationSyntax> GenerateMembers()
            => List<MemberDeclarationSyntax>();

        protected virtual BaseListSyntax GenerateBaseList() => null;
    }
}
