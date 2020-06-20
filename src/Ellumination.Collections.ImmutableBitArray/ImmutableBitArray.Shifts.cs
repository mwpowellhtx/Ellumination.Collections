using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using static Math;
    using static Elasticity;

    public partial class ImmutableBitArray
    {
        private static void VerifyShiftArguments(int startIndex, int count, int length, Elasticity? elasticity)
        {
            bool TryArgumentOutOfRange<T>(string argName, T argValue, Func<T, bool> outOfRange)
            {
                if (!outOfRange(argValue))
                {
                    return false;
                }

                if (elasticity.Contains(Silent))
                {
                    return true;
                }

                throw new ArgumentOutOfRangeException(argName, argValue, $"'{argName}' ('{argValue}') invalid.");
            }

            if (TryArgumentOutOfRange(nameof(startIndex), startIndex, x => x < 0 || x >= length)
                || TryArgumentOutOfRange(nameof(count), count, x => x < 0))
            {
                // Zero Count also returns early. There is nothing to do in that case.
            }
        }

        /// <summary>
        /// User provided <see cref="Shift"/> Callback.
        /// </summary>
        /// <param name="preserveBits">The Bits to Preserve on the Right of the Array Index.</param>
        /// <param name="insertBits">The Bits being Inserted in order to achieve the appropriate
        /// Shift Left or Right.</param>
        /// <param name="shiftBits">The Bits being Shifted Left or Right.</param>
        /// <returns></returns>
        private delegate IEnumerable<bool> ShiftStrategyCallback(IEnumerable<bool> preserveBits
            , IEnumerable<bool> insertBits, IEnumerable<bool> shiftBits);

        /// <summary>
        /// To Shift or Not to Shift: THAT is the question. The decision to do things this way
        /// jumping into the Boolean Collection realm in order to conduct the Shift operation
        /// was informed by an analysis of the Big O performance. If we were always given 3-4
        /// Bits either to Shift or Not to Shift at the Byte/Bit level, that might make it
        /// worthwhile to consider some sort of Byte-wise strategy. However, when we are given
        /// 0, 1, 2, 3 Bits in a Byte, along these lines, to Shift or Not to Shift, then it no
        /// longer makes sense to traverse Bytes for Shift purposes any longer and we may as
        /// well just insert whole collections of buffer Bits (Booleans) and do the calculation
        /// at the Boolean Collection level.
        /// </summary>
        /// <param name="array">The Bit Array which to Shift. Ostensibly the current instance.</param>
        /// <param name="startIndex">Where to begin Shifting.</param>
        /// <param name="count">The number of Bits in which to Shift.</param>
        /// <param name="strategy">The user furnished Shift Strategy.</param>
        /// <returns></returns>
        /// <see cref="ShiftStrategyCallback"/>
        private static ImmutableBitArray Shift(IEnumerable<bool> array, int startIndex, int count
            , ShiftStrategyCallback strategy)
        {
            array = array.ToArray();


            return new ImmutableBitArray(strategy(array.Take(startIndex)
                , GetRange(count, () => false), array.Skip(startIndex)));
        }

        /// <summary>
        /// Shifts the current Bit Array <paramref name="count"/> Bits to the Left. Leaves the
        /// Bits right of the <paramref name="startIndex"/> alone, while Shifting the left of the
        /// same to the Left. Applies the <paramref name="elasticity"/> specification allowing for
        /// <see cref="Elasticity.Expansion"/>. When unspecified constrains the Shifted bits to the current
        /// Bit Array <see cref="Length"/>.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="elasticity"></param>
        /// <returns></returns>
        public ImmutableBitArray ShiftLeft(int startIndex, int count = 1, Elasticity? elasticity = null)
        {
            if (count == 0)
            {
                return new ImmutableBitArray(this);
            }

            // ReSharper disable once InconsistentNaming
            var this_Length = Length;

            VerifyShiftArguments(startIndex, count, this_Length, elasticity);

            return Shift(this, startIndex, count
                , (pbits, ibits, sbits) => elasticity.Contains(Expansion)
                    ? pbits.Concat(ibits).Concat(sbits)
                    : pbits.Concat(ibits.Concat(sbits).Take(this_Length - startIndex))
            );
        }

        /// <summary>
        /// Shifts the current Bit Array <paramref name="count"/> Bits to the Right. Leaves the
        /// Bits right of the <paramref name="startIndex"/> alone, while Shifting the left of the
        /// same to the Right. Applies the <paramref name="elasticity"/> specification allowing for
        /// <see cref="Contraction"/>. When unspecified constrains the Shifted bits to the current
        /// Bit Array <see cref="Length"/>.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="elasticity"></param>
        /// <returns></returns>
        public ImmutableBitArray ShiftRight(int startIndex, int count = 1, Elasticity? elasticity = null)
        {
            // Return very early when there is nothing else to do.
            if (count == 0)
            {
                // Actually this is more precise, including the Length.
                return new ImmutableBitArray(this);
            }

            // ReSharper disable once InconsistentNaming
            var this_Length = Length;

            VerifyShiftArguments(startIndex, count, this_Length, elasticity);

            return Shift(this, startIndex, count
                , (pbits, ibits, sbits) =>
                {
                    var xbits = sbits.Concat(ibits);
                    // ReSharper disable once InconsistentNaming, PossibleMultipleEnumeration
                    var xbits_Count = xbits.Count();
                    return elasticity.Contains(Contraction)
                        // ReSharper disable once PossibleMultipleEnumeration
                        ? pbits.Concat(xbits.Skip(count).Take(Max(0, xbits_Count - count - count)))
                        // ReSharper disable once PossibleMultipleEnumeration
                        : pbits.Concat(xbits.Skip(count));
                }
            );
        }

        /// <summary>
        /// 0
        /// </summary>
        private const int DefaultStartIndex = 0;

        // TODO: TBD: want to separate these a little bit along the lines of Elasticity...
        /// <inheritdoc />
        /// <see cref="DefaultStartIndex"/>
        /// <see cref="ShiftLeft(int,int,Elasticity?)"/>
        public ImmutableBitArray ShiftLeft(int count = 1, Elasticity? elasticity = null)
            => ShiftLeft(DefaultStartIndex, count, elasticity);

        // TODO: TBD: when shift left/right are both done, they should also update the Length, especially when Elasticity comes into play
        /// <inheritdoc />
        /// <see cref="DefaultStartIndex"/>
        /// <see cref="ShiftRight(int,int,Elasticity?)"/>
        public ImmutableBitArray ShiftRight(int count = 1, Elasticity? elasticity = null)
            => ShiftRight(DefaultStartIndex, count, elasticity);

        /// <inheritdoc />
        /// <see cref="Length"/>
        public bool Remove(bool item)
        {
            /* We take the circuitous route here because we will need to work with the Index
             * anyway in order to do the appropriate Shift operation should a matching Item
             * be found. */
            for (var i = 0; i < Length; i++)
            {
                if (Get(i) != item)
                {
                    continue;
                }

                // This is a key use case for ShiftRight startIndex overload.
                var shifted = ShiftRight(i, 1, Contraction);

                // Then translate the Shifted Range into this Collection.
                ListAction(b =>
                {
                    b.Clear();
                    b.AddRange(shifted._bytes);
                });

                // And adjust the Length.
                Length = shifted.Length;

                return true;
            }

            return false;
        }
    }
}
