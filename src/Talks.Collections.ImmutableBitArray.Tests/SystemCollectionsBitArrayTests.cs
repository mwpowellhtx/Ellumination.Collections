using System;
using System.Collections;

namespace Talks.Collections
{
    using Xunit;
    using Xunit.Abstractions;
    using static BitConverter;

    // ReSharper disable once UnusedMember.Global
    public class SystemCollectionsBitArrayTests : BitArrayComparisonTests<BitArray>
    {
        // ReSharper disable once UnusedMember.Global
        public SystemCollectionsBitArrayTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        protected override bool ShouldOperatorsMutateOperands { get; } = true;

        protected override BitArray GetSubject(int value) => new BitArray(GetBytes(value));

        protected override BinaryOperationCallback<BitArray> AndCallback => (a, b) => a.And(b);

        protected override BinaryOperationCallback<BitArray> OrCallback => (a, b) => a.Or(b);

        protected override BinaryOperationCallback<BitArray> XorCallback => (a, b) => a.Xor(b);

        protected override UnaryOperationCallback<BitArray> NotCallback => a => a.Not();

        protected override void VerifyBits(BitArray subject, int value)
        {
            Assert.NotNull(subject);

            const int bitCount = sizeof(int) * 8;

            Assert.Equal(subject.Length, bitCount);
            Assert.Equal(subject.Count, bitCount);

            // We have no other choice than to compare at a bit level.
            for (var i = 0; i < bitCount; i++)
            {
                var expected = (value & (1 << i)) != 0;
                Assert.Equal(expected, subject.Get(i));
            }
        }
    }
}
