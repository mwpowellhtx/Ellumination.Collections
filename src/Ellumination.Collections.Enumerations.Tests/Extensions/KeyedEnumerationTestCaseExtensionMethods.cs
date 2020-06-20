using System;
using System.Linq;

namespace Ellumination.Collections
{
    using Xunit;
    using Xunit.Abstractions;
    using Xunit.Sdk;
    using static String;

    /// <summary>
    /// Provides a set of extension methods supporting
    /// <see cref="Keyed.KeyedEnumerationTestFixtureBase{TKey,T}"/> tests. Serves for both
    /// Ordinal as well as <see cref="Keyed.Flags.Enumeration{T}"/> based test cases.
    /// </summary>
    public static class KeyedEnumerationTestCaseExtensionMethods
    {
        /// <summary>
        /// Returns the <typeparamref name="T"/> Value corresponding to the
        /// <paramref name="key"/>. <paramref name="_"/> is provided in order to
        /// connect the caller with the <see cref="Keyed.Enumeration{TKey, T}"/>
        /// <typeparamref name="T"/> only, nothing more, nothing less.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        /// <param name="key"></param>
        /// <param name="verify"></param>
        /// <returns></returns>
        public static T GetValueByKey<TKey, T>(this T _, TKey key, Action<T> verify = null)
            where T : Keyed.Enumeration<TKey, T>
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            var value = Keyed.Enumeration<TKey, T>.FromKey(key).AssertNotNull();
            verify?.Invoke(value.AssertEqual(key, x => x.Key));
            return value;
        }

        /// <summary>
        /// <typeparamref name="T"/> <see cref="Keyed.Flags.Enumeration{T}"/> values Shall All
        /// have Consistent <see cref="Keyed.Enumeration{TKey}.Key"/> lengths. Rules out false
        /// positives in the form of empty <see cref="Enumeration.GetValues{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreNulls"></param>
        /// <param name="reporter"></param>
        internal static void ShallAllHaveConsistentBitLengths<T>(this T value, bool ignoreNulls = true
            , IEnumerationCoverageReporter<T> reporter = null)
            where T : Keyed.Flags.Enumeration<T>
        {
            var values = Enumeration.GetValues<T>(ignoreNulls).AssertNotNull().AssertNotEmpty();

            // ReSharper disable once UnusedVariable
            var distinct = values.Select(x =>
            {
                Assert.NotNull(x);

                /* There must be some Bits for this to work...
                 * Yes, while we could Assert NotEmpty here, I am trying to keep the thrown
                 * Exceptions as distinct as possible, not least of which for unit test purposes. */

                var length = x.Key.AssertNotNull().AssertNotEmpty().Length;
                reporter?.Report(x.Name);
                return length;
            }).Distinct().AssertEqual(1, x => x.Count());
        }

        /// <summary>
        /// <paramref name="value"/> is used to connect the caller with the underlying
        /// <see cref="Keyed.Enumeration{TKey,T}"/> type only. Additionally, we are making
        /// the assumption that <see cref="Enumeration.GetValues{T}"/> are unique not only
        /// in <see cref="Enumeration.Name"/>, but also in
        /// <see cref="Keyed.Enumeration{TKey}.Key"/> value. In truth, this could go either
        /// way, but for now I will expect that there must be uniqueness throughout.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="reporter"></param>
        /// <param name="outputHelper"></param>
        /// <see cref="!:http://en.wikipedia.org/wiki/Enumeration"/>
        public static void KeysShallBeUniquelyAssigned<TKey, T>(this T value
            , IEnumerationCoverageReporter<T> reporter = null
            , ITestOutputHelper outputHelper = null)
            where T : Keyed.Enumeration<TKey, T>
            where TKey : IComparable<TKey>, IEquatable<TKey>
            => value.KeysShallBeUniquelyAssigned<TKey, TKey, T>(x => x, reporter, outputHelper);

        /// <summary>
        /// <paramref name="value"/> is used to connect the caller with the underlying
        /// <see cref="Keyed.Enumeration{TKey,T}"/> type only. Additionally, we are making
        /// the assumption that <see cref="Enumeration.GetValues{T}"/> are unique not only
        /// in <see cref="Enumeration.Name"/>, but also in
        /// <see cref="Keyed.Enumeration{TKey}.Key"/> value. In truth, this could go either
        /// way, but for now I will expect that there must be uniqueness throughout.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TGroupByKey"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="groupByKey"></param>
        /// <param name="reporter"></param>
        /// <param name="outputHelper"></param>
        /// <see cref="!:http://en.wikipedia.org/wiki/Enumeration"/>
        public static void KeysShallBeUniquelyAssigned<TKey, TGroupByKey, T>(this T value
            , Func<TKey, TGroupByKey> groupByKey, IEnumerationCoverageReporter<T> reporter = null
            , ITestOutputHelper outputHelper = null)
            where T : Keyed.Enumeration<TKey, T>
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            var values = Enumeration.GetValues<T>().Select(x =>
            {
                var result = x;
                reporter?.Report(x.Name);
                return result;
            }).ToList();

            // This step is key, must all be Uniquely Assigned, that is, in Groups of One.
            //var grouped = values.GroupBy(x => x.Key).ToArray();
            var grouped = values.GroupBy(x => groupByKey.Invoke(x.Key)).ToArray();

            try
            {
                values.AssertEqual(grouped.Length, x => x.Count);
            }
            catch (EqualException)
            {
                string ListDupes() => Join(", ", grouped.Where(g => g.Count() > 1).Select(g => $"`{g.Key}´"));
                outputHelper?.WriteLine($"Some Enumerated values have Duplicate Bits: {ListDupes()}");
                throw;
            }
        }
    }
}
