using System;

namespace Ellumination.Collections.Keyed
{
    using static Enumeration.CompareToResults;

#pragma warning disable 659
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class Enumeration<TKey> : Enumeration
        , IComparable<TKey>, IEquatable<TKey>
        , IComparable<Enumeration<TKey>>, IEquatable<Enumeration<TKey>>
        where TKey : IComparable<TKey>, IEquatable<TKey>
#pragma warning restore 659
    {
        private TKey _key;

        /// <summary>
        /// Gets the Key associated with the <see cref="Enumeration{TKey}"/> instance.
        /// </summary>
        public virtual TKey Key
        {
            get => _key;
            protected set => _key = value;
        }

        /// <summary>
        /// Protected Default Constructor.
        /// </summary>
        protected Enumeration()
            : this(default(TKey), null, null)
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="name"></param>
        protected Enumeration(string name)
            : this(default(TKey), name, null)
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        protected Enumeration(string name, string displayName)
            : this(default(TKey), name, displayName)
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="key"></param>
        protected Enumeration(TKey key)
            : this(key, null, null)
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        protected Enumeration(TKey key, string name)
            : this(key, name, null)
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        protected Enumeration(TKey key, string name, string displayName)
            : base(name, displayName)
        {
            _key = key;
        }

        /// <summary>
        /// Returns the result of <see cref="IComparable{T}.CompareTo"/>
        /// involving <paramref name="a"/> and <paramref name="b"/> Operands.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected virtual int CompareTo(TKey a, TKey b)
            => a != null && b == null
                ? Gt
                : a == null && b != null
                    ? Lt
                    : a == null
                        ? Lt
                        : a.CompareTo(b);

        /// <inheritdoc />
        public virtual int CompareTo(TKey other) => CompareTo(Key, other);

        /// <inheritdoc />
        public bool Equals(TKey other) => CompareTo(Key, other) == Et;

        /// <inheritdoc />
        public int CompareTo(Enumeration<TKey> other) => CompareTo(Key, other.Key);

        /// <inheritdoc />
        public bool Equals(Enumeration<TKey> other) => CompareTo(other) == Et;

        /// <inheritdoc />
        public override int CompareTo(Enumeration other) => CompareTo(other as Enumeration<TKey>);

        /// <inheritdoc />
        public override bool Equals(Enumeration other) => CompareTo(other) == Et;

#pragma warning disable 659
        /// <inheritdoc />
        public override bool Equals(object other)
            => other is TKey key
                ? Equals(key)
                : Equals(other as Enumeration<TKey>);
#pragma warning restore 659

        // ReSharper disable once CommentTypo
        ///// <inheritdoc />
        //public override int GetHashCode() => throw new NotImplementedException();

        /// <summary>
        /// Implicitly converts the Enumerated <paramref name="value"/> to its underlying
        /// <see cref="Key"/> value.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator TKey(Enumeration<TKey> value) => value.Key;
    }
}
