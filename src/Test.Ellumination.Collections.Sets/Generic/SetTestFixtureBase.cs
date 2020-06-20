namespace Ellumination.Collections.Generic
{
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Provides some foundational <see cref="Set{T}"/> fixture bits.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SetTestFixtureBase<T> : TestFixtureBase
    {
        /// <summary>
        /// Gets a new <see cref="Set{T}"/> Instance.
        /// </summary>
        protected virtual Set<T> Instance => new Set<T>().AssertNotNull().AssertEmpty();

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="outputHelper"></param>
        protected SetTestFixtureBase(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }
    }
}
