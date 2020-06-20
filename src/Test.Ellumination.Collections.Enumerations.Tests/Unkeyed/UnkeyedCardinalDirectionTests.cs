using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Unkeyed
{
    using Xunit;
    using Xunit.Abstractions;
    using static CardinalDirectionNames;

    /// <inheritdoc />
    public class UnkeyedCardinalDirectionTests : UnkeyedEnumerationTestFixtureBase<UnkeyedCardinalDirection>
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        /// <inheritdoc />
        public UnkeyedCardinalDirectionTests(ITestOutputHelper outputHelper, EnumerationCoverageReporter<UnkeyedCardinalDirection> reporter)
            : base(outputHelper, reporter)
        {
        }

        private static IEnumerable<object[]> _enumerationNamedTestCases;

        public static IEnumerable<object[]> EnumerationNamedTestCases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    // We do it this way in order to avoid Enumeration initialization until the test case itself.
                    IEnumerable<object> GetOne(string name) => GetRange<object>(name, name);

                    yield return GetOne(North).ToArray();
                    yield return GetOne(West).ToArray();
                    yield return GetOne(South).ToArray();
                    yield return GetOne(East).ToArray();
                }

                return _enumerationNamedTestCases ?? (_enumerationNamedTestCases = GetAll().ToArray());
            }
        }

#pragma warning disable xUnit1008 // Test data attribute should only be used on a Theory
        /// <inheritdoc />
        [MemberData(nameof(EnumerationNamedTestCases))]
        public override void Enumeration_Value_Correct(string name, string displayName)
            => base.Enumeration_Value_Correct(name, displayName);

        /// <inheritdoc />
        [MemberData(nameof(EnumerationNamedTestCases))]
        public override void Verify_Base_Enumeration_Names(string name, string displayName)
            => base.Verify_Base_Enumeration_Names(name, displayName);
#pragma warning restore xUnit1008 // Test data attribute should only be used on a Theory

    }
}
