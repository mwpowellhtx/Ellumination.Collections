using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using Xunit;
    using Xunit.Abstractions;

    public partial class ImmutableBitArrayCtorTests : SubjectTestFixtureBase<ImmutableBitArray>
    {
        public ImmutableBitArrayCtorTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="expectedMsb"></param>
        /// <param name="expectedBytes"></param>
        [Theory, MemberData(nameof(CtorUInt32ArrayData))]
        public void Ctor_works_with_UInt32_Array(uint[] values, bool expectedMsb, byte[] expectedBytes)
        {
            Assert.NotNull(expectedBytes);
            GetSubject(() => CreateBitArray(values));
            Assert.Equal(values.Length * sizeof(uint) * 8, Subject.Count);
            Assert.Equal(expectedBytes, Subject.ToBytes(expectedMsb));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="expectedMsb"></param>
        [Theory, MemberData(nameof(CtorByteArrayData))]
        public void Ctor_works_with_Byte_Array(byte[] bytes, bool expectedMsb)
        {
            Assert.NotNull(bytes);
            GetSubject(() => CreateBitArrayWithArray(bytes));
            Assert.Equal(bytes.Length * 8, Subject.Count);
            Assert.Equal(expectedMsb ? bytes.Reverse() : bytes, Subject.ToBytes(expectedMsb));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="expectedMsb"></param>
        [Theory, MemberData(nameof(CtorByteIEnumerableData))]
        public void Ctor_works_with_Byte_IEnumerable(IEnumerable<byte> bytes, bool expectedMsb)
        {
            Assert.NotNull(bytes);
            GetSubject(() => CreateBitArray(bytes));
            // ReSharper disable once PossibleMultipleEnumeration
            Assert.Equal(bytes.Count() * 8, Subject.Count);
            Assert.Equal(expectedMsb
                // ReSharper disable once PossibleMultipleEnumeration
                ? bytes.Reverse()
                // ReSharper disable once PossibleMultipleEnumeration
                : bytes, Subject.ToBytes(expectedMsb));
        }
    }
}
