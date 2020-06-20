using System;

namespace Ellumination.CodeAnalysis.Verifiers
{
    /// <summary>
    /// 
    /// </summary>
    /// <inheritdoc />
    public sealed class AssertCodeFixSuccessEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public string ExpectedSource { get; }

        /// <summary>
        /// 
        /// </summary>
        public string GivenSource { get; }

        /// <summary>
        /// 
        /// </summary>
        public string FixedSource { get; }

        internal AssertCodeFixSuccessEventArgs(string givenSource, string fixedSource, string expectedSource)
        {
            ExpectedSource = expectedSource;
            GivenSource = givenSource;
            FixedSource = fixedSource;
        }
    }
}
