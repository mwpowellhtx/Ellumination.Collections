using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ellumination.Collections
{
    // TODO: TBD: use our Code.Generation.Roslyn instead?
    using CodeGeneration.Roslyn;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Validation;
    using static BitwiseOperatorOverloadsPartialGenerator;
    using static Microsoft.CodeAnalysis.Diagnostic;
    using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
    using static Category;
    using static Microsoft.CodeAnalysis.DiagnosticSeverity;

    /// <summary>
    ///
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public class FlagsEnumerationGenerator : ICodeGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeData"></param>
        // ReSharper disable once UnusedMember.Global, UnusedParameter.Local
        public FlagsEnumerationGenerator(AttributeData attributeData)
        {
            attributeData.RequiresNotNull(nameof(attributeData));
        }

        private const string IdPrefix = "ElluminationCollectionsEnumerations";
        private const string HelpUriBase = "https://github/mwpowellhtx/Ellumination.Collections/wiki/analyzers/rules/";

        private static DiagnosticDescriptor Rule(int id, string title, Category category
            , DiagnosticSeverity defaultSeverity, string messageFormat, string description = null)
        {
            const bool isEnabledByDefault = true;
            return new DiagnosticDescriptor($"{IdPrefix}{id}", title, messageFormat, $"{category}"
                , defaultSeverity, isEnabledByDefault, description, $"{HelpUriBase}{id}");
        }

        // ReSharper disable once InconsistentNaming
        private static DiagnosticDescriptor X2000_FlagsEnumerationBitwiseOperatorsCodeGen { get; }
            = Rule(2000, "Flags Enumeration Bitwise Operators Code Generation", CodeGen
                , Info, "Generating Flags Enumeration Bitwise Operators code");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="progress"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<SyntaxList<MemberDeclarationSyntax>> GenerateAsync(TransformationContext context
            , IProgress<Diagnostic> progress, CancellationToken cancellationToken)
        {
            // TODO: TBD: well, it did in fact report the result, didn't it?
            progress.Report(Create(X2000_FlagsEnumerationBitwiseOperatorsCodeGen
                , context.ProcessingNode.GetLocation()));

            // We do not just expect a Member, but the Type, returned here.
            MemberDeclarationSyntax GenerateFlagsEnumerationPartial(FlagsEnumerationDescriptor d) => Generate(d, cancellationToken);

            return Task.Run(() =>
            {
                var generatedMembers = List<MemberDeclarationSyntax>();

                // ReSharper disable once InvertIf
                if (context.ProcessingNode is ClassDeclarationSyntax classDecl)
                {
                    var descriptor = classDecl.ToFlagsEnumerationDescriptor();
                    generatedMembers = generatedMembers.Add(GenerateFlagsEnumerationPartial(descriptor));
                }

                return generatedMembers;
            }, cancellationToken);
        }
    }
}
