using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using static Math;

    public partial class ImmutableBitArray
    {
        private static bool Equals(IEnumerable<byte> a, IEnumerable<byte> b)
        {
            if (a == null || b == null)
            {
                return false;
            }

            // ReSharper disable once PossibleMultipleEnumeration
            var minCount = Min(a.Count()
                // ReSharper disable once PossibleMultipleEnumeration
                , b.Count());

            bool NonZero(byte x) => x != 0;

            bool HasMoreNonZero(IEnumerable<byte> x) => x.Skip(minCount).Any(NonZero);

            bool Equals(byte x, byte y) => x == y;

            // ReSharper disable once PossibleMultipleEnumeration
            return a.Take(minCount)
                       // ReSharper disable once PossibleMultipleEnumeration
                       .Zip(b.Take(minCount), Equals).All(z => z)
                   // ReSharper disable once PossibleMultipleEnumeration
                   && !(HasMoreNonZero(a)
                        // ReSharper disable once PossibleMultipleEnumeration
                        || HasMoreNonZero(b));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals(ImmutableBitArray a, ImmutableBitArray b)
            => Equals(a?._bytes, b?._bytes);

        /// <inheritdoc />
        public bool Equals(ImmutableBitArray other)
            => other != null && Equals(_bytes, other._bytes);
    }
}
