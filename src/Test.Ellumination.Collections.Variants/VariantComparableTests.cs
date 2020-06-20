namespace Ellumination.Collections.Variants
{
    using Xunit;
    using Xunit.Abstractions;

    // ReSharper disable once UnusedMember.Global
    public class VariantComparableTests : VariantComparisonTestFixtureBase<int, Variant<int>, int>
    {
        protected override int Compare(Variant<int> a, Variant<int> b) => a.CompareTo(b);

        protected override int Compare(Variant<int> a, Variant b) => a.CompareTo(b);

        protected override int Compare(Variant a, Variant<int> b) => a.CompareTo(b);

        public VariantComparableTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

#pragma warning disable xUnit1008 // Test data attribute should only be used on a theory
        [ClassData(typeof(CompareToSubjectSubjectClassData))]
        public override void Verify_Compare_Is_Correct(Variant<int> a, Variant<int> b, int expectedResult)
            => base.Verify_Compare_Is_Correct(a, b, expectedResult);
#pragma warning restore xUnit1008 // Test data attribute should only be used on a theory

#pragma warning disable xUnit1008 // Test data attribute should only be used on a theory
        [ClassData(typeof(CompareToSubjectVariantClassData))]
        public override void Verify_Compare_Subject_Variant_Is_Correct(Variant<int> a, Variant b, int expectedResult)
            => base.Verify_Compare_Subject_Variant_Is_Correct(a, b, expectedResult);
#pragma warning restore xUnit1008 // Test data attribute should only be used on a theory

#pragma warning disable xUnit1008 // Test data attribute should only be used on a theory
        [ClassData(typeof(CompareToVariantSubjectClassData))]
        public override void Verify_Compare_Variant_Subject_Is_Correct(Variant a, Variant<int> b, int expectedResult)
            => base.Verify_Compare_Variant_Subject_Is_Correct(a, b, expectedResult);
#pragma warning restore xUnit1008 // Test data attribute should only be used on a theory

    }
}
