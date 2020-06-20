using System;
using System.Collections.Generic;

namespace Ellumination.Collections
{
    /// <summary>
    /// Represents all the non-type specific interface bit array concerns.
    /// </summary>
    /// <inheritdoc cref="ICollection{T}"/>
    /// <inheritdoc cref="ICloneable"/>
    public interface IImmutableBitArray : ICollection<bool>, ICloneable
    {
        /// <summary>
        /// Gets the value of the bit at a specific position in the <see cref="ImmutableBitArray"/>.
        /// </summary>
        /// <param name="index">The zero-based <paramref name="index"/> of the value to get.</param>
        /// <returns>The value of the bit at position <paramref name="index"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than zero.
        /// There is no upper limit for <paramref name="index"/>, but does not expand the number of
        /// elements within.</exception>
        bool Get(int index);

        /// <summary>
        /// Sets the bit at a specific position in the <see cref="ImmutableBitArray"/> to the specified
        /// <paramref name="value"/>.
        /// </summary>
        /// <param name="index">The zero-based <paramref name="index"/> of the bit to set.</param>
        /// <param name="value">The Boolean <paramref name="value"/> to assign to the bit.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than
        /// zero.</exception>
        void Set(int index, bool value);

        /// <summary>
        /// Sets all bits in the <see cref="ImmutableBitArray"/> to the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The Boolean <paramref name="value"/> to assign to all bits.</param>
        void SetAll(bool value);

        /// <summary>
        /// Gets or sets the value of the bit at a specific position in the
        /// <see cref="ImmutableBitArray"/>. Ensures that the internals can support questions when
        /// <paramref name="index"/> is greater than the current <see cref="Length"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the value to get or set.</param>
        /// <returns>The value of the bit at position index.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than
        /// zero.</exception>
        bool this[int index] { get; set; }

        /// <summary>
        /// Gets or Sets the actual number of Bits contained by the Array.
        /// </summary>
        /// <returns>The actual number of Bits contained by the Array.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The property is set to a value that
        /// is less than zero.</exception>
        /// <seealso cref="Get"/>
        /// <seealso cref="Set"/>
        int Length { get; set; }

        /// <summary>
        /// Returns an <see cref="IEnumerable{Byte}"/> corresponding to <paramref name="msb"/>.
        /// </summary>
        /// <param name="msb"></param>
        /// <returns></returns>
        IEnumerable<byte> ToBytes(bool msb = true);

        /// <summary>
        /// Returns an <see cref="IEnumerable{UInt32}"/> corresponding to <paramref name="msb"/>.
        /// </summary>
        /// <param name="msb"></param>
        /// <returns></returns>
        IEnumerable<uint> ToInts(bool msb = true);
    }

    /// <summary>
    /// Represents <typeparamref name="T"/> specific interface concerns.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <inheritdoc cref="IImmutableBitArray"/>
    /// <inheritdoc cref="IEquatable{T}"/>
    /// <inheritdoc cref="IComparable{T}"/>
    public interface IImmutableBitArray<T> : IImmutableBitArray, IEquatable<T>, IComparable<T>
        where T : class, IImmutableBitArray<T>
    {
        /// <summary>
        /// Performs an immutable bitwise AND operation on the elements in the current
        /// <see cref="ImmutableBitArray"/> against the corresponding elements in the specified
        /// <see cref="ImmutableBitArray"/>.
        /// </summary>
        /// <param name="other">The <see cref="ImmutableBitArray"/> with which to perform the bitwise AND
        /// operation.</param>
        /// <returns> An immutable instance containing the result of the bitwise AND operation on
        /// the elements in the current <see cref="ImmutableBitArray"/> against the corresponding elements
        /// in the specified <see cref="ImmutableBitArray"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
        T And(T other);

        /// <summary>
        /// Performs an immutable bitwise AND operation on the elements in the current
        /// <see cref="ImmutableBitArray"/> against the corresponding elements in the specified
        /// <see cref="ImmutableBitArray"/>.
        /// </summary>
        /// <param name="others">The <see cref="ImmutableBitArray"/> with which to perform the
        /// bitwise AND operation.</param>
        /// <returns> An immutable instance containing the result of the bitwise AND operation on
        /// the elements in the current <see cref="ImmutableBitArray"/> against the corresponding elements
        /// in the specified <see cref="ImmutableBitArray"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="others"/> is null.</exception>
        T And(IEnumerable<T> others);

        /// <summary>
        /// Performs the bitwise OR operation on the elements in the current
        /// <see cref="ImmutableBitArray"/> against the corresponding elements in the specified
        /// <see cref="ImmutableBitArray"/>.
        /// </summary>
        /// <param name="other">The <see cref="ImmutableBitArray"/> with which to perform the bitwise OR
        /// operation.</param>
        /// <returns>The current instance containing the result of the bitwise OR operation on     
        /// the elements in the current <see cref="ImmutableBitArray"/> against the corresponding elements
        /// in the specified <see cref="ImmutableBitArray"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
        T Or(T other);

        /// <summary>
        /// Performs the bitwise OR operation on the elements in the current
        /// <see cref="ImmutableBitArray"/> against the corresponding elements in the specified
        /// <see cref="ImmutableBitArray"/>.
        /// </summary>
        /// <param name="others">The <see cref="ImmutableBitArray"/>(s) with which to perform the
        /// bitwise OR operation.</param>
        /// <returns>The current instance containing the result of the bitwise OR operation on     
        /// the elements in the current <see cref="ImmutableBitArray"/> against the corresponding elements
        /// in the specified <see cref="ImmutableBitArray"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="others"/> is null.</exception>
        T Or(IEnumerable<T> others);

        /// <summary>
        /// Performs the bitwise exclusive OR operation on the elements in the current
        /// <see cref="ImmutableBitArray"/> against the corresponding elements in the specified
        /// <see cref="ImmutableBitArray"/>.
        /// </summary>
        /// <param name="other">The <see cref="ImmutableBitArray"/> with which to perform the bitwise
        /// exclusive OR operation.</param>
        /// <returns>The current instance containing the result of the bitwise exclusive OR
        /// operation on the elements in the current <see cref="ImmutableBitArray"/> against the
        /// corresponding elements in the specified <see cref="ImmutableBitArray"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
        T Xor(T other);

        /// <summary>
        /// Performs the bitwise exclusive OR operation on the elements in the current
        /// <see cref="ImmutableBitArray"/> against the corresponding elements in the specified
        /// <see cref="ImmutableBitArray"/>.
        /// </summary>
        /// <param name="others">The <see cref="ImmutableBitArray"/>(s) with which to perform the
        /// bitwise exclusive OR operation.</param>
        /// <returns>The current instance containing the result of the bitwise exclusive OR
        /// operation on the elements in the current <see cref="ImmutableBitArray"/> against the
        /// corresponding elements in the specified <see cref="ImmutableBitArray"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="others"/> is null.</exception>
        T Xor(IEnumerable<T> others);

        /// <summary>
        /// Inverts all the bit values in the current <see cref="ImmutableBitArray"/>, so that elements
        /// set to true are changed to false, and elements set to false are changed to true.
        /// </summary>
        /// <returns>The current instance with inverted bit values.</returns>
        T Not();
    }
}
