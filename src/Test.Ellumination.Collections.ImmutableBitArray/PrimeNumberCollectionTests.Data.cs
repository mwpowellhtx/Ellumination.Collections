using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using Calculators;

    public partial class PrimeNumberCollectionTests
    {
        private static IEnumerable<T> GetRange<T>(params T[] values) => values;

        private static IEnumerable<object[]> _primeNumbersGenerated;

        public static IEnumerable<object[]> PrimeNumbersGenerated
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(IEnumerable<int> primeNumbers)
                    {
                        yield return GetRange(primeNumbers.ToArray());
                    }

                    yield return GetOne(new PrimeNumberCollection(100)).ToArray();
                }

                return _primeNumbersGenerated ?? (_primeNumbersGenerated = GetAll());
            }
        }
    }
}
