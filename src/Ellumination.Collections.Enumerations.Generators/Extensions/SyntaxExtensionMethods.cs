using System.Linq;

namespace Ellumination.Collections
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    /* TODO: TBD: this is pretty much copied/pasted from
      * https://github.com/amis92/RecordGenerator/blob/master/src/Amadevus.RecordGenerator.Generators/SyntaxExtensions.cs
      * As such, not sure we would use any of these, may use some of them, and may discover a need for others better suited
      * to the task at hand... */
    // TODO: TBD: so, likely need very few of these, perhaps; will keep them commented, and there may be one or two that end up being useful, like for semi-colon tokens, etc...
    internal static class SyntaxExtensionMethods
    {
        //public static MethodDeclarationSyntax AddModifiers(this MethodDeclarationSyntax methodDecl
        //    , params SyntaxKind[] modifier)
        //    => methodDecl.AddModifiers(modifier.Select(Token).ToArray());

        //public static PropertyDeclarationSyntax AddModifiers(this PropertyDeclarationSyntax methodDecl
        //    , params SyntaxKind[] modifier)
        //    => methodDecl.AddModifiers(modifier.Select(Token).ToArray());

        //public static ConstructorDeclarationSyntax AddModifiers(this ConstructorDeclarationSyntax methodDecl
        //    , params SyntaxKind[] modifier)
        //    => methodDecl.AddModifiers(modifier.Select(Token).ToArray());

        //public static ClassDeclarationSyntax AddModifiers(this ClassDeclarationSyntax classDecl
        //    , params SyntaxKind[] modifier)
        //    => classDecl.AddModifiers(modifier.Select(Token).ToArray());

        //public static PropertyDeclarationSyntax WithSemicolonToken(this PropertyDeclarationSyntax propertyDecl)
        //    => propertyDecl.WithSemicolonToken(Token(SyntaxKind.SemicolonToken));

        //public static AccessorDeclarationSyntax WithSemicolonToken(this AccessorDeclarationSyntax accessorDecl)
        //    => accessorDecl.WithSemicolonToken(Token(SyntaxKind.SemicolonToken));

        //public static MethodDeclarationSyntax WithSemicolonToken(this MethodDeclarationSyntax methodDecl)
        //    => methodDecl.WithSemicolonToken(Token(SyntaxKind.SemicolonToken));

        //public static ConstructorDeclarationSyntax WithParameters(this ConstructorDeclarationSyntax ctorDecl
        //    , IEnumerable<ParameterSyntax> parameters)
        //    => ctorDecl.WithParameterList(ParameterList(SeparatedList(parameters)));

        //public static ConstructorDeclarationSyntax WithBodyStatements(this ConstructorDeclarationSyntax ctorDecl
        //    , IEnumerable<StatementSyntax> parameters)
        //    => ctorDecl.WithBody(Block(parameters));

        //public static ConstructorDeclarationSyntax WithBodyStatements(this ConstructorDeclarationSyntax ctorDecl
        //    , params StatementSyntax[] parameters)
        //    => ctorDecl.WithBody(Block(parameters));

        //public static MethodDeclarationSyntax WithParameters(this MethodDeclarationSyntax methodDecl
        //    , IEnumerable<ParameterSyntax> parameters)
        //    => methodDecl.WithParameterList(ParameterList(SeparatedList(parameters)));

        //public static MethodDeclarationSyntax WithParameters(this MethodDeclarationSyntax methodDecl
        //    , params ParameterSyntax[] parameters)
        //    => methodDecl.WithParameterList(ParameterList(SeparatedList(parameters)));

        //public static MethodDeclarationSyntax WithBodyStatements(this MethodDeclarationSyntax methodDecl
        //    , IEnumerable<StatementSyntax> parameters)
        //    => methodDecl.WithBody(Block(parameters));

        //public static MethodDeclarationSyntax WithBodyStatements(this MethodDeclarationSyntax methodDecl
        //    , params StatementSyntax[] parameters)
        //    => methodDecl.WithBody(Block(parameters));

        //public static MethodDeclarationSyntax WithExpressionBody(this MethodDeclarationSyntax methodDecl
        //    , ExpressionSyntax body)
        //    => methodDecl.WithExpressionBody(ArrowExpressionClause(body))
        //        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));

        //public static PropertyDeclarationSyntax WithAccessors(this PropertyDeclarationSyntax propertyDecl
        //    , params AccessorDeclarationSyntax[] parameters)
        //    => propertyDecl.WithAccessorList(AccessorList(List(parameters)));

        //public static bool IsPropertyViable(this PropertyDeclarationSyntax propertyDecl)
        //    => propertyDecl.HasOnlyGetterWithNoBody()
        //       && propertyDecl.IsPublic()
        //       && !propertyDecl.IsStatic();

        //public static bool IsNamed(this AttributeSyntax attrib, string name)
        //    => attrib.Name is IdentifierNameSyntax id
        //       && (id.Identifier.Text == name || id.Identifier.Text == name + "Attribute");

        //public static bool HasOnlyGetterWithNoBody(this PropertyDeclarationSyntax propertyDecl)
        //    => propertyDecl.AccessorList is AccessorListSyntax accList
        //       && accList.Accessors.Count == 1
        //       && accList.Accessors.Single().IsGetterWithNoBody();

        //public static bool IsGetterWithNoBody(this AccessorDeclarationSyntax accessorDecl)
        //    => accessorDecl.Kind() == SyntaxKind.GetAccessorDeclaration && accessorDecl.Body == null;

        //public static bool IsPublic(this PropertyDeclarationSyntax propertyDecl)
        //    => propertyDecl.Modifiers.Any(x => x.Kind() == SyntaxKind.PublicKeyword);

        //public static bool IsStatic(this PropertyDeclarationSyntax propertyDecl)
        //    => propertyDecl.Modifiers.Any(x => x.Kind() == SyntaxKind.StaticKeyword);

        public static SyntaxToken WithoutTrivia(this SyntaxToken syntax)
            => syntax.WithLeadingTrivia(TriviaList()).WithTrailingTrivia(TriviaList());

        public static NameSyntax GetTypeSyntax(this TypeDeclarationSyntax typeDecl)
        {
            var identifier = typeDecl.Identifier.WithoutTrivia();
            var typeParamList = typeDecl.TypeParameterList;
            if (typeParamList == null)
            {
                return IdentifierName(identifier);
            }

            var arguments = typeParamList.Parameters.Select(param => IdentifierName(param.Identifier));
            var typeArgList = TypeArgumentList(SeparatedList<TypeSyntax>(arguments));
            return GenericName(identifier, typeArgList);
        }

        //public static bool IsImmutableArrayType(this PropertyDeclarationSyntax propertyDecl)
        //    => propertyDecl.Type is GenericNameSyntax genericName &&
        //       genericName.Identifier.Text == "ImmutableArray";

        //public static TypeSyntax WithNamespace(this SimpleNameSyntax nameSyntax, string @namespace)
        //    => QualifiedName(ParseName(@namespace), nameSyntax);
    }
}
