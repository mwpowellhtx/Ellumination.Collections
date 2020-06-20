using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using Xunit;
    using Xunit.Abstractions;
    using static String;

    public partial class PrimeNumberCollectionTests
    {
        private ITestOutputHelper OutputHelper { get; }

        public PrimeNumberCollectionTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        /// <summary>
        /// Checks that the <paramref name="primeNumbers"/> are indeed Prime.
        /// </summary>
        /// <param name="primeNumbers"></param>
        /// <see cref="!:http://stackoverflow.com/questions/1538644/c-determine-if-a-number-is-prime"/>
        [Theory, MemberData(nameof(PrimeNumbersGenerated))]
        public void Check_prime_numbers(IEnumerable<int> primeNumbers)
        {
            Assert.NotNull(primeNumbers);
            primeNumbers = primeNumbers.ToArray();

            bool IsPrime(int value)
            {
                for (var i = 2; i * i <= value; i++)
                {
                    if (value % i == 0)
                    {
                        return false;
                    }
                }
                return true;
            }

            Assert.All(primeNumbers, x => Assert.True(IsPrime(x), $"Value '{x}' was not a Prime Number."));

            OutputHelper.WriteLine($"Prime Numbers are: {Join(", ", primeNumbers.Select(x => $"{x}"))}");
        }
    }
}
