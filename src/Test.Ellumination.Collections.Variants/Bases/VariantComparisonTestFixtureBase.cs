using System;

namespace Ellumination.Collections.Variants
{
    using Xunit;
    using Xunit.Abstractions;
    using static Collections;

    public abstract class VariantComparisonTestFixtureBase<T, TSubject, TResult> : TestFixtureBase
        where TSubject : Variant<T>
    {
        protected static void VerifyOperands(Variant a, Variant b)
        {
            Assert.All(GetRange(a, b), Assert.NotNull);
        }

        /// <summary>
        /// Supports <see cref="IEquatable{IVariant}"/> as well as
        /// <see cref="IComparable{IVariant}"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected abstract TResult Compare(TSubject a, TSubject b);

        /// <summary>
        /// Supports <see cref="IEquatable{IVariant}"/> as well as
        /// <see cref="IComparable{IVariant}"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected abstract TResult Compare(Variant a, TSubject b);

        /// <summary>
        /// Supports <see cref="IEquatable{IVariant}"/> as well as
        /// <see cref="IComparable{IVariant}"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected abstract TResult Compare(TSubject a, Variant b);

        protected VariantComparisonTestFixtureBase(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

#pragma warning disable xUnit1003 // Theory methods must have test data
        [Theory]
        public virtual void Verify_Compare_Is_Correct(TSubject a, TSubject b, TResult expectedResult)
        {
            VerifyOperands(a, b);
            var actualResult = Compare(a, b);
            Assert.Equal(expectedResult, actualResult);
        }
#pragma warning restore xUnit1003 // Theory methods must have test data

#pragma warning disable xUnit1003 // Theory methods must have test data
        [Theory]
        public virtual void Verify_Compare_Variant_Subject_Is_Correct(Variant a, TSubject b, TResult expectedResult)
        {
            VerifyOperands(a, b);
            var actualResult = Compare(a, b);
            Assert.Equal(expectedResult, actualResult);
        }
#pragma warning restore xUnit1003 // Theory methods must have test data

#pragma warning disable xUnit1003 // Theory methods must have test data
        [Theory]
        public virtual void Verify_Compare_Subject_Variant_Is_Correct(TSubject a, Variant b, TResult expectedResult)
        {
            VerifyOperands(a, b);
            var actualResult = Compare(a, b);
            Assert.Equal(expectedResult, actualResult);
        }
#pragma warning restore xUnit1003 // Theory methods must have test data

    }
}
