using System;
using System.Linq;

namespace Ellumination.Collections
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using static String;

    internal static class FlagsEnumerationExtensionMethods
    {
        public static bool IsFlagsEnumerationAttributeSyntax(this AttributeSyntax syntax)
            => syntax.GetUnqualifiedIdentifier().IsFlagsEnumerationAttributeName();

        private const string FlagsEnumeration = nameof(FlagsEnumeration);

        public static bool IsFlagsEnumerationAttributeName(this SyntaxToken? syntax)
            => syntax != null
               && (!IsNullOrWhiteSpace(syntax?.ValueText) && (
                       syntax?.ValueText == FlagsEnumeration
                       || syntax?.ValueText == $"{FlagsEnumeration}Attribute"
                   ));

        public static SyntaxToken? GetUnqualifiedIdentifier(this AttributeSyntax syntax)
            => (syntax.Name.DescendantNodesAndSelf()
                .LastOrDefault(node => node is IdentifierNameSyntax) as IdentifierNameSyntax)?.Identifier;
    }
}