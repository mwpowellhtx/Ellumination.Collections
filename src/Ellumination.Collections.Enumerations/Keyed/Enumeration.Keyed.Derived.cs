using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Keyed
{
    using static Enumeration.CompareToResults;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey">The Key used in order to uniquely identify each Enumerated Value.</typeparam>
    /// <typeparam name="T">Represents the Derived Enumeration Type.</typeparam>
    public abstract class Enumeration<TKey, T> : Enumeration<TKey>
        where T : Enumeration<TKey, T>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        /// <summary>
        /// Protected Default Constructor.
        /// </summary>
        protected Enumeration()
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="name"></param>
        protected Enumeration(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        protected Enumeration(string name, string displayName)
            : base(name, displayName)
        {
        }

        /// <summary>
        /// Protected Default Constructor.
        /// </summary>
        /// <param name="key"></param>
        protected Enumeration(TKey key)
            : base(key)
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        protected Enumeration(TKey key, string name)
            : base(key, name)
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        protected Enumeration(TKey key, string name, string displayName)
            : base(key, name, displayName)
        {
        }

        /// <inheritdoc />
        protected override string ResolveName()
        {
            // TODO: TBD: may also want to restrict the field type(s) to those of the declaring type
            var field = GetDeclaringTypes(typeof(T))
                .SelectMany(type => type.GetFields(PublicStaticDeclaredOnly))
                .FirstOrDefault(fieldInfo => ReferenceEquals(this, fieldInfo.GetValue(null)));

            return field?.Name ?? string.Empty;
        }

        /// <summary>
        /// <see cref="Values"/> backing field.
        /// </summary>
        private static IEnumerable<T> _values;

        /// <summary>
        /// Gets the enumerated Values from the <typeparamref name="T"/> type.
        /// </summary>
        /// <see cref="Enumeration.GetValues{T}"/>
        public static IEnumerable<T> Values => _values ?? (_values = GetValues<T>().ToArray());

        /// <summary>
        /// Unpacks the current Enumerated value, if possible, and returns the unpacked instance
        /// contributing to the composite Enumerated value.
        /// </summary>
        public abstract IEnumerable<T> EnumeratedValues { get; }

        /// <summary>
        /// Returns the <typeparamref name="T"/> Value corresponding to the <paramref name="key"/>.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T FromKey(TKey key) => GetValues<T>().SingleOrDefault(x => x.Key.CompareTo(key) == Et);

        /// <summary>
        /// <paramref name="key"/> Increment Callback delegate.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected delegate TKey IncrementKeyCallback(TKey key);

        /// <summary>
        /// Initializes the Keyed <paramref name="values"/> starting from the <paramref name="start"/> <typeparamref name="TKey"/>. Increments the Key by invoking the <paramref name="increment"/> callback between iterations.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="start"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        protected static IEnumerable<T> InitializeValueKeys(IEnumerable<T> values, TKey start, IncrementKeyCallback increment)
        {
            // ReSharper disable PossibleMultipleEnumeration
            var _ = values.Aggregate(start, (key, value) =>
            {
                value.Key = key;
                return increment.Invoke(key);
            });

            return values;
            // ReSharper restore PossibleMultipleEnumeration
        }
    }
}
