using System;

namespace Ellumination.Collections.Independents
{
    using Xunit;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    public class PublicFlagsEnumerationCtorTests : IndependentEnumerationTestFixtureBase<PublicFlagsEnumerationCtor>
    {
        /// <summary>
        /// Public Constructor.
        /// </summary>
        /// <param name="outputHelper"></param>
        public PublicFlagsEnumerationCtorTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public void There_Are_No_Expected_Ctors() => GetSingle<Action>(() => NullInstance.HasExpectedFlagsCtors()).AssertThrows<NotNullException>();

        [Fact]
        public void Expecting_All_Values_Not_Null() => GetSingle<Action>(() => NullInstance.ShallAllHaveConsistentBitLengths(false)).AssertThrows<NotNullException>();

        [Fact]
        public void Expecting_Consistent_Bit_Lengths() => GetSingle<Action>(() => NullInstance.ShallAllHaveConsistentBitLengths()).AssertThrows<EqualException>();

        [Fact]
        public void Expecting_Uniquely_Assigned_Value_Keys() => GetSingle<Action>(() => NullInstance.KeysShallBeUniquelyAssigned()).AssertThrows<EqualException>();
    }
}
