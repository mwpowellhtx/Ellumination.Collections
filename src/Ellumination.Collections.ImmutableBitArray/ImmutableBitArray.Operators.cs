using System.Collections.Generic;

namespace Ellumination.Collections
{
    using static Elasticity;

    public partial class ImmutableBitArray
    {
        /// <summary>
        /// Returns the result of <see cref="And(ImmutableBitArray)"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ImmutableBitArray operator &(ImmutableBitArray a
            , ImmutableBitArray b) => a.And(b);

        /// <summary>
        /// Returns the result of <see cref="Or(ImmutableBitArray)"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ImmutableBitArray operator |(ImmutableBitArray a
            , ImmutableBitArray b) => a.Or(b);

        /// <summary>
        /// Returns the result of <see cref="Xor(ImmutableBitArray)"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ImmutableBitArray operator ^(ImmutableBitArray a
            , ImmutableBitArray b) => a.Xor(b);

        /// <summary>
        /// Returns the result of <see cref="And(IEnumerable{ImmutableBitArray})"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="others"></param>
        /// <returns></returns>
        public static ImmutableBitArray operator &(ImmutableBitArray a
            , IEnumerable<ImmutableBitArray> others) => a.And(others);

        /// <summary>
        /// Returns the result of <see cref="Or(IEnumerable{ImmutableBitArray})"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="others"></param>
        /// <returns></returns>
        public static ImmutableBitArray operator |(ImmutableBitArray a
            , IEnumerable<ImmutableBitArray> others) => a.Or(others);

        /// <summary>
        /// Returns the result of <see cref="Xor(IEnumerable{ImmutableBitArray})"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="others"></param>
        /// <returns></returns>
        public static ImmutableBitArray operator ^(ImmutableBitArray a
            , IEnumerable<ImmutableBitArray> others) => a.Xor(others);

        /// <summary>
        /// Returns the result of <see cref="ShiftLeft(int,Elasticity?)"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static ImmutableBitArray operator <<(ImmutableBitArray a, int count)
            => a.ShiftLeft(count, None);

        /// <summary>
        /// Returns the result of <see cref="ShiftRight(int,Elasticity?)"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static ImmutableBitArray operator >>(ImmutableBitArray a, int count)
            => a.ShiftRight(count, None);
    }
}
