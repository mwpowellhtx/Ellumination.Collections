using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Ellumination.Collections
{
    using CodeAnalysis;
    using CodeAnalysis.Verifiers;
    using CodeGeneration.Roslyn;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Xunit;
    using Xunit.Abstractions;
    using static LanguageNames;
    using static SyntaxFactory;
    using static SyntaxKind;

    public class FlagsEnumerationGeneratorTests : IClassFixture<EnumerationCompilationDiagnosticVerifier>, IDisposable
    {
        private static readonly string ProjectDirectory
            = typeof(FlagsEnumerationGeneratorTests).Assembly.Location;

        private class DiagnosticProgress : Progress<Diagnostic>
        {
        }

        private EnumerationCompilationDiagnosticVerifier Verifier { get; }

        private ITestOutputHelper OutputHelper { get; }

        public FlagsEnumerationGeneratorTests(EnumerationCompilationDiagnosticVerifier verifier
            , ITestOutputHelper outputHelper)
        {
            Verifier = verifier;
            OutputHelper = outputHelper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number">Number does not serve any purposes other than to signal to
        /// the runner that this is actually a different test case, since comparisons of
        /// <paramref name="source"/> are problematic at best.</param>
        /// <param name="source"></param>
        /// <param name="language"></param>
        [Theory, MemberData(nameof(TestData))]
        public void Test(int number, string source, string language)
        {
            // TODO: TBD: there's a couple of corner cases I could investigate a bit further, and some work to do in the setup/integration of code generation, but this is a fair bit of progress made toward these goals...
            Assert.True(number > 0);

            OutputHelper.WriteLine($"Testing for source: <source>{source}</source>");

            void VerifyExpectedSyntaxTree(IEnumerable<SyntaxTree> trees)
            {
                // ReSharper disable once PossibleMultipleEnumeration
                Assert.Single(trees);
                // ReSharper disable once PossibleMultipleEnumeration
                var tree = trees.First();

                // TODO: TBD: now identify the expected class definition, and the expected FlagsEnumerationAttribute...
                var root = tree.GetRootAsync().Result as CompilationUnitSyntax;
                Assert.NotNull(root);

                /* TODO: TBD: While we could technically see more complex Namespace use cases, for
                 now, let's just assume the simplest of the cases. We may need to establish more
                 robust support at a later time. */

                // Not vet some introductory level bits...
                Assert.Single(root.Members);
                var myClassesNamespace = root.Members.First() as NamespaceDeclarationSyntax;
                Assert.NotNull(myClassesNamespace);
                Assert.Equal("MyClasses", $"{myClassesNamespace.Name}");

                Assert.Single(myClassesNamespace.Members);
                var classDecl = myClassesNamespace.Members.First() as ClassDeclarationSyntax;
                Assert.NotNull(classDecl);

                Assert.Equal("CardinalDirection", $"{classDecl.Identifier}");

                // Now are get into the nitty gritty...
                Assert.Single(classDecl.AttributeLists);
                var flagsEnumAttrList = classDecl.AttributeLists.First();

                Assert.Single(flagsEnumAttrList.Attributes);
                var flagsEnumAttr = flagsEnumAttrList.Attributes.First();
                Assert.True(
                    $"{flagsEnumAttr}" == "FlagsEnumeration"
                    || $"{flagsEnumAttr}" == nameof(FlagsEnumerationAttribute)
                );
            }

            // ReSharper disable once SuggestBaseTypeForParameter
            void VerifyGeneratedMember(MemberDeclarationSyntax memberDecl)
            {
                // Here is a Descriptive set of Modifier Tokens.
                var ctorModifierTokens = new[] {PrivateKeyword};
                var opModifierTokens = new[] {PublicKeyword, StaticKeyword};

                // Let's also provide a bit more Descriptive Context for the verification.
                const SyntaxKind bitwiseComplementOp = TildeToken;
                const SyntaxKind bitwiseAndOp = AmpersandToken;
                const SyntaxKind bitwiseOrOp = BarToken;
                const SyntaxKind bitwiseXorOp = CaretToken;

                /* TODO: TBD: the only down side to this is that if one aspect breaks they all
                 break, so we lose a bit of discernment potentially, i.e. with one error on line
                 109 for where ever the issue actually originated... */

                // It works quite well; VERY fluent, and VERY declarative.
                Assert.True(memberDecl.IsA<ClassDeclarationSyntax>(
                        classDecl => classDecl.HasAllMembers<ConstructorDeclarationSyntax>(
                            ctorDecl => ctorDecl.Modifiers.HasAllSyntaxTokens(ctorModifierTokens)
                                        && ctorDecl.HasExactParameters(
                                            bytes => $"{bytes.Type}" == "byte[]"
                                                     && $"{bytes.Identifier}" == nameof(bytes)
                                        )
                                        && classDecl.HasAllMembers<OperatorDeclarationSyntax>(
                                            opDecl => opDecl.Modifiers.HasExactSyntaxTokens(opModifierTokens)
                                                      && $"{opDecl.OperatorToken}" == $"{Token(bitwiseComplementOp)}"
                                                      && opDecl.HasExactParameters(
                                                          other =>
                                                              $"{other.Type}" == $"{classDecl.Identifier}"
                                                              && $"{other.Identifier}" == nameof(other)
                                                      ),
                                            opDecl => opDecl.Modifiers.HasExactSyntaxTokens(opModifierTokens)
                                                      && $"{opDecl.OperatorToken}" == $"{Token(bitwiseAndOp)}"
                                                      && opDecl.HasExactParameters(
                                                          a =>
                                                              $"{a.Type}" == $"{classDecl.Identifier}"
                                                              && $"{a.Identifier}" == nameof(a),
                                                          b =>
                                                              $"{b.Type}" == $"{classDecl.Identifier}"
                                                              && $"{b.Identifier}" == nameof(b)
                                                      ),
                                            opDecl => opDecl.Modifiers.HasExactSyntaxTokens(opModifierTokens)
                                                      && $"{opDecl.OperatorToken}" == $"{Token(bitwiseOrOp)}"
                                                      && opDecl.HasExactParameters(
                                                          a =>
                                                              $"{a.Type}" == $"{classDecl.Identifier}"
                                                              && $"{a.Identifier}" == nameof(a),
                                                          b =>
                                                              $"{b.Type}" == $"{classDecl.Identifier}"
                                                              && $"{b.Identifier}" == nameof(b)
                                                      ),
                                            opDecl => opDecl.Modifiers.HasExactSyntaxTokens(opModifierTokens)
                                                      && $"{opDecl.OperatorToken}" == $"{Token(bitwiseXorOp)}"
                                                      && opDecl.HasExactParameters(
                                                          a =>
                                                              $"{a.Type}" == $"{classDecl.Identifier}"
                                                              && $"{a.Identifier}" == nameof(a),
                                                          b =>
                                                              $"{b.Type}" == $"{classDecl.Identifier}"
                                                              && $"{b.Identifier}" == nameof(b)
                                                      )
                                        )
                        )
                    )
                );
            }

            void VerifyExpectedSymbols(CSharpCompilation compilation)
            {
                Assert.NotNull(compilation);

                var classDecl = ((CompilationUnitSyntax) compilation.SyntaxTrees.First().GetRoot())
                    .Members.OfType<NamespaceDeclarationSyntax>().First()
                    .Members.OfType<ClassDeclarationSyntax>().First();

                // TODO: TBD: so far so good... now, how do we arrive at Microsoft.CodeAnalysis.AttributeData, from either the Syntax Tree, or what have you ?
                // TODO: TBD: drill into the class and corresponding attribute as quickly as possible...
                var cardinalDirectionSymbol = compilation.GetSymbolsWithName(
                    name => name == "CardinalDirection").SingleOrDefault();

                Assert.NotNull(cardinalDirectionSymbol);
                var flagsEnumAttrData = cardinalDirectionSymbol.GetAttributes().SingleOrDefault(
                    attr => attr.AttributeClass.Name == nameof(FlagsEnumerationAttribute)
                );

                // TODO: TBD: This will be fed into the Generator...
                Assert.NotNull(flagsEnumAttrData);

                // TODO: TBD: these bits seem like more of an integration test of the entire Code Generation pipeline...
                // TODO: TBD: I want a more granular unit test of the code generation itself, at least to start with...
                var generator = new FlagsEnumerationGenerator(flagsEnumAttrData);

                var semanticModel = compilation.GetSemanticModel(compilation.SyntaxTrees.First());

                // Ad-hoc helper function...
                IEnumerable<T> GetRange<T>(params T[] items)
                {
                    foreach (var item in items)
                    {
                        yield return item;
                    }
                }

                // TODO: TBD: would it be at all appropriate to inject some Using Directives here?
                var context = new TransformationContext(classDecl, semanticModel, compilation, ProjectDirectory
                    , GetRange<UsingDirectiveSyntax>(), GetRange<ExternAliasDirectiveSyntax>());

                var members = generator.GenerateAsync(context, new DiagnosticProgress(), CancellationToken.None).Result;

                Assert.Single(members);

                /* We need to Normalize the Member Whitespace, apparently. This is
                 in lieu of CodeGeneration.Roslyn performing this step for us. */

                VerifyGeneratedMember(members.First().NormalizeWhitespace());
            }

            void OnAfterCompilation(object sender, AfterCompilationEventArgs e)
            {
                // Verify this approaching the meat of the issue.
                Assert.Equal(language, e.Compilation.Language);
                Assert.Empty(e.Compilation.GetDiagnostics().ToList());

                VerifyExpectedSyntaxTree(e.Compilation.SyntaxTrees);

                VerifyExpectedSymbols(e.Compilation as CSharpCompilation);
            }

            void OnDiagnosticsReceived(object sender, AfterDiagnosticsReceivedEventArgs e)
            {
                // The Compilation was, after all, successful.
                Assert.Empty(e.Diagnostics);
            }

            try
            {
                Verifier.AfterDiagnosticsReceived += OnDiagnosticsReceived;
                Verifier.AfterCompilation += OnAfterCompilation;

                /* So, our objective here is not to vet any Analyzers, much less the Diagnostics,
                 but rather to work with the resulting Compilation as a basis for verifying the
                 Generators. */

                // TODO: TBD: may be a Theory with ascribed data in the making here...
                // TODO: TBD: should handle not only [FlagsEnumeration] but also [FlagsEnumerationAttribute]

                // TODO: TBD: the "source" here could potentially live in a resource; at minimum in a MemberData, potentially...
                Verifier.VerifyDiagnostics(language, source);
            }
            finally
            {
                Verifier.AfterDiagnosticsReceived -= OnDiagnosticsReceived;
                Verifier.AfterCompilation -= OnAfterCompilation;
            }
        }

        public static IEnumerable<object[]> TestData { get; } = GetTestData().ToArray();

        private static IEnumerable<object[]> GetTestData()
        {
            const string language = CSharp;

            IEnumerable<string> GetSources()
            {
                const string s = @"using Ellumination.Collections;

namespace MyClasses
{
    [FlagsEnumeration]
    public partial class CardinalDirection : Enumeration<CardinalDirection>
    {
        public static readonly CardinalDirection North = new CardinalDirection();

        public static readonly CardinalDirection East = new CardinalDirection();

        public static readonly CardinalDirection South = new CardinalDirection();

        public static readonly CardinalDirection West = new CardinalDirection();

        static CardinalDirection() => InitializeBits(Values);

        private CardinalDirection()
        {
        }
    }
}";
                yield return s;
                yield return s.Replace("[FlagsEnumeration]", "[FlagsEnumerationAttribute]");
            }

            var count = 0;

            return GetSources().Select(s => new object[] {++count, s, language}).ToArray();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
