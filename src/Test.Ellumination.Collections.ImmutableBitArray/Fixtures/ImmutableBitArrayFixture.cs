using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using Xunit;

    /// <summary>
    /// Helps by exposing <see cref="ImmutableBitArray._bytes"/> into the unit tests
    /// for verification.
    /// </summary>
    /// <inheritdoc cref="ImmutableBitArray"/>
    public class ImmutableBitArrayFixture : ImmutableBitArray
    {
        internal List<bool> Values => this.ToList();

        internal ImmutableBitArrayFixture(IImmutableBitArray other)
            : base(other.ToBytes(), other.Length)
       {
       }

        internal ImmutableBitArrayFixture(IEnumerable<bool> values)
            : base(values)
        {
        }

        /// <summary>
        /// Construct the fixture with <paramref name="bytes"/> in LSB.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="length"></param>
        /// <inheritdoc />
        internal ImmutableBitArrayFixture(IEnumerable<byte> bytes, int? length = null)
            : base(bytes, length)
        {
        }

        /// <summary>
        /// Construct the fixture with <paramref name="values"/> in LSB.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="length"></param>
        /// <inheritdoc />
        internal ImmutableBitArrayFixture(IEnumerable<uint> values, int? length = null)
            : base(values.ToArray(), length)
        {
        }

        internal ImmutableBitArrayFixture(int length)
            : base(GetRange<byte>())
        {
            Length = length;
        }

        internal ImmutableBitArrayFixture(int length, bool defaultValue)
            : this(length)
        {
            SetAll(defaultValue);
        }

        internal static ImmutableBitArrayFixture FromBytes(params byte[] bytes)
        {
            return new ImmutableBitArrayFixture(bytes);
        }

        internal ImmutableBitArrayFixture InternalAnd(ImmutableBitArrayFixture other)
        {
            var result = And(other);
            Assert.NotNull(result);
            return new ImmutableBitArrayFixture(result);
        }

        internal ImmutableBitArrayFixture InternalOr(ImmutableBitArrayFixture other)
        {
            var result = Or(other);
            Assert.NotNull(result);
            return new ImmutableBitArrayFixture(result);
        }

        internal ImmutableBitArrayFixture InternalXor(ImmutableBitArrayFixture other)
        {
            var result = Xor(other);
            Assert.NotNull(result);
            return new ImmutableBitArrayFixture(result);
        }

        internal ImmutableBitArrayFixture InternalNot()
        {
            var result = Not();
            Assert.NotNull(result);
            return new ImmutableBitArrayFixture(result);
        }

        internal ImmutableBitArrayFixture InternalShiftLeft(int count = 1, Elasticity elasticity = Elasticity.None)
        {
            var result = ShiftLeft(count, elasticity);
            Assert.NotNull(result);
            Assert.NotSame(this, result);
            return new ImmutableBitArrayFixture(result);
        }

        internal ImmutableBitArrayFixture InternalShiftRight(int count = 1, Elasticity elasticity = Elasticity.None)
        {
            var result = ShiftRight(count, elasticity);
            Assert.NotNull(result);
            Assert.NotSame(this, result);
            return new ImmutableBitArrayFixture(result);
        }

        public override object Clone()
        {
            return new ImmutableBitArrayFixture(this);
        }
    }
}
