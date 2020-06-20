using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    public partial class ImmutableBitArray
    {
        private void ListAction(Action<List<byte>> action) => action.Invoke(_bytes);

        private TResult ListFunc<TResult>(Func<List<byte>, TResult> func) => func(_bytes);

        private static IEnumerable<bool> GetBooleans(IEnumerable<byte> bytes, int length)
        {
            var shifts = Enumerable.Range(0, BitCount).ToArray();
            // TODO: TBD: may want to Take(Length) of the bits here...
            return bytes.SelectMany(b => shifts.Select(shift => ((1 << shift) & b) != 0)).Take(length);
        }

        private static byte MergeAll(params byte[] values)
            => values.Aggregate(default(byte), (g, y) => (byte) (g | y));

        /// <summary>
        /// Not actually used, yet. We may have usage for this later on, however.
        /// </summary>
        /// <param name="shifts"></param>
        /// <returns></returns>
        /// <remarks>Marking it <see cref="ObsoleteAttribute"/> for now, however, hold onto it
        /// for the time being. It may be useful later on.</remarks>
        [Obsolete]
        private static byte MakeMask(params int[] shifts)
            => shifts.Aggregate(default(byte)
                , (g, shift) => (byte) (g | (byte) (1 << shift))
            );

        /// <summary>
        /// Returns the <see cref="byte"/> following appropriate masking shifting from
        /// <paramref name="loShift"/> through <paramref name="hiShift"/>. Not to be confused
        /// with actual Bit Positions for purposes of indexing and so forth, per se.
        /// </summary>
        /// <param name="loShift"></param>
        /// <param name="hiShift"></param>
        /// <returns></returns>
        internal static byte MakeMask(int loShift, int hiShift)
        {
            var mask = default(byte);

            for (; loShift <= hiShift; loShift++)
            {
                mask |= (byte) (1 << loShift);
            }

            return mask;
        }

        /// <summary>
        /// Returns a Range of <typeparamref name="T"/> across for the <paramref name="count"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="count"></param>
        /// <param name="getDefault"></param>
        /// <returns></returns>
        protected internal static IEnumerable<T> GetRange<T>(int count, Func<T> getDefault = null)
        {
            while (count-- > 0)
            {
                yield return getDefault == null ? default(T) : getDefault.Invoke();
            }
        }

        /// <summary>
        /// Returns a Range of <paramref name="items"/> as a true <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        protected internal static IEnumerable<T> GetRange<T>(params T[] items) => items;
    }
}
