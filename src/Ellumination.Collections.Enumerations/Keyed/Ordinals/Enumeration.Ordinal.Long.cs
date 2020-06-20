namespace Ellumination.Collections.Keyed.Ordinals
{
    // ReSharper disable once UnusedTypeParameter
    public abstract partial class LongOrdinalEnumeration<T>
    {
        /// <inheritdoc />
        public override int GetHashCode() => Ordinal.GetHashCode();
    }
}
