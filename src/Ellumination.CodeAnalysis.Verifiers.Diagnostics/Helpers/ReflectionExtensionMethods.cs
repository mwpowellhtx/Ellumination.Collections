using System;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace Ellumination.CodeAnalysis.Verifiers
{
    using static MetadataReference;

    /// <summary>
    /// 
    /// </summary>
    public static class ReflectionExtensionMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Assembly GetTypeAssembly(this Type type)
            => type.GetTypeInfo().Assembly;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MetadataReference GetMetadataReferenceFromType(this Type type)
            => CreateFromFile(type.GetTypeAssembly().Location);
    }
}
