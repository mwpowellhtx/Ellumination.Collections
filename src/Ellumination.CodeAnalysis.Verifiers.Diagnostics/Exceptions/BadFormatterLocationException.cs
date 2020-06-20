namespace Ellumination.CodeAnalysis.Verifiers
{
    using Microsoft.CodeAnalysis;

    /// <summary>
    /// 
    /// </summary>
    /// <inheritdoc />
    public sealed class BadFormatterLocationException : VerificationException
    {
        /// <summary>
        /// 
        /// </summary>
        public Diagnostic Diagnostic { get; }

        internal BadFormatterLocationException(string message, VerificationResult result
            , Diagnostic diagnostic)
            : base(message, result)
        {
            Diagnostic = diagnostic;
        }
    }
}
