using System;

namespace Ellumination.Collections.Independents
{
    using Xunit;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    public class ProtectedBitwiseCtorTests : IndependentEnumerationTestFixtureBase<ProtectedBitwiseCtor>
    {
        /// <summary>
        /// Public Constructor.
        /// </summary>
        /// <param name="outputHelper"></param>
        public ProtectedBitwiseCtorTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public void Expected_Ctor_Is_Not_Private() => GetSingle<Action>(() => NullInstance.HasExpectedFlagsCtors()).AssertThrows<TrueException>();

        [Fact]
        public void Expecting_at_least_One_value() => GetSingle<Action>(() => NullInstance.ShallAllHaveConsistentBitLengths()).AssertThrows<NotEmptyException>();
    }
}
