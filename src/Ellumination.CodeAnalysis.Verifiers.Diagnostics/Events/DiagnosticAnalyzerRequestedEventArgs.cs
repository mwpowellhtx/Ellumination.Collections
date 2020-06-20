using System;
using Microsoft.CodeAnalysis;

namespace Ellumination.CodeAnalysis.Verifiers
{
    using Microsoft.CodeAnalysis.Diagnostics;
    using static LanguageNames;

    /// <summary>
    /// Useful when a <see cref="DiagnosticAnalyzer"/> may be required.
    /// </summary>
    /// <inheritdoc />
    public sealed class DiagnosticAnalyzerRequestedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the Language in accordance with <see cref="LanguageNames"/>.
        /// </summary>
        public string Language { get; internal set; }

        /// <summary>
        /// Gets or sets the Analyzer. When a <see cref="DiagnosticAnalyzer"/> is requested,
        /// fill this in, or not, depending on your Verifier requirements.
        /// </summary>
        public DiagnosticAnalyzer Analyzer { get; set; }

        internal DiagnosticAnalyzerRequestedEventArgs()
            : this(CSharp)
        {
        }

        internal DiagnosticAnalyzerRequestedEventArgs(string language)
        {
            Language = language ?? CSharp;
        }
    }
}
