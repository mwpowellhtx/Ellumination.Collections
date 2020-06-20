using System;

namespace Ellumination.CodeAnalysis.Verifiers
{
    using Microsoft.CodeAnalysis;

    /// <summary>
    /// 
    /// </summary>
    /// <inheritdoc />
    public sealed class AfterCompilationEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the <see cref="Microsoft.CodeAnalysis.Compilation"/>.
        /// </summary>
        public Compilation Compilation { get; }

        internal AfterCompilationEventArgs(Compilation compilation)
        {
            Compilation = compilation;
        }
    }
}
