using System;

namespace Ellumination.Collections.Keyed
{
    using Xunit;
    using Xunit.Abstractions;

    /// <inheritdoc cref="EnumerationTestFixtureBase{T}" />
    public abstract class KeyedEnumerationTestFixtureBase<TKey, T> : EnumerationTestFixtureBase<T>
        where T : Enumeration<TKey, T>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="outputHelper"></param>
        /// <param name="reporter"></param>
        /// <inheritdoc />
        protected KeyedEnumerationTestFixtureBase(ITestOutputHelper outputHelper, IEnumerationCoverageReporter<T> reporter)
            : base(outputHelper, reporter)
        {
        }

        /// <summary>
        /// Enumerated Keys shall all be uniquely assigned. Gaps in Keyed coverage are
        /// acceptable, however, unassigned or duplicate Keys are not acceptable.
        /// </summary>
        /// <see cref="KeyedEnumerationTestCaseExtensionMethods.KeysShallBeUniquelyAssigned{TKey,T}"/>
        [Fact]
        public virtual void Keys_Shall_Be_Uniquely_Assigned() => NullInstance.KeysShallBeUniquelyAssigned<TKey, T>(Reporter, OutputHelper);

#pragma warning disable xUnit1003 // Test data attribute should only be used on a Theory
        /// <summary>
        /// Verifies that the <see cref="Enumeration{TKey,T}"/> <paramref name="key"/>
        /// looks up the Value correctly. Also verifies the <paramref name="name"/> and
        /// <paramref name="displayName"/> are correct.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        [Theory]
        public virtual void Enumeration_Value_Key_Correct(TKey key, string name, string displayName)
        {
            var value = Enumeration<TKey, T>.FromKey(key).AssertNotNull().AssertEqual(key, x => x.Key);
            value.AssertEqual(name, x => x.Name);
            value.AssertEqual(displayName, x => x.DisplayName);
        }
#pragma warning restore xUnit1003 // Test data attribute should only be used on a Theory

    }
}
