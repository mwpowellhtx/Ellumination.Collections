using System.Collections.Generic;

namespace Talks.Collections
{
    using Xunit;
    using Xunit.Abstractions;

    // TODO: TBD: from here, we could consider migrating to one of the netcoreapp runtimes rather than a desktop framework runtime...
    // TODO: TBD: which would significantly reduce the complexity in the csproj file itself, and so on.
    public abstract class BitArrayComparisonTests<T>
        where T : class
    {
        protected ITestOutputHelper OutputHelper { get; }

        protected BitArrayComparisonTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        [Fact]
        public void T_Not_An_Interface()
        {
            var type = typeof(T);
            Assert.False(type.IsInterface);
        }

        [Fact]
        public void T_Is_Not_Abstract()
        {
            var type = typeof(T);
            Assert.False(type.IsAbstract);
        }

        protected delegate string ReportValueCallback(string name, int value);

        protected void ReportValue(string name, int value, ReportValueCallback formatter = null)
            => OutputHelper.WriteLine((
                    formatter ?? ((s, x) => $"Testing value '{name}': {value:X8}")
                ).Invoke(name, value)
            );

        protected abstract T GetSubject(int value);

        protected abstract void VerifyBits(T subject, int value);

        public static IEnumerable<object[]> TwoDimCoordinateData
        {
            get
            {
                var coords = new int[] { 1, 2, 3 };
                foreach (var x in coords)
                {
                    foreach (var y in coords)
                    {
                        yield return new object[] { x, y };
                    }
                }
            }
        }

        /// <summary>
        /// Verifies whether the Bitwise And method Is Correct.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        [Theory
            , MemberData(nameof(TwoDimCoordinateData), DisableDiscoveryEnumeration = true)
            ]
        public virtual void Verify_Bitwise_that_Binary_And_operation_is_correct(int x, int y)
            => VerifyBinaryOperation(x, y, GetSubject(x), GetSubject(y), (a, b) => a & b, AndCallback);

        /// <summary>
        /// Verifies whether the Bitwise Or method Is Correct.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        [Theory
            , MemberData(nameof(TwoDimCoordinateData), DisableDiscoveryEnumeration = true)
            ]
        public virtual void Verify_Bitwise_that_Binary_Or_operation_is_correct(int x, int y)
            => VerifyBinaryOperation(x, y, GetSubject(x), GetSubject(y), (a, b) => a | b, OrCallback);

        /// <summary>
        /// Verifies whether the Bitwise Xor method Is Correct.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        [Theory
            , MemberData(nameof(TwoDimCoordinateData), DisableDiscoveryEnumeration = true)
            ]
        public virtual void Verify_Bitwise_that_Binary_Xor_operation_is_correct(int x, int y)
            => VerifyBinaryOperation(x, y, GetSubject(x), GetSubject(y), (a, b) => a ^ b, XorCallback);

        public static IEnumerable<object[]> OneDimCoordinateData
        {
            get
            {
                var coords = new int[] { 1, 2, 3 };
                foreach (var x in coords)
                {
                    yield return new object[] { x };
                }
            }
        }

        /// <summary>
        /// Verifies whether the Bitwise Not method Is Correct.
        /// </summary>
        /// <param name="x"></param>
        [Theory
            , MemberData(nameof(OneDimCoordinateData), DisableDiscoveryEnumeration = true)
            ]
        public virtual void Verify_Bitwise_that_Unary_Not_operation_is_correct(int x)
            => VerifyUnaryOperation(x, GetSubject(x), a => ~a, NotCallback);

        protected delegate TOperand BinaryOperationCallback<TOperand>(TOperand x, TOperand y);

        protected delegate TOperand UnaryOperationCallback<TOperand>(TOperand x);

        protected abstract bool ShouldOperatorsMutateOperands { get; }

        protected virtual void VerifyBinaryOperation(int x, int y, T a, T b
            , BinaryOperationCallback<int> calcIntegerResult
            , BinaryOperationCallback<T> calcSubjectResult)
        {
            ReportValue(nameof(x), x);
            ReportValue(nameof(y), y);
            Assert.NotNull(a);
            Assert.NotNull(b);
            Assert.NotSame(a, b);
            VerifyBits(a, x);
            VerifyBits(b, y);
            var subjectResult = calcSubjectResult(a, b);
            if (ShouldOperatorsMutateOperands)
            {
                Assert.Same(a, subjectResult);
            }
            else
            {
                Assert.NotSame(a, subjectResult);
            }

            Assert.NotSame(b, subjectResult);
            var expectedResult = calcIntegerResult(x, y);
            ReportValue(nameof(expectedResult), expectedResult
                , delegate { return $"Expected result '{nameof(expectedResult)}': {expectedResult:X8}"; });
            VerifyBits(subjectResult, expectedResult);
        }

        protected virtual void VerifyUnaryOperation(int x, T a
            , UnaryOperationCallback<int> calcIntegerResult
            , UnaryOperationCallback<T> calcSubjectResult)
        {
            ReportValue(nameof(x), x);
            Assert.NotNull(a);
            VerifyBits(a, x);
            var expectedResult = calcIntegerResult(x);
            var subjectResult = calcSubjectResult(a);
            if (ShouldOperatorsMutateOperands)
            {
                Assert.Same(a, subjectResult);
            }
            else
            {
                Assert.NotSame(a, subjectResult);
            }

            ReportValue(nameof(expectedResult), expectedResult
                , delegate { return $"Expected result '{nameof(expectedResult)}': {expectedResult:X8}"; });
            VerifyBits(subjectResult, expectedResult);
        }

        protected abstract BinaryOperationCallback<T> AndCallback { get; }

        protected abstract BinaryOperationCallback<T> OrCallback { get; }

        protected abstract BinaryOperationCallback<T> XorCallback { get; }

        protected abstract UnaryOperationCallback<T> NotCallback { get; }
    }
}
