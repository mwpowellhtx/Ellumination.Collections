using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace Ellumination.CodeAnalysis.Verifiers
{
    using Microsoft.CodeAnalysis.CSharp;
    using static MetadataReference;

    /// <summary>
    /// 
    /// </summary>
    /// <inheritdoc />
    public sealed class MetadataReferencesRequiredEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the <see cref="IList{T}"/> of <see cref="MetadataReference"/>,
        /// starting with several nominal default references.
        /// </summary>
        public IList<MetadataReference> MetadataReferences { get; }

        internal MetadataReferencesRequiredEventArgs()
        {
            MetadataReferences = new List<MetadataReference>
            {
                GetMetadataReferenceFromType<object>(),
                typeof(Enumerable).GetMetadataReferenceFromType(),
                GetMetadataReferenceFromType<CSharpCompilation>(),
                GetMetadataReferenceFromType<Compilation>()
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static Assembly GetTypeAssembly<T>()
            => typeof(T).GetTypeInfo().Assembly;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public MetadataReference GetMetadataReferenceFromType<T>()
            => CreateFromFile(GetTypeAssembly<T>().Location);
    }
}
