using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using static BitConverter;

    public partial class ImmutableBitArray
    {
        private static IEnumerable<byte> FromIntValues(IEnumerable<uint> values)
            => values.SelectMany(x => IsLittleEndian ? GetBytes(x) : GetBytes(x).Reverse());

        /// <summary>
        /// Returns a newly created bit array based on the <paramref name="bytes"/>.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="msb"></param>
        /// <returns></returns>
        public static ImmutableBitArray FromBytes(IEnumerable<byte> bytes, bool msb = true)
            => new ImmutableBitArray((msb ? bytes.Reverse() : bytes) ?? GetRange<byte>());

        /// <summary>
        /// Returns a newly created bit array based on the <paramref name="uints"/>.
        /// </summary>
        /// <param name="uints"></param>
        /// <returns></returns>
        public static ImmutableBitArray FromInts(IEnumerable<uint> uints)
            => new ImmutableBitArray(uints ?? GetRange<uint>());
    }
}
