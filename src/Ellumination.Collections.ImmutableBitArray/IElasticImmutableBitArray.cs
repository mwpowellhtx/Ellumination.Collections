namespace Ellumination.Collections
{
    using static Elasticity;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <inheritdoc />
    public interface IElasticImmutableBitArray<T> : IImmutableBitArray<T>
        where T : class, IElasticImmutableBitArray<T>
    {
        /// <summary>
        /// Returns whether the Bit at <paramref name="index"/> is Set. This operation also
        /// involves <paramref name="elasticity"/>, meaning that <paramref name="index"/>
        /// may exceed the current <see cref="IImmutableBitArray.Length"/> of the Array.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="elasticity"></param>
        /// <returns></returns>
        bool Get(int index, Elasticity elasticity);

        /// <summary>
        /// Sets the Bit at <paramref name="index"/> to the <paramref name="value"/>.
        /// This operation also involves <paramref name="elasticity"/>, meaning that
        /// <paramref name="index"/> may exceed the current
        /// <see cref="IImmutableBitArray.Length"/> of the Array.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="elasticity"></param>
        void Set(int index, bool value, Elasticity elasticity);

        /// <summary>
        /// Shifts the current <see cref="ImmutableBitArray"/> left by the
        /// <paramref name="count"/> number of bits. Optionally expands the
        /// bit array depending on the value of <paramref name="elasticity"/>.
        /// </summary>
        /// <param name="count">The number of bits to shift left.</param>
        /// <param name="elasticity">Optionally provides for <see cref="Expansion"/> or
        /// <see cref="Contraction"/> of the bit array following the Shift operation.</param>
        /// <returns>A new instance with the bits shifted left by the <paramref name="count"/>.</returns>
        T ShiftLeft(int count = 1, Elasticity? elasticity = null);

        /// <summary>
        /// Shifts the current <see cref="ImmutableBitArray"/> right by the
        /// <paramref name="count"/> number of bits. Optionally contracts the
        /// bit array depending on the value of <paramref name="elasticity"/>.
        /// </summary>
        /// <param name="count">The number of bits to shift right.</param>
        /// <param name="elasticity">Optionally provides for <see cref="Expansion"/> or
        /// <see cref="Contraction"/> of the bit array following the shift operation.</param>
        /// <returns>A new instance with the bits shifted right by the <paramref name="count"/>.</returns>
        T ShiftRight(int count = 1, Elasticity? elasticity = null);
    }
}
