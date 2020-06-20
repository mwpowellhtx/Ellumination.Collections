using System;

namespace Ellumination.Collections
{
    using Xunit;
    using Xunit.Abstractions;

    public partial class ImmutableBitArrayGetSetSetAllTests
        : SubjectTestFixtureBase<ImmutableBitArray>
    {
        public ImmutableBitArrayGetSetSetAllTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        /// <summary>
        /// In this case we Verify that Get works correctly, leaving off consideration for
        /// <see cref="Elasticity"/> behavior.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <param name="expected"></param>
        /// <see cref="ImmutableBitArray.Get(int)"/>
        [Theory, MemberData(nameof(InelasticXetData))]
        public void Verify_that_Inelastic_Get_works_correctly(uint value, int index, bool expected)
        {
            OutputHelper.WriteLine($"Value '{value:X08}' bit {index} expected to be {expected}.");
            GetSubject(() => CreateBitArray(value));
            Assert.Equal(expected, Subject.Get(index));
        }

        /// <summary>
        /// In this case we Verify that Get and Set works correctly, leaving off consideration for
        /// <see cref="Elasticity"/> behavior. We Get and Set a couple different times and either
        /// side of the <paramref name="expected"/> result for completeness.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <param name="expected"></param>
        /// <see cref="ImmutableBitArray.Get(int)"/>
        /// <see cref="ImmutableBitArray.Set(int, bool)"/>
        [Theory, MemberData(nameof(InelasticXetData))]
        public void Verify_that_Inelastic_Set_works_correctly(uint value, int index, bool expected)
        {
            OutputHelper.WriteLine($"Value '{value:X08}' bit {index} expected to be {expected}.");
            GetSubject(() => CreateBitArray(value));
            Assert.Equal(expected, Subject.Get(index));
            Subject.Set(index, expected);
            Assert.Equal(expected, Subject.Get(index));
            Subject.Set(index, !expected);
            Assert.Equal(!expected, Subject.Get(index));
            Subject.Set(index, !expected);
            Assert.Equal(!expected, Subject.Get(index));
        }

        [Theory, MemberData(nameof(BadInelasticXetData))]
        public void Verify_that_Bad_Inelastic_Get_throws(uint value, int index)
        {
            OutputHelper.WriteLine($"Attempting to Get with index '{index}'.");
            GetSubject(() => CreateBitArray(value));
            Assert.Throws<ArgumentOutOfRangeException>(() => Subject.Get(index))
                // ReSharper disable once ImplicitlyCapturedClosure
                .WithExceptionDetail(aoorex =>
                {
                    Assert.Equal(nameof(index), aoorex.ParamName);
                    Assert.Equal(index, aoorex.ActualValue);
                });
        }

        [Theory, MemberData(nameof(BadInelasticXetData))]
        public void Verify_that_Bad_Inelastic_Set_throws(uint value, int index)
        {
            OutputHelper.WriteLine($"Attempting to Set with index '{index}'.");
            GetSubject(() => CreateBitArray(value));
            Assert.Throws<ArgumentOutOfRangeException>(() => Subject.Set(index, default(bool)))
                // ReSharper disable once ImplicitlyCapturedClosure
                .WithExceptionDetail(aoorex =>
                {
                    Assert.Equal(nameof(index), aoorex.ParamName);
                    Assert.Equal(index, aoorex.ActualValue);
                });
        }
    }
}
