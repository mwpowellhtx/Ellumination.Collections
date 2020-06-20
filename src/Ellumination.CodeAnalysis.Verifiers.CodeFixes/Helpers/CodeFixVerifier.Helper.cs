using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Ellumination.CodeAnalysis.Verifiers
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.Formatting;
    using Microsoft.CodeAnalysis.Simplification;

    public partial class CodeFixVerifier
    {
        /// <summary>
        /// Apply the given <paramref name="codeAction"/> to the <paramref name="document"/>.
        /// Meant to be used to apply code fixes.
        /// </summary>
        /// <param name="document"><see cref="Document"/> on which to apply the
        /// <paramref name="codeAction"/>.</param>
        /// <param name="codeAction"><see cref="CodeAction"/> that will be applied to the
        /// <paramref name="document"/>.</param>
        /// <returns><see cref="Document"/> with the changes from the <see cref="CodeAction"/>.</returns>
        // ReSharper disable once SuggestBaseTypeForParameter
        private static Document ApplyFix(Document document, CodeAction codeAction)
        {
            var ops = codeAction.GetOperationsAsync(CancellationToken.None).Result;
            var sln = ops.OfType<ApplyChangesOperation>().Single().ChangedSolution;
            return sln.GetDocument(document.Id);
        }

        /// <summary>
        /// Compare two collections of <see cref="Diagnostic"/>, and return a list of any new
        /// diagnostics that appear only in the second collection. Note, considers Diagnostics
        /// to be the same if they have the same Ids.  In the case of multiple diagnostics with
        /// the same Id in a row, this method may not necessarily return the new one.
        /// </summary>
        /// <param name="diagnostics"><see cref="Diagnostic"/> that existed in the code before the
        /// Code Fix was applied.</param>
        /// <param name="newDiagnostics"><see cref="Diagnostic"/> that exist in the code after the
        /// Code Fix was applied.</param>
        /// <returns>The <see cref="Diagnostic"/> that only surfaced in the code after the Code
        /// Fix was applied</returns>
        private static IEnumerable<Diagnostic> FilterNewDiagnostics(IEnumerable<Diagnostic> diagnostics
            , IEnumerable<Diagnostic> newDiagnostics)
        {
            var oldArray = diagnostics.SortDiagnostics().ToArray();
            var newArray = newDiagnostics.SortDiagnostics().ToArray();

            for (int oldIndex = 0, newIndex = 0; newIndex < newArray.Length; newIndex++)
            {
                if (oldIndex < oldArray.Length && oldArray[oldIndex].Id == newArray[newIndex].Id)
                {
                    ++oldIndex;
                }
                else
                {
                    yield return newArray[newIndex];
                }
            }
        }

        /// <summary>
        /// Get the existing compiler <see cref="Diagnostic"/> on the <paramref name="document"/>.
        /// </summary>
        /// <param name="document">The document on which to run the compiler <see cref="DiagnosticAnalyzer"/>.</param>
        /// <returns>The compiler <see cref="Diagnostic"/> that were found in the code.</returns>
        private static IEnumerable<Diagnostic> GetCompilerDiagnostics(Document document)
            => document.GetSemanticModelAsync().Result.GetDiagnostics();

        /// <summary>
        /// Returns the Textual string representing the <paramref name="document"/>.
        /// </summary>
        /// <param name="document">The document to be converted to a string.</param>
        /// <returns>A string containing the syntax of the <paramref name="document"/> after formatting.</returns>
        private static string GetStringFromDocument(Document document)
        {
            var sdoc = Simplifier.ReduceAsync(document, Simplifier.Annotation).Result;
            var root = sdoc.GetSyntaxRootAsync().Result;
            root = Formatter.Format(root, Formatter.Annotation, sdoc.Project.Solution.Workspace);
            return root.GetText().ToString();
        }
    }
}

