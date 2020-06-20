using System;
using Microsoft.CodeAnalysis;

namespace Ellumination.CodeAnalysis.Verifiers
{
    using Microsoft.CodeAnalysis.CodeFixes;
    using static LanguageNames;

    /// <summary>
    /// 
    /// </summary>
    /// <inheritdoc />
    public sealed class CodeFixProviderRequiredEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public string Language { get; }

        /// <summary>
        /// Gets or sets the CodeFixProvider.
        /// </summary>
        public CodeFixProvider CodeFixProvider { get; set; }

        internal CodeFixProviderRequiredEventArgs()
            : this(CSharp)
        {
        }

        internal CodeFixProviderRequiredEventArgs(string language)
        {
            Language = language;
        }
    }
}
