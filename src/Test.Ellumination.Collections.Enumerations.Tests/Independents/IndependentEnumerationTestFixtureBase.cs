using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ellumination.Collections.Independents
{
    using Xunit;
    using Xunit.Abstractions;
    using ConstructorInfoInspector = Action<ConstructorInfo>;

    public abstract class IndependentEnumerationTestFixtureBase<T> : TestFixtureBase
        where T : Enumeration
    {
        /// <summary>
        /// Returns a Null Instance of the <typeparamref name="T"/> <see cref="Enumeration"/> type.
        /// </summary>
        /// <returns></returns>
        private static T GetNullInstance() => null;

        /// <summary>
        /// Gets the <see cref="GetNullInstance"/> instance.
        /// </summary>
        protected static T NullInstance { get; } = GetNullInstance().AssertNull();

        /// <summary>
        /// Gets a default set of <see cref="ConstructorInfoInspector"/> instances.
        /// </summary>
        protected virtual IEnumerable<ConstructorInfoInspector> CtorInspectors
        {
            get
            {
                yield return ctorInfo => ctorInfo.AssertTrue(x => x.IsPrivate)
                    .GetParameters().AssertNotNull().AssertEmpty();
            }
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="outputHelper"></param>
        protected IndependentEnumerationTestFixtureBase(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        /// <summary>
        /// Verifies that a Collection is always returned, although that Collection
        /// may not have any values populating its contents.
        /// </summary>
        [Fact]
        public void Values_Collection_Always_Returned() => Enumeration.GetValues<T>().AssertNotNull();
    }
}
