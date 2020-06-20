using System;

namespace Ellumination.Collections
{
    /// <summary>
    /// Bit array extension methods provided for purposes of supporting Enumeration behavior.
    /// </summary>
    internal static class ReflectionExtensionMethods
    {
        /// <summary>
        /// Returns whether the <see cref="Type"/> IsStatic. Effectively this means
        /// that the <see cref="Type.IsAbstract"/> and <see cref="Type.IsSealed"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsStatic(this Type type)
        {
            return type.IsAbstract && type.IsSealed;
        }
    }
}
