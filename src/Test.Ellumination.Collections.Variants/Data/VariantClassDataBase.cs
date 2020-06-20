using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Variants
{
    using static Collections;

    internal abstract class VariantClassDataBase : ClassDataBase
    {
        /// <summary>
        /// false
        /// </summary>
        protected const bool VariantDoesNotEqualResult = false;

        /// <summary>
        /// Establishes the <see cref="IEquatable{T}"/> Expected Result in a consistent manner.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected static bool DoesVariantEqual(IVariant<int> a, IVariant<int> b)
            => ReferenceEquals(a, b) || a.Value == b.Value;

        /// <summary>
        /// -1
        /// </summary>
        protected const int VariantDoesNotCompareResult = -1;

        /// <summary>
        /// Establishes the <see cref="IComparable{T}"/> Expected Result in a consistent manner.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected static int VariantCompareTo(IVariant<int> a, IVariant<int> b)
            => ReferenceEquals(a, b) ? 0 : a.Value.CompareTo(b.Value);

        private static IVariantConfigurationCollection _configuration;

        private static IVariantConfigurationCollection Configuration
            => _configuration ?? (_configuration = VariantConfigurationCollection.Create(
                       VariantConfiguration.Configure<int>(
                           (x, y) => (int) x == (int) y
                           , (x, y) => ((int) x).CompareTo((int) y))
                       , VariantConfiguration.Configure<bool>(
                           (x, y) => (bool) x == (bool) y
                           , (x, y) => ((bool) x).CompareTo((bool) y))
                   )
               );

        private const int DefaultInteger = default(int);

        private static IEnumerable<int> _integerValues;

        private static IEnumerable<int> _integerGreaterThan;

        //// TODO: TBD: enumerate a handful of contributing combinatoric elements here.
        protected static IEnumerable<int> IntegerValues
            => _integerValues ?? (_integerValues
                   = GetRange(DefaultInteger, DefaultInteger + 1, DefaultInteger + 2)
               );

        protected static IEnumerable<int> IntegerGreaterThan
            => _integerGreaterThan ?? (_integerGreaterThan
                   = IntegerValues.Select(x => x + 1).ToArray()
               );

        private const bool DefaultBool = default(bool);

        private static IEnumerable<bool> _boolValues;

        protected static IEnumerable<bool> BoolValues
            => _boolValues ?? (_boolValues
                   = GetRange(DefaultBool, !DefaultBool, DefaultBool)
               );

        protected static IEnumerable<Variant<T>> GetVariantValues<T>(IEnumerable<T> values)
            => values.Select(x => Variant.Create(x, Configuration));
    }
}
