using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Ellumination.CodeAnalysis.Verifiers
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    public abstract partial class DiagnosticVerifier : SourceDocumentVerifier
    {
        /// <summary>
        /// Given code <paramref name="sources"/> in the form of strings, their
        /// <paramref name="language"/>, and an <see cref="DiagnosticAnalyzer"/> to apply to
        /// it, return the diagnostics found in the string after converting it to a document.
        /// </summary>
        /// <param name="sources">Code in the form of strings.</param>
        /// <param name="language">The <see cref="LanguageNames"/> the source classes are in.</param>
        /// <param name="analyzer">The analyzer to be run on the <paramref name="sources"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Diagnostic"/> that surface
        /// in the <paramref name="sources"/>, sorted by <see cref="Location"/>.</returns>
        private IEnumerable<Diagnostic> GetSortedDiagnostics(IEnumerable<string> sources
            , string language, DiagnosticAnalyzer analyzer)
            => GetSortedDiagnosticsFromCompiledDocuments(analyzer, GetDocuments(sources, language).ToArray());

        /// <summary>
        /// Given an <paramref name="analyzer"/> and <paramref name="documents"/> to apply it to,
        /// run the <paramref name="analyzer"/> and gather a set of <see cref="Diagnostic"/> found
        /// in it. The returned <see cref="Diagnostic"/> are then ordered by
        /// <see cref="Location"/> in the source <paramref name="documents"/>.
        /// </summary>
        /// <param name="analyzer">The <see cref="DiagnosticAnalyzer"/> to run on the <paramref name="documents"/>.</param>
        /// <param name="documents">The Documents that the <paramref name="analyzer"/> will be run on.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Diagnostic"/> that surface
        /// in the source code, sorted by <see cref="Location"/>.</returns>
        protected IEnumerable<Diagnostic> GetSortedDiagnosticsFromCompiledDocuments(DiagnosticAnalyzer analyzer
            , params Document[] documents)
        {
            CompileDocumentsWithAnalyers(analyzer, out var diagnostics, documents);
            return diagnostics.SortDiagnostics();
        }

        private class ProjectEqualityComparer : IEqualityComparer<Project>
        {
            public int GetHashCode(Project project)
                => (project.Id?.Id ?? Guid.Empty).GetHashCode();

            public bool Equals(Project x, Project y)
                => (!(x == null || y == null))
                   && (ReferenceEquals(x, y)
                       || (x.Id.Id.Equals(y.Id.Id)));
        }

        /// <summary>
        /// Raised After <see cref="Compilation"/> occurs.
        /// </summary>
        public event EventHandler<AfterCompilationEventArgs> AfterCompilation;

        /// <summary>
        /// Raised After <see cref="CompilationWithAnalyzers"/> occurs.
        /// </summary>
        public event EventHandler<AfterCompilationWithAnalyzersEventArgs> AfterCompilationWithAnalyers;

        private void OnAfterCompilation(Compilation compilation)
            => AfterCompilation?.Invoke(this, new AfterCompilationEventArgs(compilation));

        private void OnAfterCompilationWithAnalyzers(DiagnosticAnalyzer analyzer
            , CompilationWithAnalyzers compilationWithAnalyzers)
            => AfterCompilationWithAnalyers?.Invoke(this
                , new AfterCompilationWithAnalyzersEventArgs(analyzer, compilationWithAnalyzers));

        /// <summary>
        /// Raised After <see cref="Diagnostic"/> received from the Compilation.
        /// </summary>
        public event EventHandler<AfterDiagnosticsReceivedEventArgs> AfterDiagnosticsReceived;

        private void OnAfterDiagnosticsReceived(IEnumerable<Diagnostic> diagnostics)
        {
            AfterDiagnosticsReceived?.Invoke(this, new AfterDiagnosticsReceivedEventArgs(diagnostics));
        }

        /// <summary>
        /// Given an <paramref name="analyzer"/> and <paramref name="documents"/> to apply it to,
        /// run the <paramref name="analyzer"/> and gather a set of <see cref="Diagnostic"/> found
        /// in it. The returned <see cref="Diagnostic"/> are then ordered by
        /// <see cref="Location"/> in the source <paramref name="documents"/>.
        /// </summary>
        /// <param name="analyzer">The <see cref="DiagnosticAnalyzer"/> to run on the
        /// <paramref name="documents"/>.</param>
        /// <param name="diagnostics"></param>
        /// <param name="documents">The <see cref="Document"/> that the
        /// <paramref name="analyzer"/> will be run on.</param>
        private void CompileDocumentsWithAnalyers(DiagnosticAnalyzer analyzer
            , out IEnumerable<Diagnostic> diagnostics, params Document[] documents)
        {
            // TODO: TBD: whereas this was initially starting out as a sort of Diagnostic feedback focus, we can still maintain that, but we need to further expose the compilation bits themselves, with and/or without the analysis...

            // Just preclude the same Project being compiled additional times.
            var projects = documents.Select(d => d.Project).Distinct(new ProjectEqualityComparer()).ToArray();

            // TODO: TBD: may not be such a great idea to run this asynchronously all things considered, because if anything is wrong evaluating the output, we see it here instead of at the point of failure.
            OnAfterDiagnosticsReceived(diagnostics = projects.SelectMany(
                x => GetDiagnosticsFromProjectCompilationAsync(x).Result).ToArray());

            Task<IEnumerable<Diagnostic>> GetDiagnosticsFromProjectCompilationAsync(Project project)
            {
                return Task.Run(async () =>
                {
                    // TODO: TBD: how are Generators connected here? somewhere between the Project being provided and the Result, the compilation occurs, which presumably would potentially also invoke the generator, correct?
                    // TODO: TBD: I would need to add the CodeGeneration bits, including DotNetCli references, to the project?

                    var compilation = await project.GetCompilationAsync();

                    OnAfterCompilation(compilation);

                    IEnumerable<Diagnostic> GetDiagnosticsWithoutAnalysis()
                    {
                        var result = compilation.GetDiagnostics().ToArray();

                        foreach (var diagnostic in result)
                        {
                            if (diagnostic.Location == Location.None
                                || diagnostic.Location.IsInMetadata
                                || documents.Select(async document => await document.GetSyntaxTreeAsync())
                                    .Any(tree => tree.Result == diagnostic.Location.SourceTree))
                            {
                                yield return diagnostic;
                            }
                        }
                    }

                    IEnumerable<Diagnostic> GetDiagnosticsWithAnalysis()
                    {
                        var compilationWithAnalyzers = compilation.WithAnalyzers(ImmutableArray.Create(analyzer));

                        OnAfterCompilationWithAnalyzers(analyzer, compilationWithAnalyzers);

                        var result = compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync().Result.ToArray();

                        // TODO: TBD: re-design the API to the "get diagnostic" paradigm...
                        foreach (var diagnostic in result)
                        {
                            // TODO: TBD: and if the async/result does not get too blocked...
                            // TODO: TBD or perhaps there is some sort of observable pattern that could better be used there...
                            if (diagnostic.Location == Location.None
                                || diagnostic.Location.IsInMetadata
                                || documents.Select(async document => await document.GetSyntaxTreeAsync())
                                    .Any(tree => tree.Result == diagnostic.Location.SourceTree))
                            {
                                yield return diagnostic;
                            }
                        }
                    }

                    return analyzer == null ? GetDiagnosticsWithoutAnalysis() : GetDiagnosticsWithAnalysis();
                });
            }
        }
    }
}
