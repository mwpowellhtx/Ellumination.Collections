using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Keyed.Ordinals
{
    using Xunit;
    using Xunit.Abstractions;
    using static CardinalDirectionNames;

    /// <inheritdoc />
    public class IntegerCardinalDirectionTests : OrdinalEnumerationTestFixtureBase<int, IntegerCardinalDirection>
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        /// <inheritdoc />
        public IntegerCardinalDirectionTests(ITestOutputHelper outputHelper, EnumerationCoverageReporter<IntegerCardinalDirection> reporter)
            : base(outputHelper, reporter)
        {
        }

        private static IEnumerable<object[]> _enumerationKeyedTestCases;

        public static IEnumerable<object[]> EnumerationKeyedTestCases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(ref int o, string name) => GetRange<object>(
                        o++, name, name.GetHumanReadableCamelCase()
                    );

                    // ReSharper disable InconsistentNaming
                    const string North = nameof(North);
                    const string NorthWest = nameof(NorthWest);
                    const string West = nameof(West);
                    const string SouthWest = nameof(SouthWest);
                    const string South = nameof(South);
                    const string SouthEast = nameof(SouthEast);
                    const string East = nameof(East);
                    const string NorthEast = nameof(NorthEast);
                    // ReSharper restore InconsistentNaming

                    var ordinal = 0;

                    yield return GetOne(ref ordinal, North).ToArray();
                    yield return GetOne(ref ordinal, NorthWest).ToArray();
                    yield return GetOne(ref ordinal, West).ToArray();
                    yield return GetOne(ref ordinal, SouthWest).ToArray();
                    yield return GetOne(ref ordinal, South).ToArray();
                    yield return GetOne(ref ordinal, SouthEast).ToArray();
                    yield return GetOne(ref ordinal, East).ToArray();
                    yield return GetOne(ref ordinal, NorthEast).ToArray();
                }

                return _enumerationKeyedTestCases ?? (_enumerationKeyedTestCases = GetAll().ToArray());
            }
        }

        private static IEnumerable<object[]> _enumerationNamedTestCases;

        public static IEnumerable<object[]> EnumerationNamedTestCases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(string name) => GetRange<object>(
                        name, name.GetHumanReadableCamelCase()
                    );

                    yield return GetOne(North).ToArray();
                    yield return GetOne(NorthWest).ToArray();
                    yield return GetOne(West).ToArray();
                    yield return GetOne(SouthWest).ToArray();
                    yield return GetOne(South).ToArray();
                    yield return GetOne(SouthEast).ToArray();
                    yield return GetOne(East).ToArray();
                    yield return GetOne(NorthEast).ToArray();
                }

                return _enumerationNamedTestCases ?? (_enumerationNamedTestCases = GetAll().ToArray());
            }
        }

#pragma warning disable xUnit1008 // Test data attribute should only be used on a Theory
        /// <inheritdoc />
        [MemberData(nameof(EnumerationKeyedTestCases))]
        public override void Enumeration_Value_Key_Correct(int key, string name, string displayName)
            => base.Enumeration_Value_Key_Correct(key, name, displayName);

        /// <inheritdoc />
        [MemberData(nameof(EnumerationNamedTestCases))]
        public override void Verify_Base_Enumeration_Names(string name, string displayName)
            => base.Verify_Base_Enumeration_Names(name, displayName);
#pragma warning restore xUnit1008 // Test data attribute should only be used on a Theory

    }
}
