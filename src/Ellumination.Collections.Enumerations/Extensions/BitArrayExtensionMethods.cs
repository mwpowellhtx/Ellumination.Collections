using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using static Convert;

    /// <summary>
    /// Bit array extension methods provided for purposes of supporting Enumeration behavior.
    /// </summary>
    public static class BitArrayExtensionMethods
    {
        //TODO: TBD: really should have some simple unit tests around these...
        /// <summary>
        /// Returns an <see cref="IEnumerable{Byte}"/> from the <paramref name="text"/> in
        /// <paramref name="msb"/> order. <paramref name="msb"/> indicates the byte order of the
        /// text, not the resulting enumerated bytes, which are always in least significant order,
        /// consistent with their enumerated positions.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="msb">Default is true by default consistent with
        /// <see cref="ImmutableBitArray.ToBytes"/>.</param>
        /// <returns></returns>
        public static IEnumerable<byte> ToBytes(this string text, bool msb = true)
        {
            // Byte width of two characters, assuming we are talking about a hex string here.
            const int bw = 2;

            // Assumes hexadecimal encoding.
            const int fromBase = 16;

            // Prepending is appropriate for MSB ordered text.
            text = text.Length % bw == 1 ? "0" + text : text;

            /* For the same reason as converting to a string, we cannot reverse a string of
             * hex nibbles and expect the bits themselves to yield the correct byte values. */

            if (msb)
            {
                for (var i = text.Length - bw; i >= 0; i -= bw)
                {
                    // Assumes that there is valid hex encoding there.
                    var part = text.Substring(i, bw);
                    yield return ToByte(part, fromBase);
                }
            }
            else
            {
                // The direction is correct but watch out for edge (n-1) use cases.
                for (var i = 0; i <= text.Length - bw; i += bw)
                {
                    // Assumes that there is valid hex encoding there.
                    var part = text.Substring(i, bw);
                    yield return ToByte(part, fromBase);
                }
            }
        }

        /// <summary>
        /// Returns a hexadecimal representation of the <paramref name="arr"/> in the usual MSB
        /// order.
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="msb">Returns the string in <paramref name="msb"/> order. Default is true
        /// consistent with <see cref="ImmutableBitArray.ToBytes"/>.</param>
        /// <returns></returns>
        public static string ToByteString(this ImmutableBitArray arr, bool msb = true)
        {
            // Return in the appropriate order.
            return (arr == null ? new byte[0] : arr.ToBytes(msb)).ToByteString();
        }

        /// <summary>
        /// Returns a hexadecimal representation of the <paramref name="bytes"/> in the order in
        /// which they were provided.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToByteString(this IEnumerable<byte> bytes)
        {
            bytes = bytes ?? new byte[0];

            /* Do it this way because reversing a string of nibbles is not the same thing
             * as reversing the actual bytes, especially for oddly positioned nibbles. */

            return bytes.Aggregate(string.Empty, (s, b) => s + $"{b:X2}");
        }
    }
}
