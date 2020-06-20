namespace Ellumination.Collections.Keyed.Flags
{
    // ReSharper disable once UnusedTypeParameter
    public abstract partial class Enumeration<T>
    {
        /// <inheritdoc />
        /// <remarks>Comparable and Equality is already implemented at the base class
        /// level. The only thing we have to do here is specialize that for the
        /// <see cref="ImmutableBitArray"/>.</remarks>
        /// <see cref="ImmutableBitArray.CompareTo(ImmutableBitArray,ImmutableBitArray)"/>
        public override int CompareTo(ImmutableBitArray other) => ImmutableBitArray.CompareTo(Key, other);
    }
}
