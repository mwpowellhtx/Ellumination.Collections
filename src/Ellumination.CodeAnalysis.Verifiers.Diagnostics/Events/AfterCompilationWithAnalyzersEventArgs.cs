using System;

namespace Ellumination.CodeAnalysis.Verifiers
{
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// 
    /// </summary>
    /// <inheritdoc />
    public sealed class AfterCompilationWithAnalyzersEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the Analyzer.
        /// </summary>
        public DiagnosticAnalyzer Analyzer { get; }

        /// <summary>
        /// Gets the <see cref="Microsoft.CodeAnalysis.Diagnostics.CompilationWithAnalyzers"/>.
        /// </summary>
        public CompilationWithAnalyzers CompilationWithAnalyzers { get; }

        internal AfterCompilationWithAnalyzersEventArgs(DiagnosticAnalyzer analyzer
            , CompilationWithAnalyzers compilationWithAnalyzers)
        {
            Analyzer = analyzer;
            CompilationWithAnalyzers = compilationWithAnalyzers;
        }
    }
}
