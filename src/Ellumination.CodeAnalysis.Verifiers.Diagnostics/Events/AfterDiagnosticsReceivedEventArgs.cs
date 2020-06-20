using System;
using System.Collections.Generic;

namespace Ellumination.CodeAnalysis.Verifiers
{
    using Microsoft.CodeAnalysis;

    /// <summary>
    /// 
    /// </summary>
    /// <inheritdoc />
    public sealed class AfterDiagnosticsReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the <see cref="Microsoft.CodeAnalysis.Diagnostics"/>.
        /// </summary>
        public IEnumerable<Diagnostic> Diagnostics { get; }

        internal AfterDiagnosticsReceivedEventArgs(IEnumerable<Diagnostic> diagnostics)
        {
            Diagnostics = diagnostics;
        }
    }
}
