using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ellumination.Collections.Keyed.Flags
{
    using Xunit;
    using Xunit.Abstractions;
    using static CardinalDirectionNames;
    using ConstructorInfoInspector = Action<ConstructorInfo>;

    /// <inheritdoc />
    public class FlagsCardinalDirectionTests : FlagsEnumerationTestFixtureBase<FlagsCardinalDirection>
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        /// <inheritdoc />
        public FlagsCardinalDirectionTests(ITestOutputHelper outputHelper, EnumerationCoverageReporter<FlagsCardinalDirection> reporter)
            : base(outputHelper, reporter)
        {
        }

        protected override IEnumerable<ConstructorInfoInspector> CtorInspectors
        {
            get
            {
                yield return ctorInfo => ctorInfo.AssertNotNull().AssertTrue(x => x.IsPrivate)
                    .GetParameters().AssertNotNull().AssertEmpty();

                var bytes = GetRange<byte>().ToArray();

                yield return ctorInfo => ctorInfo.AssertNotNull().AssertTrue(x => x.IsPrivate)
                    .GetParameters().AssertNotNull().AssertCollection(
                        argInfo => argInfo.AssertNotNull()
                            .AssertEqual(nameof(bytes), x => x.Name)
                            .AssertEqual(bytes.GetType(), x => x.ParameterType)
                    );
            }
        }

        private static IEnumerable<object[]> _enumerationFlagsTestCases;

        public static IEnumerable<object[]> EnumerationFlagsTestCases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(ref byte b, string name)
                    {
                        // Order of operations matters, especially here.
                        var x = b;
                        b <<= 1;
                        return GetRange<object>(GetRange(x).ToArray(), name, name.GetHumanReadableCamelCase());
                    }

                    byte bytes = 0x1;

                    yield return GetOne(ref bytes, North).ToArray();
                    yield return GetOne(ref bytes, NorthWest).ToArray();
                    yield return GetOne(ref bytes, West).ToArray();
                    yield return GetOne(ref bytes, SouthWest).ToArray();
                    yield return GetOne(ref bytes, South).ToArray();
                    yield return GetOne(ref bytes, SouthEast).ToArray();
                    yield return GetOne(ref bytes, East).ToArray();
                    yield return GetOne(ref bytes, NorthEast).ToArray();
                }

                return _enumerationFlagsTestCases ?? (_enumerationFlagsTestCases = GetAll().ToArray());
            }
        }

        private static IEnumerable<object[]> _enumerationKeyedTestCases;

        public static IEnumerable<object[]> EnumerationKeyedTestCases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(ref ImmutableBitArray k, string name)
                    {
                        // Order of operations matters, especially here.
                        var x = k;
                        k <<= 1;
                        return GetRange<object>(x, name, name.GetHumanReadableCamelCase());
                    }

                    var key = new ImmutableBitArray(0x1);

                    yield return GetOne(ref key, North).ToArray();
                    yield return GetOne(ref key, NorthWest).ToArray();
                    yield return GetOne(ref key, West).ToArray();
                    yield return GetOne(ref key, SouthWest).ToArray();
                    yield return GetOne(ref key, South).ToArray();
                    yield return GetOne(ref key, SouthEast).ToArray();
                    yield return GetOne(ref key, East).ToArray();
                    yield return GetOne(ref key, NorthEast).ToArray();
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
                    IEnumerable<object> GetOne(string name)
                        => GetRange<object>(name, name.GetHumanReadableCamelCase());

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
        [MemberData(nameof(EnumerationFlagsTestCases))]
        public override void Has_Flags_Enumerated_Value(byte[] bytes, string name, string displayName)
            => base.Has_Flags_Enumerated_Value(bytes, name, displayName);

        /// <inheritdoc />
        [MemberData(nameof(EnumerationKeyedTestCases))]
        public override void Enumeration_Value_Key_Correct(ImmutableBitArray key, string name, string displayName)
            => base.Enumeration_Value_Key_Correct(key, name, displayName);

        /// <inheritdoc />
        [MemberData(nameof(EnumerationNamedTestCases))]
        public override void Verify_Base_Enumeration_Names(string name, string displayName)
            => base.Verify_Base_Enumeration_Names(name, displayName);
#pragma warning restore xUnit1008 // Test data attribute should only be used on a Theory

    }
}
