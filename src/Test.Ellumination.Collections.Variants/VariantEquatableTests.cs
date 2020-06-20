namespace Ellumination.Collections.Variants
{
    using Xunit;
    using Xunit.Abstractions;

    // ReSharper disable once UnusedMember.Global
    public class VariantEquatableTests : VariantComparisonTestFixtureBase<int, Variant<int>, bool>
    {
        protected override bool Compare(Variant<int> a, Variant<int> b) => a.Equals(b);

        protected override bool Compare(Variant<int> a, Variant b) => a.Equals(b);

        protected override bool Compare(Variant a, Variant<int> b) => a.Equals(b);

        public VariantEquatableTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

#pragma warning disable xUnit1008 // Test data attribute should only be used on a theory
        [ClassData(typeof(EquatableSubjectSubjectClassData))]
        public override void Verify_Compare_Is_Correct(Variant<int> a, Variant<int> b, bool expectedResult)
            => base.Verify_Compare_Is_Correct(a, b, expectedResult);
#pragma warning restore xUnit1008 // Test data attribute should only be used on a theory

#pragma warning disable xUnit1008 // Test data attribute should only be used on a theory
        [ClassData(typeof(EquatableSubjectVariantClassData))]
        public override void Verify_Compare_Subject_Variant_Is_Correct(Variant<int> a, Variant b, bool expectedResult)
            => base.Verify_Compare_Subject_Variant_Is_Correct(a, b, expectedResult);
#pragma warning restore xUnit1008 // Test data attribute should only be used on a theory

#pragma warning disable xUnit1008 // Test data attribute should only be used on a theory
        [ClassData(typeof(EquatableVariantSubjectClassData))]
        public override void Verify_Compare_Variant_Subject_Is_Correct(Variant a, Variant<int> b, bool expectedResult)
            => base.Verify_Compare_Variant_Subject_Is_Correct(a, b, expectedResult);
#pragma warning restore xUnit1008 // Test data attribute should only be used on a theory

    }
}
