using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using Xunit;
    using Xunit.Abstractions;
    using static Elasticity;

    public partial class ImmutableBitArrayShiftTests : SubjectTestFixtureBase<ImmutableBitArray>
    {
        public ImmutableBitArrayShiftTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        private ImmutableBitArray MakeBitArray(IEnumerable<byte> bytes)
        {
            Assert.NotNull(bytes);
            bytes = bytes.ToArray();
            return GetSubject(() => CreateBitArrayWithArray(bytes.ToArray()));
        }

        private ImmutableBitArray MakeBitArray(params uint[] values)
            => GetSubject(() => CreateBitArray(values));

        [Theory, MemberData(nameof(InvalidStartIndexData))]
        public void ShiftLeft_invalid_startIndex_throws(IEnumerable<byte> bytes, int startIndex)
        {
            var a = MakeBitArray(bytes);
            // This one requires a Positive Non-Zero Count for the Exception to occur.
            Assert.Throws<ArgumentOutOfRangeException>(() => a.ShiftLeft(startIndex, 1))
                // ReSharper disable once IdentifierTypo, ImplicitlyCapturedClosure
                .WithExceptionDetail(aoorex =>
                {
                    Assert.Equal(startIndex, aoorex.ActualValue);
                    Assert.Equal(nameof(startIndex), aoorex.ParamName);
                });
        }

        [Theory, MemberData(nameof(InvalidStartIndexData))]
        public void ShiftRight_invalid_startIndex_throws(IEnumerable<byte> bytes, int startIndex)
        {
            var a = MakeBitArray(bytes);
            // This one requires a Positive Non-Zero Count for the Exception to occur.
            Assert.Throws<ArgumentOutOfRangeException>(() => a.ShiftRight(startIndex, 1))
                // ReSharper disable once IdentifierTypo, ImplicitlyCapturedClosure
                .WithExceptionDetail(aoorex =>
                {
                    Assert.Equal(nameof(startIndex), aoorex.ParamName);
                    Assert.Equal(startIndex, aoorex.ActualValue);
                });
        }

        [Theory, MemberData(nameof(InvalidCountData))]
        public void ShiftLeft_invalid_count_throws(IEnumerable<byte> bytes, int startIndex, int count)
        {
            var a = MakeBitArray(bytes);
            Assert.Throws<ArgumentOutOfRangeException>(() => a.ShiftLeft(startIndex, count))
                // ReSharper disable once IdentifierTypo, ImplicitlyCapturedClosure
                .WithExceptionDetail(aoorex =>
                {
                    Assert.Equal(count, aoorex.ActualValue);
                    Assert.Equal(nameof(count), aoorex.ParamName);
                });
        }

        [Theory, MemberData(nameof(InvalidCountData))]
        public void ShiftRight_invalid_count_throws(IEnumerable<byte> bytes, int startIndex, int count)
        {
            var a = MakeBitArray(bytes);
            Assert.Throws<ArgumentOutOfRangeException>(() => a.ShiftRight(startIndex, count))
                // ReSharper disable once IdentifierTypo, ImplicitlyCapturedClosure
                .WithExceptionDetail(aoorex =>
                {
                    Assert.Equal(count, aoorex.ActualValue);
                    Assert.Equal(nameof(count), aoorex.ParamName);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="expectedBytes"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="elasticity"></param>
        /// <param name="expectedLength"></param>
        [Theory, MemberData(nameof(ShiftLeftCorrectData))]
        public void ShiftLeft_is_correct(IEnumerable<byte> bytes, IEnumerable<byte> expectedBytes
            , int startIndex, int count, Elasticity? elasticity, int expectedLength)
        {
            Assert.NotNull(bytes);
            Assert.NotNull(expectedBytes);

            bytes = bytes.ToArray();

            var a = MakeBitArray(bytes);
            var b = a.ShiftLeft(startIndex, count, elasticity);

            Assert.NotSame(a, b);
            Assert.Equal(expectedLength, b.Length);

            // ReSharper disable once ImplicitlyCapturedClosure
            When(() => elasticity.Contains(Expansion), () =>
            {
                // We should be able to glean this.
                When(() => startIndex > 0, () => Assert.Equal(a.Take(startIndex), b.Take(startIndex)));

                // All of the Bits inserted to achieve the Shift should be False.
                Assert.All(b.Skip(startIndex).Take(count), Assert.False);

                // Last but not least we should be able to Compare the Shifted Bits.
                Assert.Equal(a.Skip(startIndex), b.Skip(startIndex + count));
            });

            // ReSharper disable once ImplicitlyCapturedClosure
            When(() => !elasticity.Contains(Expansion), () =>
            {
                // When there is no Expansion, then we can reliably look further this direction.
                var expected = CreateBitArrayWithArray(expectedBytes.ToArray());
                Assert.Equal(expected.ToBytes(false), b.ToBytes(false));
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="expectedBytes"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="elasticity"></param>
        /// <param name="expectedLength"></param>
        [Theory, MemberData(nameof(ShiftRightCorrectData))]
        public void ShiftRight_is_correct(IEnumerable<byte> bytes, IEnumerable<byte> expectedBytes
            , int startIndex, int count, Elasticity? elasticity, int expectedLength)
        {
            Assert.NotNull(bytes);
            Assert.NotNull(expectedBytes);

            bytes = bytes.ToArray();
            expectedBytes = expectedBytes.ToArray();

            var a = MakeBitArray(bytes);
            var b = a.ShiftRight(startIndex, count, elasticity);

            Assert.NotSame(a, b);
            Assert.Equal(expectedLength, b.Length);

            /* The Length is the critical part.
             * After that, any Bits or Bytes that received Contraction
             * may be compared in terms of Null/Zero conditions. */
            Assert.Equal(CreateBitArrayWithArray(expectedBytes.ToArray()), b);
        }

        [Theory, MemberData(nameof(RemoveCorrectData))]
        public void Remove_works_correctly(uint value, bool item, bool expectedRemoved
            , VerifyBooleanArrayCallback verify)
        {
            OutputHelper.WriteLine($"'{nameof(value)}' is '{value:X08}'.");
            // TODO: TBD: we may further complicate the unit test and verify the Bytes themselves, but for now let's just focus on the operation itself
            var s = MakeBitArray(value);
            Assert.Equal(expectedRemoved, s.Remove(item));
            verify?.Invoke(s.ToArray());
        }
    }

    public delegate void VerifyBooleanArrayCallback(IEnumerable<bool> bits);
}
