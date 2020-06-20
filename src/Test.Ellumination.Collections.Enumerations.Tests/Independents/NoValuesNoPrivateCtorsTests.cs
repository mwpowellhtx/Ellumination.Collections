using System;
using System.Linq;

namespace Ellumination.Collections.Independents
{
    using Xunit;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    /// <summary>
    /// This set of unit tests asserts several negative consequences of a badly formed
    /// <see cref="Enumeration"/> pattern. Chiefly, the <see cref="NoValuesNoPrivateCtors"/>
    /// <see cref="Unkeyed.Enumeration{T}"/> Has No values, Does Not Have an Expected Private
    /// Constructor, and Does Have a Public Constructor.
    /// </summary>
    public class NoValuesNoPrivateCtorsTests : IndependentEnumerationTestFixtureBase<NoValuesNoPrivateCtors>
    {
        public NoValuesNoPrivateCtorsTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public void Does_Not_Have_Expected_Ctors() => GetSingle<Action>(() => NullInstance.HasExpectedCtors(CtorInspectors.ToArray())).AssertThrows<NotEmptyException>();

        [Fact]
        public void Does_Not_Have_Any_Values() => GetSingle<Action>(() => NullInstance.ShallHaveAtLeastOneValue()).AssertThrows<NotEmptyException>();

        [Fact]
        public void Does_Have_One_Public_Ctor() => GetSingle<Action>(() => NullInstance.ShallNotHaveAnyPublicCtors()).AssertThrows<EmptyException>();
    }
}
