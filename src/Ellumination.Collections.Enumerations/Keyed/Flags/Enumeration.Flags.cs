using System.Linq;

namespace Ellumination.Collections.Keyed.Flags
{
    // ReSharper disable once UnusedTypeParameter
    public abstract partial class Enumeration<T>
    {
        /// <summary>
        /// 8
        /// </summary>
        internal const int BitsPerByte = 8;

        /// <summary>
        /// In order for things to work out well, especially with serialization, to from
        /// data store, through object relational mapping, and things of this nature, the
        /// proposed length should be given in multiples of bits per byte (or 8).
        /// </summary>
        /// <param name="proposed"></param>
        /// <returns></returns>
        /// <see cref="BitsPerByte"/>
        public static int NormalizeLength(int proposed)
            => BitsPerByte
               * (proposed % BitsPerByte == 0
                   ? proposed
                   : proposed / BitsPerByte + 1);

        /// <summary>
        /// Gets the KeyByteString associated with the long byte array <see cref="Key"/>
        /// representation.
        /// </summary>
        protected string KeyByteString { get; private set; }

        /// <summary>
        /// Gets the SetKeyBitCount, the number of <see cref="Enumeration{TKey}.Key"/> Bits that are actually set.
        /// </summary>
        protected long SetKeyBitCount { get; private set; }

        /// <inheritdoc />
        public override ImmutableBitArray Key
        {
            get => base.Key;
            protected set
            {
                void EvaluateKey(out ImmutableBitArray key)
                    => key = value == null
                        ? ImmutableBitArray.FromBytes(new[] {default(byte)})
                        : (ImmutableBitArray) value.Clone();

                EvaluateKey(out var candidateKey);
                // TODO: TBD: set the candidateKey length here?
                // Make sure that we treat the bytes as LSB throughout.
                var bytes = candidateKey.ToBytes(false).ToArray();
                KeyByteString = bytes.ToByteString();
                // TODO: TBD: what are we actually counting by this? the number of bits that are actually masked?
                // TODO: TBD: further, is it something that we should be setting once in response to other properties?
                // TODO: TBD: or is it something that we could potentially just inquire about on the fly?
                SetKeyBitCount = Enumerable.Range(0, candidateKey.Length).Count(x => candidateKey[x]);
                base.Key = candidateKey;
            }
        }

        // TODO: TBD: we may want an IsZero for Ordinal as well, in fact, abstract at a base class level...
        /// <summary>
        /// Returns whether IsZero.
        /// </summary>
        public virtual bool IsZero => Key.All(x => !x);

        // ReSharper disable once NonReadonlyMemberInGetHashCode
        /// <inheritdoc />
        public override int GetHashCode() => KeyByteString?.GetHashCode() ?? default(int);
    }
}
