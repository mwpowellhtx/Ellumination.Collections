using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ellumination.Collections
{
    using Xunit;
    using Xunit.Abstractions;
    using ConstructorInfoInspector = Action<ConstructorInfo>;

    /// <summary>
    /// Provides Base related <see cref="Enumeration"/> unit tests.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EnumerationTestFixtureBase<T> : TestFixtureBase, IEnumerationTestFixtureBase<T>
        where T : Enumeration
    {
        /// <summary>
        /// Returns a single <see cref="NullInstance"/> for use throughout the framework.
        /// </summary>
        /// <returns></returns>
        private static T GetNullInstance() => null;

        /// <summary>
        /// Gets a single NullInstance for use throughout the framework.
        /// </summary>
        /// <see cref="GetNullInstance"/>
        protected static T NullInstance { get; } = GetNullInstance().AssertNull();

        /// <summary>
        /// Gets the Reporter.
        /// </summary>
        protected IEnumerationCoverageReporter<T> Reporter { get; }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="outputHelper"></param>
        /// <param name="reporter"></param>
        /// <inheritdoc />
        protected EnumerationTestFixtureBase(ITestOutputHelper outputHelper, IEnumerationCoverageReporter<T> reporter)
            : base(outputHelper)
        {
            Reporter = reporter;
        }

        /// <summary>
        /// By definition, Enumerations are not to have Public Constructors.
        /// </summary>
        /// <see cref="NullInstance"/>
        /// <see cref="EnumerationTestCaseExtensionMethods.ShallNotHaveAnyPublicCtors{T}"/>
        public virtual void Shall_Not_Have_Any_Public_Ctors() => NullInstance.ShallNotHaveAnyPublicCtors(Reporter);

        /// <summary>
        /// Returns the Default set of <see cref="ConstructorInfo"/> Inspectors.
        /// Override the property or inject a local variable when the requirements
        /// shift in any way whatsoever.
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
        /// Verifies whether <see cref="Enumeration"/> Has the Expected Constructors.
        /// </summary>
        /// <see cref="NullInstance"/>
        /// <see cref="EnumerationTestCaseExtensionMethods.HasExpectedCtors{T}"/>
        [Fact]
        public virtual void Has_Expected_Ctors()
        {
            NullInstance.HasExpectedCtors(CtorInspectors.ToArray());
            Reporter.Report(Enumeration.GetValues<T>().Select(x => x.Name).ToArray());
        }

        /// <summary>
        /// Reports whether the <typeparamref name="T"/> <see cref="Enumeration"/>
        /// Shall Have at least One Value.
        /// </summary>
        /// <see cref="NullInstance"/>
        /// <see cref="EnumerationTestCaseExtensionMethods.ShallHaveAtLeastOneValue{T}"/>
        [Fact]
        public void Shall_Have_At_Least_One_Value() => NullInstance.ShallHaveAtLeastOneValue(Reporter);

#pragma warning disable xUnit1003 // Theory methods must have test data
        /// <summary>
        /// Verifies that the Enumerated <typeparamref name="T"/> Value is correct corresponding
        /// to the given parameters. Static data must also be provided at the time the tests are
        /// derived into the specific test case.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        [Theory]
        public virtual void Verify_Base_Enumeration_Names(string name, string displayName)
        {
            var value = NullInstance.GetValueByName(name.AssertNotNull().AssertNotEmpty());
            var other = NullInstance.GetValueByDisplayName(displayName.AssertNotNull().AssertNotEmpty());
            other.AssertSame(value);
            Reporter.Report(value.Name);
        }
#pragma warning restore xUnit1003 // Theory methods must have test data

    }
}
