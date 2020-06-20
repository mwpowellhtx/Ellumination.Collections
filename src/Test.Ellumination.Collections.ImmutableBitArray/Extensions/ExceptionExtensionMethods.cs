using System;

namespace Ellumination.Collections
{
    internal static class ExceptionExtensionmethods
    {
        /// <summary>
        /// Use this method to examine the details of <paramref name="exception"/>.
        /// <paramref name="verify"/> is optional, but is usually something you want to provide
        /// when invoking this method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exception"></param>
        /// <param name="verify"></param>
        /// <returns></returns>
        public static T WithExceptionDetail<T>(this T exception, Action<T> verify = null)
            where T : Exception
        {
            verify?.Invoke(exception);
            return exception;
        }
    }
}
