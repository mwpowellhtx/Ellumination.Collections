namespace Ellumination.Collections
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    // ReSharper disable once PartialTypeWithSinglePart
    internal partial class FlagsEnumerationDescriptor
    {
        internal NamespaceDeclarationSyntax NamespaceDecl { get; }

        internal TypeSyntax Type { get; }

        internal SyntaxToken TypeIdentifier { get; }

        //private ImmutableArray<Entry> Entries { get; }

        internal TypeDeclarationSyntax TypeDecl { get; }

        /// <summary>
        /// Returns a Created instance of the <see cref="FlagsEnumerationDescriptor"/> based on
        /// the given parameters.  Furthermore, as far as I could determine, this can all be
        /// derived from the <paramref name="typeDecl"/> itself.
        /// </summary>
        /// <param name="typeDecl"></param>
        /// <returns></returns>
        public static FlagsEnumerationDescriptor Create(TypeDeclarationSyntax typeDecl)
            => new FlagsEnumerationDescriptor(typeDecl);

#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved
        /// <summary>
        /// Derived <see cref="NameSyntax"/> Type Syntax based on the <paramref name="typeDecl"/>,
        /// Identifier <see cref="SyntaxToken"/>, as well as the base <paramref name="typeDecl"/>.
        /// We do so in a way that hides the
        /// <see cref="SyntaxNodeExtensions.WithoutTrivia{TSyntax}"/> from the caller in a
        /// transparent manner.
        /// </summary>
        /// <param name="typeDecl"></param>
        private FlagsEnumerationDescriptor(TypeDeclarationSyntax typeDecl)
        {
            // TODO: TBD: add some Namespace comprehension...
            NamespaceDecl = (typeDecl.Parent as NamespaceDeclarationSyntax)?.WithoutTrivia();
            Type = typeDecl.GetTypeSyntax().WithoutTrivia();
            TypeIdentifier = typeDecl.Identifier.WithoutTrivia();
            TypeDecl = typeDecl.WithoutTrivia();
        }
#pragma warning restore CS1574 // XML comment has cref attribute that could not be resolved

    }
}
