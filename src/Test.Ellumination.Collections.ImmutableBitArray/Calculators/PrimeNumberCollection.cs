using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Calculators
{
    internal class PrimeNumberCollection : IReadOnlyCollection<int>
    {
        private readonly IList<int> _values;

        internal PrimeNumberCollection(int max)
        {
            _values = CalculatePrimeNumbers(max).ToList();
        }

        public int Count => _values.Count;

        public IEnumerator<int> GetEnumerator() => _values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Returns the set of Calculated Prime Numbers no greater than <paramref name="max"/>. 
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        /// <remarks>Algorithm based on link in the notes. I discovered that they had a technical
        /// error in the notes and made the adjustment on wikipedia.</remarks>
        /// <see cref="!:http://en.wikipedia.org/wiki/Sieve_of_Eratosthenes#Pseudocode"/>
        private static IEnumerable<int> CalculatePrimeNumbers(int max)
        {
            if (max <= 1)
            {
                throw new ArgumentException($"Let '{nameof(max)}' be greater than 1", nameof(max));
            }

            // Doing so in terms of Dictionary helps with performance.
            var found = new Dictionary<int, bool>();

            for (var i = 2; i < max; i++)
            {
                if (found.TryGetValue(i, out var current) && !current)
                {
                    continue;
                }

                // We start off by assuming we may have a Prime Number.
                found[i] = true;

                int CalculateNonCandidate(int j) => i * i + i * j;

                // Then we filter out any that actually are not Prime Numbers.
                for (var j = 0; CalculateNonCandidate(j) < max * max; j++)
                {
                    found[CalculateNonCandidate(j)] = false;
                }
            }

            // Afterwards we simply return the Found Actual Prime Numbers.
            var actual = found.Where(item => item.Value).ToArray();

            IEnumerable<T> GetRange<T>(params T[] values) => values;

            return actual.Any()
                ? actual.Select(item => item.Key).OrderBy(prime => prime).ToArray()
                : GetRange<int>();
        }
    }
}
