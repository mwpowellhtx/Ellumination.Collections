using System;

namespace Ellumination.CodeAnalysis.Verifiers
{
    /// <summary>
    /// 
    /// </summary>
    /// <inheritdoc />
    public sealed class VerifyingFixEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public string Language { get; }

        /// <summary>
        /// 
        /// </summary>
        public string GivenSource { get; }

        /// <summary>
        /// 
        /// </summary>
        public string ExpectedSource { get; }

        /// <summary>
        /// 
        /// </summary>
        public int? CodeFixIndex { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool AllowNewCompilerDiagnostics { get; }

        internal VerifyingFixEventArgs(string language, string givenSource, string expectedSource
            , int? codeFixIndex, bool allowNewCompilerDiagnostics)
        {
            Language = language;
            GivenSource = givenSource;
            ExpectedSource = expectedSource;
            CodeFixIndex = codeFixIndex;
            AllowNewCompilerDiagnostics = allowNewCompilerDiagnostics;
        }
    }
}
