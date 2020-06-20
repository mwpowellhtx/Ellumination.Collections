using System;

namespace Ellumination.Collections.Keyed.Ordinals
{
    /// <summary>
    /// Provides a <see cref="Enumeration{TKey,T}"/> base class. Common specializations include
    /// <see cref="IntegerOrdinalEnumeration{T}"/> and <see cref="LongOrdinalEnumeration{T}"/>,
    /// for starters.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="T"></typeparam>
    public abstract partial class Enumeration<TKey, T> : Keyed.Enumeration<TKey, T>
        where T : Enumeration<TKey, T>
        where TKey : struct, IComparable<TKey>, IEquatable<TKey>
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
        /// Protected Constructor.
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
    }
}
