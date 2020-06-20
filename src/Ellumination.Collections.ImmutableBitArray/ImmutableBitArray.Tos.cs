using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    public partial class ImmutableBitArray
    {
        /// <inheritdoc />
        public IEnumerable<byte> ToBytes(bool msb = true)
            => msb ? _bytes.ToArray().Reverse() : _bytes;

        private static IEnumerable<uint> ToInts(IEnumerable<byte> bytes, bool msb = true)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            var values = bytes.ToArray();

            const int size = sizeof(uint);

            // When there are Any we are guaranteed at least One result.
            while (values.Any())
            {
                // This is a byte faster than looping even though we are talking about a handful of bytes.
                if (values.Length < size)
                {
                    values = values.Concat(new byte[size - values.Length]).ToArray();
                }

                // The Array is ordered front to back in LSB, whereas the conversion may want MSB.
                yield return BitConverter.ToUInt32(BitConverter.IsLittleEndian
                    ? values.Take(size).ToArray()
                    : values.Take(size).Reverse().ToArray(), 0);

                // Carry on with the Next iteration if necessary.
                values = values.Skip(size).ToArray();
            }
        }

        // TODO: TBD: need a ctor corresponding to this one, to/from uints ...
        // TODO: TBD: I think the idea of "msb" in this instance was perhaps misinformed, is better to track with the IsLittleEndian ...
        /// <inheritdoc />
        public IEnumerable<uint> ToInts(bool msb = true) => ToInts(_bytes, msb);
    }
}
