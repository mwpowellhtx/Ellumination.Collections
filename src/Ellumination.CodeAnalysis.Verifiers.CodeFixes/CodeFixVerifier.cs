using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Ellumination.CodeAnalysis.Verifiers
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.Formatting;
    using static String;

    /// <summary>
    /// Parent class of all Unit tests made for diagnostics with code fixes. Contains methods
    /// used to verify correctness of code fixes. Diagnostic Producer class with extra methods
    /// dealing with applying code fixes. Refactored from original project template as services
    /// passed to the test class.
    /// </summary>
    /// <inheritdoc />
    public partial class CodeFixVerifier : DiagnosticVerifier
    {
        /// <summary>
        /// Occurs when a <see cref="CodeFixVerifier"/> is required.
        /// </summary>
        protected event EventHandler<CodeFixProviderRequiredEventArgs> CodeFixProviderRequired;

        private void OnCodeFixProviderRequired(string language, out CodeFixProviderRequiredEventArgs e)
        {
            e = new CodeFixProviderRequiredEventArgs(language);
            CodeFixProviderRequired?.Invoke(this, e);
        }

        /// <summary>
        /// Occurs when verifying the Code Fix.
        /// </summary>
        public event EventHandler<VerifyingFixEventArgs> VerifyingFix;

        private void OnVerifyingFix(string language, string givenSource, string expectedSource
            , int? codeFixIndex, bool allowNewCompilerDiagnostics)
            => VerifyingFix?.Invoke(this, new VerifyingFixEventArgs(language, givenSource, expectedSource
                , codeFixIndex, allowNewCompilerDiagnostics));

        /// <summary>
        /// Occurs when Asserting Code Fix Success.
        /// </summary>
        protected event EventHandler<AssertCodeFixSuccessEventArgs> AssertCodeFixSuccess;

        private void OnAssertCodeFixSuccess(string givenSource, string fixedSource, string expectedSource)
            => AssertCodeFixSuccess?.Invoke(this, new AssertCodeFixSuccessEventArgs(
                givenSource, fixedSource, expectedSource)
            );

        /// <summary>
        /// General verifier for Code Fixes. Creates a <see cref="Document"/> from the source
        /// string, then gets diagnostics on it and applies the relevant Code Fixes. Then gets
        /// the string after the code fix is applied and compares it with the expected result.
        /// Note, if any code fix causes new diagnostics to show up, the test fails unless
        /// <paramref name="allowNewCompilerDiagnostics"/> is set to true.
        /// </summary>
        /// <param name="language">The language the source code is in.</param>
        /// <param name="givenSource">A class in the form of a string before the Code Fix was
        /// applied to it.</param>
        /// <param name="expectedSource">A class in the form of a string after the Code Fix was
        /// applied to it.</param>
        /// <param name="codeFixIndex">Index determining which Code Fix to apply if there are
        /// multiple.</param>
        /// <param name="allowNewCompilerDiagnostics">A bool controlling whether or not the
        /// test will fail if the Code Fix introduces other warnings after being applied.</param>
        /// <exception cref="CompilerDiagnosticsNotAllowedException"></exception>
        public void VerifyFix(string language, string givenSource, string expectedSource
            , int? codeFixIndex = null, bool allowNewCompilerDiagnostics = false)
        {
            if (IsNullOrEmpty(expectedSource))
            {
                return;
            }

            OnVerifyingFix(language, givenSource, expectedSource, codeFixIndex, allowNewCompilerDiagnostics);

            var document = CreateDocument(givenSource, language);

            OnDiagnosticAnalyzerRequested(language, out var analyerRequestedArgs);

            var analyzerDiagnostics = GetSortedDiagnosticsFromCompiledDocuments(analyerRequestedArgs.Analyzer, document);
            var compilerDiagnostics = GetCompilerDiagnostics(document).ToArray();
            // ReSharper disable once PossibleMultipleEnumeration
            var attempts = analyzerDiagnostics.Count();

            OnCodeFixProviderRequired(language, out var codeFixProviderArgs);

            // ReSharper disable once PossibleMultipleEnumeration
            // Check if there are analyzer diagnostics left after the code fix.
            for (var i = 0; i < attempts && analyzerDiagnostics.Any(); ++i)
            {
                var actions = new List<CodeAction>();
                // ReSharper disable once PossibleMultipleEnumeration
                var context = new CodeFixContext(document, analyzerDiagnostics.First()
                    , (a, d) => actions.Add(a), CancellationToken.None);

                codeFixProviderArgs.CodeFixProvider.RegisterCodeFixesAsync(context).Wait();

                if (!actions.Any())
                {
                    break;
                }

                if (codeFixIndex != null)
                {
                    document = ApplyFix(document, actions.ElementAt((int) codeFixIndex));
                    break;
                }

                document = ApplyFix(document, actions.ElementAt(0));
                analyzerDiagnostics = GetSortedDiagnosticsFromCompiledDocuments(analyerRequestedArgs.Analyzer, document);

                var newCompilerDiagnostics = FilterNewDiagnostics(compilerDiagnostics
                    , GetCompilerDiagnostics(document)).SortDiagnostics().ToArray();

                if (allowNewCompilerDiagnostics || !newCompilerDiagnostics.Any())
                {
                    continue;
                }

                // Format and get the compiler diagnostics again so that the locations make sense in the output.
                document = document.WithSyntaxRoot(Formatter.Format(document.GetSyntaxRootAsync().Result
                    , Formatter.Annotation, document.Project.Solution.Workspace));

                newCompilerDiagnostics = FilterNewDiagnostics(compilerDiagnostics
                    , GetCompilerDiagnostics(document)).SortDiagnostics().ToArray();

                throw new CompilerDiagnosticsNotAllowedException(
                    "Fix introduced new compiler diagnostics:"
                    + $"\r\n{Join("\r\n", newCompilerDiagnostics.Select(d => $"{d}"))}"
                    + $"\r\n\r\nNew document:\r\n{document.GetSyntaxRootAsync().Result.ToFullString()}");
            }

            // After applying all of the Code Fixes, compare the resulting string to the inputted one.
            var fixedSource = GetStringFromDocument(document);

            OnAssertCodeFixSuccess(givenSource, fixedSource, expectedSource);
        }
    }
}
