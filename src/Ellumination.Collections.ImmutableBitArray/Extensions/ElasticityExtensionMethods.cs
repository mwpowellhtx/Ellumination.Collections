using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    /// <summary>
    /// <see cref="Elasticity"/> extension methods.
    /// </summary>
    public static class ElasticityExtensionMethods
    {
        /// <summary>
        /// Returns whether <paramref name="value"/> Contains the <paramref name="mask"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public static bool Contains(this Elasticity value, Elasticity mask)
            => (value & mask) == mask;

        /// <summary>
        /// Returns whether <paramref name="value"/> Contains the <paramref name="mask"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public static bool Contains(this Elasticity? value, Elasticity mask)
            => value.HasValue && (value & mask) == mask;

        /// <summary>
        /// Truncation returns the <paramref name="values"/> as is when
        /// <see cref="Elasticity.Expansion"/> is provided. Otherwise, we take what is there,
        /// or the <paramref name="originalLength"/> if necessary, which ever is smallest.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="originalLength"></param>
        /// <param name="elasticity"></param>
        /// <returns></returns>
        internal static IEnumerable<bool> Truncate(this IEnumerable<bool> values
            , int originalLength, Elasticity elasticity)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            var valuesCount = values.Count();

            return elasticity.Contains(Elasticity.Expansion)
                // ReSharper disable once PossibleMultipleEnumeration
                ? values
                // ReSharper disable once PossibleMultipleEnumeration
                : values.Take(Math.Min(originalLength, valuesCount));
        }

        /// <summary>
        /// Shrinking returns the <paramref name="values"/> as is when
        /// <see cref="Elasticity.Contraction"/> is not provided, in either the
        /// <paramref name="originalLength"/> or the given length, which ever is
        /// smallest. When <see cref="Elasticity.Contraction"/> is expected, discards all
        /// trailing false, or zero, bits from the collection. Maintains the assumption that the
        /// Most Significant Bits are the least important to keep when they are false, or zero.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="originalLength"></param>
        /// <param name="elasticity"></param>
        /// <returns></returns>
        internal static IEnumerable<bool> Shrink(this IEnumerable<bool> values
            , int originalLength, Elasticity elasticity)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            var valuesCount = values.Count();

            // ReSharper disable once InvertIf
            // Allowing for the Contraction use case...
            if (elasticity.Contains(Elasticity.Contraction))
            {
                var width = 0;

                // ReSharper disable once PossibleMultipleEnumeration
                while (width < valuesCount && !values.ElementAt(valuesCount - width - 1))
                {
                    width++;
                }

                // ReSharper disable once PossibleMultipleEnumeration
                return values.Take(valuesCount - width);
            }

            // ReSharper disable once PossibleMultipleEnumeration
            return values;
        }
    }
}
