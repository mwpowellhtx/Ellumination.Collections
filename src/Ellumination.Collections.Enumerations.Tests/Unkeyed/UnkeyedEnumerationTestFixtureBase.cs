namespace Ellumination.Collections.Unkeyed
{
    using Xunit;
    using Xunit.Abstractions;

    /// <inheritdoc cref="EnumerationTestFixtureBase{T}" />
    /// <see cref="Enumeration{T}"/>
    public abstract class UnkeyedEnumerationTestFixtureBase<T> : EnumerationTestFixtureBase<T>
        where T : Enumeration<T>
    {
        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="outputHelper"></param>
        /// <param name="reporter"></param>
        /// <inheritdoc />
        protected UnkeyedEnumerationTestFixtureBase(ITestOutputHelper outputHelper, IEnumerationCoverageReporter<T> reporter)
            : base(outputHelper, reporter)
        {
        }

#pragma warning disable xUnit1003 // Test data attribute should only be used on a Theory
        /// <summary>
        /// Verifies that the <see cref="Enumeration{T}"/> looks up the Value correctly to
        /// <paramref name="name"/>. Also verifies the <paramref name="displayName"/> is correct.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        [Theory]
        public virtual void Enumeration_Value_Correct(string name, string displayName)
        {
            var value = Enumeration.FromName<T>(name).AssertNotNull().AssertEqual(name, x => x.Name);
            value.AssertEqual(displayName, x => x.DisplayName);
        }
#pragma warning restore xUnit1003 // Test data attribute should only be used on a Theory

    }
}
