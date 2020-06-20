using System;

namespace Ellumination.Collections.Variants
{
    using Xunit;

    internal static class ExceptionExtensionMethods
    {
        public static TException VerifyException<TException>(this TException exception, Action<TException> action = null)
            where TException : Exception
        {
            Assert.NotNull(exception);
            action?.Invoke(exception);
            return exception;
        }
    }
}
