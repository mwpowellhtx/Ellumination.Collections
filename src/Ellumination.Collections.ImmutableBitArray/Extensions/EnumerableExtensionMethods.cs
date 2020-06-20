using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    internal static class EnumerableExtensionMethods
    {
        public static IEnumerable<T> ReverseTake<T>(this IEnumerable<T> values, int count = 0)
        {
            var i = 0;

            while (i++ < count)
            {
                // ReSharper disable once PossibleMultipleEnumeration
                yield return values.ElementAt(
                    // ReSharper disable once PossibleMultipleEnumeration
                    values.Count() - i - 1);
            }
        }

        [Obsolete("Will probably remove this but keeping it as a reference for the time being")]
		public static IEnumerable<T> TakeFromBack<T>(this IEnumerable<T> values, int count = 0)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            if (count > values.Count())
            {
                yield break;
            }

            // ReSharper disable once PossibleMultipleEnumeration
            foreach (var value in values
                // ReSharper disable once PossibleMultipleEnumeration
                .Skip(values.Count() - count))
            {
                yield return value;
            }
        }
    }
}
