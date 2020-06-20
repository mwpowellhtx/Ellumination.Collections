using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using static Math;

    public partial class ImmutableBitArray
    {
        /// <summary>
        /// Returns the Comparison of <paramref name="a"/> with <paramref name="b"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static int CompareTo(IList<byte> a, IList<byte> b)
        {
            const int greater = 1;

            if (a != null && b == null)
            {
                return greater;
            }

            const int lesser = -1;

            if (b != null && a == null)
            {
                return lesser;
            }

            // Accounts for the longer BitArray.
            bool IsLongerAndGreater(IEnumerable<byte> l, IEnumerable<byte> r)
            // ReSharper disable once PossibleMultipleEnumeration
                => l.Count()
                   // ReSharper disable once PossibleMultipleEnumeration
                   > r.Count()
                   // ReSharper disable once PossibleMultipleEnumeration
                   && l.Skip(
                       // ReSharper disable once PossibleMultipleEnumeration
                       r.Count()).Any(x => x != 0);

            // If any Bits at all are set beyond the Other, consider that one Greater.
            if (IsLongerAndGreater(a, b))
            {
                return greater;
            }

            if (IsLongerAndGreater(b, a))
            {
                return lesser;
            }

            /* Comparing each Byte is insufficient in this case. What we actually want to
             * determine is whether there are any bits set in a greater position that are not set
             * in the base instance. */

            // Compare both ends of the equation for Lesser or Greater outcomes.
            int? CompareSameLength(IEnumerable<byte> x, IEnumerable<byte> y, int result)
            {
                int i;

                // Nothing to do in this loop other than determine the boundary Index.
                // ReSharper disable once PossibleMultipleEnumeration
                for (i = x.Count() - 1;
                    // ReSharper disable once PossibleMultipleEnumeration
                    i >= 0 && x.ElementAt(i) == 0;
                    i--)
                {
                }

                // ReSharper disable once InvertIf
                if (i >= 0)
                {
                    // We may return early when there are any Bits set in B beyond the Boundary Byte.
                    // ReSharper disable once PossibleMultipleEnumeration
                    if (y.Skip(i + 1).Any(z => z != 0))
                    {
                        return result;
                    }

                    // ReSharper disable once PossibleMultipleEnumeration
                    if (x.ElementAt(i)
                            // ReSharper disable once PossibleMultipleEnumeration
                            .CompareTo(y.ElementAt(i)) is int zz && zz != 0)
                    {
                        return zz / Abs(zz);
                    }
                }

                int? Indeterminate() => null;

                // Otherwise, we must examine the Boundary Byte itself.
                return Indeterminate();
            }

            const int equal = 0;

            return CompareSameLength(a, b, lesser)
                   ?? CompareSameLength(b, a, greater)
                   ?? equal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int CompareTo(ImmutableBitArray a, ImmutableBitArray b)
            => CompareTo(a?._bytes, b?._bytes);

        /// <inheritdoc />
        public int CompareTo(ImmutableBitArray other) => CompareTo(_bytes, other?._bytes);
    }
}
