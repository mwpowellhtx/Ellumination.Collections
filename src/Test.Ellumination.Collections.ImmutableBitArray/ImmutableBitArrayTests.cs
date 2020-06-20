using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using Xunit;
    using Xunit.Abstractions;
    using static BitConverter;
    using static ImmutableBitArray;

    public partial class ImmutableBitArrayTests : SubjectTestFixtureBase<ImmutableBitArray>
    {
        public ImmutableBitArrayTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        // TODO: TBD: test the other permutations of Binary Bitwise operators involving 2+ arrays...
        // TODO: TBD: test elastic get/set
        // TODO: TBD: test the shift left/right issues...
        // TODO: TBD: test Add/Remove ... also an extension of the shift left/right issue

        // TODO: TBD: there are two approaches I want to take addressing the shift/elasticity issue...
        // TODO: TBD: the first is a sort of brute force get/set approach, getting from one source at the source position, setting the destination at the destination position. / that will be a slower performer I think...
        // TODO: TBD: the second, and better performer, I think, would be to determine the source/destination byte(s), fields, etc, and to shift/merge the bytes from source to destination. this will be by far a better performer, but will also be more complex managing the positions, fields, shifts, etc. probably not as hard as I think it is on the front side of the effort, but it is not something I want to dive into all that casually.

        /* TODO: TBD: Length/Count should be more intimately connected with an Elasticity behavior,
         * inelastic behavior should preclude changes; whereas permitting Contraction/Expansion
         * should respond accordingly, which all told is not really a shift issue, but does comport
         * with kosher Elasticity behavior... this is coming in a future version, which will also
         * involve a string separation of Elastic and Inelastic behavior, but not right now. */

        private static void VerifyBinaryCalculation(uint x, uint y
            , CalculateBinaryOperationCallback<uint> expectedOp
            , CalculateBinaryOperationCallback<ImmutableBitArray> actualOp)
        {
            var expected = expectedOp(x, y);
            var a = CreateBitArray(x);
            var b = CreateBitArray(y);
            Assert.NotSame(a, b);
            var actual = actualOp(a, b);
            Assert.NotNull(actual);
            // Demarcation from System.Collections.BitArray.
            Assert.NotSame(a, actual);
            Assert.NotSame(b, actual);
            Assert.Equal(GetEndianAwareBytes(expected), actual.ToBytes(!IsLittleEndian));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="expectedOp"></param>
        /// <param name="actualOp"></param>
        [Theory, MemberData(nameof(BinaryOrData))]
        public void Bitwise_Or_Binary_calculation_is_correct(uint x, uint y
            , CalculateBinaryOperationCallback<uint> expectedOp
            , CalculateBinaryOperationCallback<ImmutableBitArray> actualOp)
            => VerifyBinaryCalculation(x, y, expectedOp, actualOp);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="expectedOp"></param>
        /// <param name="actualOp"></param>
        [Theory, MemberData(nameof(BinaryAndData))]
        public void Bitwise_And_Binary_calculation_is_correct(uint x, uint y
            , CalculateBinaryOperationCallback<uint> expectedOp
            , CalculateBinaryOperationCallback<ImmutableBitArray> actualOp)
            => VerifyBinaryCalculation(x, y, expectedOp, actualOp);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="expectedOp"></param>
        /// <param name="actualOp"></param>
        [Theory, MemberData(nameof(BinaryXorData))]
        public void Bitwise_Xor_Binary_calculation_is_correct(uint x, uint y
            , CalculateBinaryOperationCallback<uint> expectedOp
            , CalculateBinaryOperationCallback<ImmutableBitArray> actualOp)
            => VerifyBinaryCalculation(x, y, expectedOp, actualOp);

        private static void VerifyUnaryCalculation(uint x
            , CalculateUnaryOperationCallback<uint> expectedOp
            , CalculateUnaryOperationCallback<ImmutableBitArray> actualOp)
        {
            var expected = expectedOp(x);
            var a = CreateBitArray(x);
            var actual = actualOp(a);
            Assert.NotNull(actual);
            // Demarcation from System.Collections.BitArray.
            Assert.NotSame(a, actual);
            Assert.Equal(GetEndianAwareBytes(expected), actual.ToBytes(!IsLittleEndian));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="expectedOp"></param>
        /// <param name="actualOp"></param>
        [Theory, MemberData(nameof(UnaryNotData))]
        public void Bitwise_Not_Unary_calculation_is_correct(uint x
            , CalculateUnaryOperationCallback<uint> expectedOp
            , CalculateUnaryOperationCallback<ImmutableBitArray> actualOp)
            => VerifyUnaryCalculation(x, expectedOp, actualOp);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="expected"></param>
        /// <see cref="ImmutableBitArray.Equals(ImmutableBitArray)"/>
        /// <see cref="Equals(ImmutableBitArray, ImmutableBitArray)"/>
        [Theory, MemberData(nameof(EqualsData))]
        public void Equals_calculation_is_correct(IEnumerable<uint> x, IEnumerable<uint> y, bool expected)
        {
            Assert.NotNull(x);
            Assert.NotNull(y);
            var a = CreateBitArray(x.ToArray());
            var b = CreateBitArray(y.ToArray());
            Assert.False(a.Equals(null));
            Assert.False(b.Equals(null));
            Assert.False(ImmutableBitArray.Equals(a, null));
            Assert.False(ImmutableBitArray.Equals(b, null));
            Assert.False(ImmutableBitArray.Equals(null, a));
            Assert.False(ImmutableBitArray.Equals(null, b));
            Assert.Equal(expected, a.Equals(b));
            Assert.Equal(expected, b.Equals(a));
            Assert.Equal(expected, ImmutableBitArray.Equals(a, b));
            Assert.Equal(expected, ImmutableBitArray.Equals(b, a));
        }

        /// <summary>
        /// The breadth of <see cref="CompareToData"/> provided for this test case is fairly wide.
        /// Do not be fooled by the terseness of the assertions themselves. There are actually
        /// several use cases which need to be exercised in the
        /// <see cref="ImmutableBitArray.CompareTo(ImmutableBitArray)"/> code by
        /// those test cases.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="expected"></param>
        /// <see cref="ImmutableBitArray.CompareTo(ImmutableBitArray)"/>
        /// <see cref="CompareTo(ImmutableBitArray, ImmutableBitArray)"/>
        [Theory, MemberData(nameof(CompareToData))]
        public void CompareTo_calculation_is_correct(IEnumerable<uint> x, IEnumerable<uint> y, int expected)
        {
            var a = CreateBitArray(x.ToArray());
            var b = CreateBitArray(y.ToArray());
            const int greaterThan = 1;
            const int lessThan = -1;
            Assert.Equal(greaterThan, a.CompareTo(null));
            Assert.Equal(greaterThan, b.CompareTo(null));
            Assert.Equal(greaterThan, CompareTo(a, null));
            Assert.Equal(greaterThan, CompareTo(b, null));
            Assert.Equal(lessThan, CompareTo(null, a));
            Assert.Equal(lessThan, CompareTo(null, b));
            Assert.Equal(expected, a.CompareTo(b));
            Assert.Equal(-expected, b.CompareTo(a));
            Assert.Equal(expected, CompareTo(a, b));
            Assert.Equal(-expected, CompareTo(b, a));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="expectedMsb"></param>
        [Theory, MemberData(nameof(ToBytesData))]
        public void ToBytes_is_correct(IEnumerable<byte> bytes, bool expectedMsb)
        {
            Assert.NotNull(bytes);
            bytes = bytes.ToArray();
            GetSubject(() => CreateBitArrayWithArray(bytes.ToArray()));
            // ReSharper disable once PossibleMultipleEnumeration
            Assert.Equal(bytes.Count() * BitCount, Subject.Count);
            // ReSharper disable once PossibleMultipleEnumeration
            Assert.Equal(bytes.ToArray(), Subject.InternalBytes().ToArray());
            // ReSharper disable once PossibleMultipleEnumeration
            Assert.Equal(expectedMsb
                ? bytes.Reverse()
                // ReSharper disable once PossibleMultipleEnumeration
                : bytes, Subject.ToBytes(expectedMsb));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="expectedMsb"></param>
        [Theory, MemberData(nameof(ToIntsData))]
        public void ToInts_is_correct(IEnumerable<uint> values, bool expectedMsb)
        {
            Assert.NotNull(values);
            values = values.ToArray();
            GetSubject(() => CreateBitArray(values.ToArray()));
            Assert.Equal(values, Subject.ToInts(expectedMsb));
        }
    }
}
