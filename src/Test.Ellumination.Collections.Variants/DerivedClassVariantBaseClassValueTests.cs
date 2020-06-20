using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Variants
{
    using Xunit;
    using Xunit.Abstractions;
    using static Collections;

    public class DerivedClassVariantBaseClassValueTests : VariantBaseClassValueTestFixtureBase<DerivedClass>
    {
        public DerivedClassVariantBaseClassValueTests(ITestOutputHelper outputHelper)
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            : base(outputHelper, new DerivedClass { })
        {
        }

        private static IEnumerable<object[]> _canReplaceTestCases;

        public static IEnumerable<object[]> CanReplaceTestCases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
                    yield return GetRange<object>(new DerivedClass { }).ToArray();
                    yield return GetRange<object>((DerivedClass) null).ToArray();
                }

                return _canReplaceTestCases ?? (_canReplaceTestCases = GetAll()).ToArray();
            }
        }

#pragma warning disable xUnit1008 // Test data attribute should only be used on a theory
        [MemberData(nameof(CanReplaceTestCases))]
        public override void Can_Replace_Like_Types(DerivedClass value) => base.Can_Replace_Like_Types(value);
#pragma warning restore xUnit1008 // Test data attribute should only be used on a theory

        private static IEnumerable<object[]> _cannotReplaceTestCases;

        public static IEnumerable<object[]> CannotReplaceTestCases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
                    yield return GetRange<object>(new BaseClass { }).ToArray();
                    yield return GetRange<object>(Guid.NewGuid()).ToArray();
                    yield return GetRange<object>(double.NaN).ToArray();
                }

                return _cannotReplaceTestCases ?? (_cannotReplaceTestCases = GetAll()).ToArray();
            }
        }

#pragma warning disable xUnit1008 // Test data attribute should only be used on a theory
        [MemberData(nameof(CannotReplaceTestCases))]
        public override void Cannot_Replace_Disparate_Types(object replacementValue) => base.Cannot_Replace_Disparate_Types(replacementValue);
#pragma warning restore xUnit1008 // Test data attribute should only be used on a theory

    }
}
