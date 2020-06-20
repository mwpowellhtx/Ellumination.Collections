using System.Threading;

namespace Ellumination.Collections
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using static Microsoft.CodeAnalysis.SyntaxTokenList;
    using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
    using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;
    using static BitwiseOperatorOverloadsPartialGenerator.OperatorMemberNames;

    internal class BitwiseOperatorOverloadsPartialGenerator : PartialGeneratorBase
    {
        private BitwiseOperatorOverloadsPartialGenerator(FlagsEnumerationDescriptor descriptor
            , CancellationToken cancellationToken)
            : base(descriptor, cancellationToken)
        {
        }

        internal static class OperatorMemberNames
        {
            /// <summary>
            /// &quot;BitwiseNot&quot;
            /// </summary>
            public const string BitwiseNot = nameof(BitwiseNot);

            /// <summary>
            /// &quot;BitwiseAnd&quot;
            /// </summary>
            public const string BitwiseAnd = nameof(BitwiseAnd);

            /// <summary>
            /// &quot;BitwiseOr&quot;
            /// </summary>
            public const string BitwiseOr = nameof(BitwiseOr);

            /// <summary>
            /// &quot;BitwiseXor&quot;
            /// </summary>
            public const string BitwiseXor = nameof(BitwiseXor);
        }

        public static MemberDeclarationSyntax Generate(FlagsEnumerationDescriptor descriptor
            , CancellationToken cancellationToken)
            => new BitwiseOperatorOverloadsPartialGenerator(descriptor, cancellationToken)
                .GenerateMemberDeclaration(descriptor);

        protected override SyntaxList<MemberDeclarationSyntax> GenerateMembers()
        {
            /* TODO: TBD: it may be worth having a reference to the Enumerations project here... However,
             in order to do this, I need to target .NET Standard 1.6, as well as .NET Standard 2.0, in the
             Ellumination.Collections.Enumerations, etc, project(s). Then I may be able to leverage things like
             nameof(BitwiseNot) instead of hard-coding the concerns here... */

            return SingletonList(GeneratePrivateBytesCtor(Descriptor))
                    .Add(GenerateUnaryOperatorOverload(Descriptor, BitwiseNot, TildeToken))
                    .Add(GenerateBinaryOperatorOverload(Descriptor, BitwiseAnd, AmpersandToken))
                    .Add(GenerateBinaryOperatorOverload(Descriptor, BitwiseOr, BarToken))
                    .Add(GenerateBinaryOperatorOverload(Descriptor, BitwiseXor, CaretToken))
                ;

            //// TODO: TBD: So, we can generate a UsingDeclarationSyntax, however, we may want to fully qualify any base classes...
            //MemberDeclarationSyntax GenerateUsingStatement(FlagsEnumerationDescriptor descriptor)
            //{
            //    return UsingStatement(EmptyStatement())
            //        .WithOpenParenToken(MissingToken(OpenParenToken))
            //            .WithExpression(MemberAccessExpression(
            //                SimpleMemberAccessExpression
            //                , IdentifierName("Ellumination")
            //                , IdentifierName("Collections")
            //                ))
            //        .WithCloseParenToken(MissingToken(CloseParenToken))
            //        ;
            //}

            // TODO: TBD: let's run with this for now; we'll find out soon enough whether this is at all accurate...
            MemberDeclarationSyntax GeneratePrivateBytesCtor(FlagsEnumerationDescriptor descriptor)
            {
                const string bytes = nameof(bytes);

                // Literally: "byte[] bytes"
                var bytesSyntax = Parameter(Identifier(bytes))
                        .WithType(
                            ArrayType(PredefinedType(Token(ByteKeyword)))
                                // Not sure what these are all about, but we'll run with this anyway...
                                .WithRankSpecifiers(SingletonList(
                                    ArrayRankSpecifier(
                                        SingletonSeparatedList<ExpressionSyntax>(OmittedArraySizeExpression())
                                    )
                                ))
                        )
                    ;

                // Opening with: $"private {Descriptor.Type}({bytesSyntax})"
                return ConstructorDeclaration(descriptor.TypeIdentifier)
                        .WithModifiers(Create(Token(PrivateKeyword)))
                        .WithParameterList(ParameterList().AddParameters(bytesSyntax))
                        .WithInitializer(ConstructorInitializer(BaseConstructorInitializer
                            , ArgumentList(
                                SingletonSeparatedList(Argument(IdentifierName(bytesSyntax.Identifier)))
                            )
                        ))
                        // No body, per se, or, rather, no statements in the body: i.e. "{ }"
                        .WithBody(Block())
                    ;
            }

            // With special reference: https://roslynquoter.azurewebsites.net/
            MemberDeclarationSyntax GenerateUnaryOperatorOverload(FlagsEnumerationDescriptor descriptor
                , string callback, SyntaxKind opKind)
            {
                const string other = nameof(other);

                var otherSyntax = Parameter(Identifier(other))
                        .WithType(descriptor.Type)
                    ;

                // Opening with: $"public static {Descriptor.Type} operator {opKind}({Descriptor.Type} {otherSyntax})"
                return OperatorDeclaration(descriptor.Type, Token(opKind))
                        .WithModifiers(Create(Token(PublicKeyword)).Add(Token(StaticKeyword)))
                        .WithParameterList(ParameterList(SingletonSeparatedList(otherSyntax)))
                        // Literally like saying: $" => {otherSyntax}?.{callback}()"
                        .WithExpressionBody(ArrowExpressionClause(
                            ConditionalAccessExpression(IdentifierName(otherSyntax.Identifier)
                                , InvocationExpression(MemberBindingExpression(IdentifierName(callback)))
                                    .AddArgumentListArguments()
                            )
                        ))
                        // Yes, from what I can determine, even tokens such as ";" are not assumed.
                        .WithSemicolonToken(Token(SemicolonToken))
                    ;
            }

            MemberDeclarationSyntax GenerateBinaryOperatorOverload(FlagsEnumerationDescriptor descriptor
                , string callback, SyntaxKind opKind)
            {
                const string a = nameof(a);
                const string b = nameof(b);

                var aSyntax = Parameter(Identifier(a))
                        .WithType(descriptor.Type)
                    ;

                var bSyntax = Parameter(Identifier(b))
                        .WithType(descriptor.Type)
                    ;

                // Opening with: $"public static {Descriptor.Type} operator {opKind}({Descriptor.Type} {aSyntax}, {Descriptor.Type} {bSyntax})"
                return OperatorDeclaration(descriptor.Type, Token(opKind))
                        .WithModifiers(Create(Token(PublicKeyword)).Add(Token(StaticKeyword)))
                        // TODO: TBD: I think this should take care of the Comma-delimiting issue?
                        .WithParameterList(ParameterList().AddParameters(aSyntax, bSyntax))
                        // Literally like saying: $" => {aSyntax}?.{callback}({bSyntax})"
                        .WithExpressionBody(ArrowExpressionClause(
                            ConditionalAccessExpression(IdentifierName(aSyntax.Identifier)
                                , InvocationExpression(MemberBindingExpression(IdentifierName(callback)))
                                    .AddArgumentListArguments(Argument(IdentifierName(bSyntax.Identifier)))
                            )
                        ))
                        // Yes, from what I can determine, even tokens such as ";" are not assumed.
                        .WithSemicolonToken(Token(SemicolonToken))
                    ;
            }
        }
    }
}
