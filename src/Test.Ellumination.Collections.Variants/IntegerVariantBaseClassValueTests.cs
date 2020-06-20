namespace Ellumination.Collections.Variants
{
    using Xunit;
    using Xunit.Abstractions;

    // ReSharper disable once UnusedMember.Global
    public class IntegerVariantBaseClassValueTests : VariantBaseClassValueTestFixtureBase<int>
    {
        public IntegerVariantBaseClassValueTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

#pragma warning disable xUnit1008 // Test data attribute should only be used on a theory
        /// <inheritdoc />
        [InlineData(int.MinValue)
         , InlineData(int.MaxValue)]
        public override void Can_Replace_Like_Types(int value) => base.Can_Replace_Like_Types(value);
#pragma warning restore xUnit1008 // Test data attribute should only be used on a theory

#pragma warning disable xUnit1008 // Test data attribute should only be used on a theory
        /// <inheritdoc />
        [InlineData(long.MaxValue)
         , InlineData(byte.MaxValue)
         , InlineData(char.MaxValue)
         , InlineData("")]
        public override void Cannot_Replace_Disparate_Types(object value) => base.Cannot_Replace_Disparate_Types(value);
#pragma warning restore xUnit1008 // Test data attribute should only be used on a theory

    }
}
