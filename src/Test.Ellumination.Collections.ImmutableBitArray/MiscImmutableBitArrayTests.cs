using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using Xunit;
    using Xunit.Abstractions;
    using static BitConverter;
    using static Math;
    using static ImmutableBitArray;
    using static String;

    public partial class MiscImmutableBitArrayTests : SubjectTestFixtureBase<ImmutableBitArray>
    {
        public MiscImmutableBitArrayTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        public static IEnumerable<object[]> OneRandomIntData
        {
            get
            {
                var values = new uint[] { 0, 1, 2, 3 };
                foreach (var x in values)
                {
                    yield return new object[] { x };
                }
            }
        }

        [Theory
            , MemberData(nameof(OneRandomIntData), DisableDiscoveryEnumeration = true)
            ]
        public void Verify_that_Clear_works_correctly(uint value)
        {
            GetSubject(() => CreateBitArray(value));
            Assert.Equal(sizeof(uint) * 8, Subject.Length);
            Subject.Clear();
            Assert.Empty(Subject.ToBytes());
            Assert.Equal(0, Subject.Length);
#pragma warning disable xUnit2013 // Do not use equality check to check for collection size.
            /* This is a legit Assertion to perform considering the Subject.
             * Therefore, override whatever Xunit and/or R# may be complaining about. */
            Assert.Equal(0, Subject.Count);
#pragma warning restore xUnit2013 // Do not use equality check to check for collection size.
        }

        [Theory
            , MemberData(nameof(OneRandomIntData), DisableDiscoveryEnumeration = true)
            ]
        public void Verify_that_Clone_works_correctly(uint value)
        {
            GetSubject(() => CreateBitArray(value));
            ImmutableBitArray clone;
            Assert.NotNull(clone = Subject.Clone() as ImmutableBitArray);
            Assert.NotSame(Subject, clone);
            Assert.Equal(Subject.ToBytes(), clone.ToBytes());
        }

        [Theory
            , MemberData(nameof(OneRandomIntData), DisableDiscoveryEnumeration = true)
            ]
        public void Verify_that_Equal_arrays_work_correctly(uint value)
        {
            var a = CreateBitArray(value);
            var b = CreateBitArray(value);
            Assert.NotSame(a, b);
            var expected = GetEndianAwareBytes(value);
            var isBigEndian = !IsLittleEndian;
            Assert.Equal(expected, a.ToBytes(isBigEndian));
            Assert.Equal(a.ToBytes(), b.ToBytes());
            Assert.True(a.Equals(b));
        }

        public static IEnumerable<object[]> TwoRandomIntData
        {
            get
            {
                var values = new uint[] { 0, 1, 2, 3 };
                foreach (var x in values)
                {
                    foreach (var y in values)
                    {
                        yield return new object[] { x, y };
                    }
                }
            }
        }

        [Theory
            , MemberData(nameof(TwoRandomIntData), DisableDiscoveryEnumeration = true)
            ]
        public void Verify_that_Not_Equal_arrays_work_correctly(uint x, uint y)
        {
            // TODO: TBD: run this through actual MemberData and ensure that X and Y are both different.
            // TODO: TBD: for now at this late hour this is the poor man's...
            if (x == y)
            {
                return;
            }

            var a = CreateBitArray(x);
            var b = CreateBitArray(y);
            Assert.NotSame(a, b);
            var expectedX = GetEndianAwareBytes(x);
            var expectedY = GetEndianAwareBytes(y);
            var isBigEndian = !IsLittleEndian;
            Assert.Equal(expectedX, a.ToBytes(isBigEndian));
            Assert.Equal(expectedY, b.ToBytes(isBigEndian));
            Assert.NotEqual(a.ToBytes(), b.ToBytes());
            Assert.False(a.Equals(b));
        }

        /// <summary>
        /// Verifies that the Length and Count respond properly to <paramref name="bytes"/>
        /// Initialization as well as to changes in
        /// <see cref="ImmutableBitArray.Length"/> via <paramref name="lengthDelta"/>.
        /// We are guaranteed here by virtual of unit test and test case data design that
        /// <paramref name="lengthDelta"/> should be non-zero and positive. For sake of
        /// argument, we will also expect the numbers to be Prime Numbers in nature,
        /// although we will leave that to the test case generation to determine.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="lengthDelta">Permits changes to
        /// <see cref="ImmutableBitArray.Length"/> allowing it to either expand or
        /// contract accordingly, which may also yield <see cref="ArgumentOutOfRangeException"/>
        /// when excessive negative delta values are provided.</param>
        /// <remarks>We could ostensibly separate this into several more focused individual unit
        /// tests, but we can just as easily leverage the differences in furnished
        /// <see cref="MemberDataAttribute"/>.</remarks>
        [Theory
            , MemberData(nameof(LengthCountData))
            ]
        public void Length_and_Count_work_correctly(IEnumerable<byte> bytes, int lengthDelta)
        {
            Assert.NotNull(bytes);
            bytes = bytes.ToArray();
            GetSubject(() => CreateBitArrayWithArray(bytes.ToArray()));

            // Let's make some helpful reports during the unit test for post-mortem analysis.
            OutputHelper.WriteLine(
                "Original bytes were: "
                + $"{Join(", ", Subject.InternalBytes().Select(b => $"'{b:X2}'"))}"
            );

            var initialExpectedLengthAndCount = bytes.Count() * BitCount;
            Assert.Equal(initialExpectedLengthAndCount, Subject.Length);
            Assert.Equal(initialExpectedLengthAndCount, Subject.Count);

            // With recent changes, Count now simply reports Length.
            var expectedLength = Subject.Length + lengthDelta;

            // This is the money shot right here.
            Subject.Length += lengthDelta;
            Assert.Equal(expectedLength, Subject.Length);
            Assert.Equal(expectedLength, Subject.Count);

            // ReSharper disable once InvertIf do not invert this since we use bitIndex subsequently.
            // Last but not least verify that any Bits on the Boundary Byte exceeding Length are reset.
            if (Subject.Length % BitCount is int bitPosition && bitPosition != 0)
            {
                var boundaryIndex = Subject.Length / BitCount;
                var actualBoundaryByte = Subject.InternalBytes().ElementAt(boundaryIndex);

                OutputHelper.WriteLine(
                    $"New Length '{Subject.Length}' occurs within Byte Boundary '{boundaryIndex}'"
                    + $" at bit index '{bitPosition - 1}' with unmasked byte '{actualBoundaryByte:X2}'."
                );

                /* So this may not be so Kosher, but there are only so many ways
                 * to Makean appropriate Mask in the problem domain. */

                // BitPosition, not to be confused with the actual BitIndex.
                var boundaryByteMask = MakeMask(0, bitPosition - 1);
                const byte expectedMaskedByte = default(byte);
                Assert.Equal(expectedMaskedByte, actualBoundaryByte & (byte) ~boundaryByteMask);
            }
        }

        /// <summary>
        /// Contain the use case where no <see cref="ImmutableBitArray.Length"/> change
        /// occurs. This is a fairly concise, standalone <see cref="FactAttribute"/>.
        /// </summary>
        [Fact]
        public void No_Length_Delta_does_nothing()
        {
            // Does not really matter what we initialize it to for sake of unit test argument.
            GetSubject(() => CreateBitArrayWithArray(0, 1, 2));
            var originalLength = Subject.Length;
            Assert.True(originalLength > 0);
            var originalBytesCount = Subject.InternalBytes().Count();
            Subject.Length = originalLength;
            Assert.Equal(originalLength, Subject.Length);
            Assert.Equal(originalBytesCount, Subject.InternalBytes().Count());
        }

        /// <summary>
        /// Setting <see cref="ImmutableBitArray.Length"/> to a negative value is
        /// strictly prohibited and we expect <see cref="ArgumentOutOfRangeException"/> would be
        /// thrown.
        /// </summary>
        /// <param name="newLength"></param>
        [Theory, MemberData(nameof(InvalidLengthData))]
        public void Invalid_Length_throws_exception(int newLength)
        {
            GetSubject(() => CreateBitArrayWithArray());
            Assert.Throws<ArgumentOutOfRangeException>(() => Subject.Length = newLength)
                // ReSharper disable once ImplicitlyCapturedClosure
                .WithExceptionDetail(aoorex =>
                {
                    var value = newLength;
                    Assert.Equal(value, aoorex.ActualValue);
                    Assert.Equal(nameof(value), aoorex.ParamName);
                });
        }

        /// <summary>
        /// The unit test verification is not especially extensive, however, the furnished
        /// <see cref="ContainsData"/> is fairly exhaustive of the possible use case variants.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="expectedItem"></param>
        /// <param name="expectedContainsResult"></param>
        [Theory, MemberData(nameof(ContainsData))]
        public void Contains_works_correctly(IEnumerable<byte> bytes, bool expectedItem, bool expectedContainsResult)
        {
            Assert.NotNull(bytes);
            bytes = bytes.ToArray();
            GetSubject(() => CreateBitArrayWithArray(bytes.ToArray()));
            // Double check that the Bytes Actually Are the Bytes.
            Assert.Equal(bytes, Subject.InternalBytes());
            Assert.Equal(expectedContainsResult, Subject.Contains(expectedItem));
        }

        // TODO: TBD: then the challenge is to engineer a decent range of test cases exercising either dimension, both, or neither...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="desiredArrayLength"></param>
        /// <param name="bitArrayIndex"></param>
        [Theory, MemberData(nameof(CopyToData))]
        public void CopyTo_works_correctly(IEnumerable<byte> bytes, int desiredArrayLength, int bitArrayIndex)
        {
            Assert.NotNull(bytes);
            bytes = bytes.ToArray();
            GetSubject(() => CreateBitArrayWithArray(bytes.ToArray()));
            var actualArray = new bool[desiredArrayLength];
            Subject.CopyTo(actualArray, bitArrayIndex);
            var availableLength = Subject.Length - (bitArrayIndex + 1);
            /* Overlapping is really the only two choices we want from Both Arrays. Either:
             * 1. We have more than enough Bit Array to accommodate the bool[], or:
             * 2. We have insufficient Bit Array to furnish some or all of the bool[].
             * 3. Which, in either case, we want to ignore the remaining values in one array
             * or the other. */
            var overlappingLength = Min(desiredArrayLength, availableLength);
            var actualArraySnapshot = actualArray.Take(overlappingLength);
            var expectedBitArraySnapshot = Subject.ToArray().Skip(bitArrayIndex).Take(overlappingLength);
            Assert.Equal(expectedBitArraySnapshot, actualArraySnapshot);
        }

        /// <summary>
        /// Verifies that <see cref="ImmutableBitArray.CopyTo"/> throws
        /// <see cref="ArgumentOutOfRangeException"/> when an invalid
        /// <paramref name="arrayIndex"/> is discovered. Unit tests assume a Zero
        /// <see cref="ImmutableBitArray.Length"/> Bit Array.
        /// </summary>
        /// <param name="arrayIndex"></param>
        [Theory, MemberData(nameof(InvalidCopyToArrayIndexData))]
        public void Invalid_CopyTo_arrayIndex_throws_exception(int arrayIndex)
        {
            GetSubject(() => CreateBitArrayWithArray());
            // Does not matter what we provide for an Array in this instance.
            var array = GetRange<bool>().ToArray();
            Assert.Throws<ArgumentOutOfRangeException>(
                    () => Subject.CopyTo(array, arrayIndex))
                // ReSharper disable once ImplicitlyCapturedClosure
                .WithExceptionDetail(aoorex =>
                {
                    Assert.Equal(arrayIndex, aoorex.ActualValue);
                    // Argument is named specifically for this reason.
                    Assert.Equal(nameof(arrayIndex), aoorex.ParamName);
                });
        }
    }
}
