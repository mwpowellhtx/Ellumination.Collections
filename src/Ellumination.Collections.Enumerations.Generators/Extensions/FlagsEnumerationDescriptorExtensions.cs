using System;
using System.Linq;

namespace Ellumination.Collections
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
    using static Char;
    using static String;

    internal static class FlagsEnumerationDescriptorExtensions
    {
        public static FlagsEnumerationDescriptor ToFlagsEnumerationDescriptor(this ClassDeclarationSyntax classDecl)
            => FlagsEnumerationDescriptor.Create(classDecl);

        public static QualifiedNameSyntax ToNestedBuilderType(this NameSyntax type)
            => QualifiedName(type, IdentifierName(Names.Builder));

        public static SyntaxToken ToLowerFirstLetter(this SyntaxToken identifier)
            => Identifier(identifier.Text.ToLowerFirstLetter());

        public static string ToLowerFirstLetter(this string name)
            => IsNullOrEmpty(name)
                ? name
                : $"{ToLowerInvariant(name.First())}{name.Substring(1)}";
    }
}
