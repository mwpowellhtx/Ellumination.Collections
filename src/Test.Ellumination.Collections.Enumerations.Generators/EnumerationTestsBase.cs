namespace Ellumination.Collections
{
    using Keyed.Flags;
    using Xunit;

    public abstract class EnumerationTestsBase<T>
        where T : Enumeration<T>
    {
        /// <summary>
        /// Check that there is at least one value defined.
        /// </summary>
        [Fact]
        public void Enumeration_has_at_least_one_Value()
        {
            var values = Enumeration<T>.Values;
            Assert.NotNull(values);
            Assert.NotEmpty(values);
        }
    }
}
