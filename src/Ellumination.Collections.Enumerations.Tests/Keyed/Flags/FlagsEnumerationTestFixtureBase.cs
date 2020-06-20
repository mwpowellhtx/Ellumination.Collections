using System.Linq;

namespace Ellumination.Collections.Keyed.Flags
{
    using Xunit;
    using Xunit.Abstractions;

    /// <inheritdoc cref="KeyedEnumerationTestFixtureBase{TKey,T}" />
    /// <see cref="Enumeration{T}"/>
    public abstract class FlagsEnumerationTestFixtureBase<T> : KeyedEnumerationTestFixtureBase<ImmutableBitArray, T>
        where T : Enumeration<T>
    {
        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="outputHelper"></param>
        /// <param name="reporter"></param>
        /// <inheritdoc />
        protected FlagsEnumerationTestFixtureBase(ITestOutputHelper outputHelper, IEnumerationCoverageReporter<T> reporter)
            : base(outputHelper, reporter)
        {
        }

        /// <summary>
        /// The Enumeration Has the Expected Constructors.
        /// </summary>
        [Fact]
        public virtual void Has_Expected_Flags_Ctors() => NullInstance.HasExpectedFlagsCtors(Reporter);

#pragma warning disable xUnit1003 // Theory methods must have test data
        /// <summary>
        /// Verifies that the Enumerated <typeparamref name="T"/> Value is correct corresponding
        /// to the given parameters. Static data must also be provided at the time the tests are
        /// derived into the specific test case.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        [Theory]
        public virtual void Has_Flags_Enumerated_Value(byte[] bytes, string name, string displayName)
        {
            // There must be at least one Non Zero Byte.
            bytes.AssertNotNull().AssertNotEmpty().AssertTrue(x => x.Any(y => y > 0));

            T VerifyAgainstRoot(T rootInstance)
            {
                var value = rootInstance.GetValueByBytes(bytes).AssertNotNull();

                rootInstance.GetValueByName(name.AssertNotNull().AssertNotEmpty())
                    .AssertSame(value.AssertEqual(name, x => x.Name));

                rootInstance.GetValueByDisplayName(displayName.AssertNotNull().AssertNotEmpty())
                    .AssertSame(value.AssertEqual(displayName, x => x.DisplayName));

                return value;
            }

            Reporter.Report(VerifyAgainstRoot(NullInstance).Name);
        }
#pragma warning restore xUnit1003 // Theory methods must have test data

    }
}
