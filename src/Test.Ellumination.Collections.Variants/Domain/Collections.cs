using System.Collections.Generic;

namespace Ellumination.Collections.Variants
{
    internal static class Collections
    {
        public static IEnumerable<T> GetRange<T>(params T[] values)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var value in values)
            {
                yield return value;
            }
        }
    }
}
