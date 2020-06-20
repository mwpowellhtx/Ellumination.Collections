using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Ellumination.CodeAnalysis.Verifiers
{
    /// <summary>
    /// 
    /// </summary>
    public static class MetadataReferenceExtensionMethods
    {
        /// <summary>
        /// Adds the Range of <paramref name="items"/> to the <paramref name="list"/>
        /// and returns the <paramref name="list"/>.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IList<MetadataReference> AddRange(this IList<MetadataReference> list
            , params MetadataReference[] items)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }

            return list;
        }
    }
}
