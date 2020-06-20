using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Generic
{
    /// <summary>
    /// Set implementation using <see cref="ISet{T}"/>, based on
    /// <see cref="Dictionary{T,T}"/> as a basis for the set.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <see cref="!:http://powercollections.codeplex.com/discussions/33832"/>
    public class Set<T> : IDictionary<T, T>, ISet<T>
    {
        /// <summary>
        /// Dictionary backing field.
        /// </summary>
        private readonly IDictionary<T, T> _dictionary;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Set()
            : this(new List<T>())
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="values"></param>
        public Set(IEnumerable<T> values)
            : this(values.ToDictionary(x => x, x => x))
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dictionary"></param>
        public Set(IDictionary<T, T> dictionary)
        {
            _dictionary = dictionary;
        }

        private void SetAction(Action<IDictionary<T, T>> action)
        {
            action(_dictionary);
        }

        private TResult SetFunc<TResult>(Func<IDictionary<T, T>, TResult> func)
        {
            return func(_dictionary);
        }

        #region Set Members

        //TODO: look into SetAction and SetFunction helpers

        /// <inheritdoc />
        public bool Add(T item)
        {
            return SetFunc(x =>
            {
                if (x.ContainsKey(item))
                {
                    return false;
                }

                x[item] = item;
                return true;
            });
        }

        /// <inheritdoc />
        public void ExceptWith(IEnumerable<T> other)
        {
            foreach (var o in other)
            {
                var item = o;
                SetAction(x => x.Remove(item));
            }
        }

        /// <inheritdoc />
        public void IntersectWith(IEnumerable<T> other)
        {
            var excepted = SetFunc(x => x.Values.Except(other));
            ExceptWith(excepted);
        }

        // ReSharper disable PossibleMultipleEnumeration
        /// <inheritdoc />
        public bool IsProperSubsetOf(IEnumerable<T> other)
            => other.Count() < Count
               && other.All(o => SetFunc(x => x.ContainsKey(o)));
        // ReSharper restore PossibleMultipleEnumeration

        // ReSharper disable PossibleMultipleEnumeration
        /// <inheritdoc />
        public bool IsProperSupersetOf(IEnumerable<T> other)
            => other.Count() > Count
               && SetFunc(x => x.Values.All(other.Contains));
        // ReSharper disable restore PossibleMultipleEnumeration

        // ReSharper disable PossibleMultipleEnumeration
        /// <inheritdoc />
        public bool IsSubsetOf(IEnumerable<T> other)
            => other.Count() <= Count
               && other.All(o => SetFunc(x => x.ContainsKey(o)));
        // ReSharper restore PossibleMultipleEnumeration

        // ReSharper disable PossibleMultipleEnumeration
        /// <inheritdoc />
        public bool IsSupersetOf(IEnumerable<T> other)
            => other.Count() >= Count
               && SetFunc(x => x.Values.All(other.Contains));
        // ReSharper restore PossibleMultipleEnumeration

        /// <inheritdoc />
        public bool Overlaps(IEnumerable<T> other) => SetFunc(x => x.Values.Any(other.Contains));

        /// <inheritdoc />
        public bool SetEquals(IEnumerable<T> other) => SetFunc(x => x.Values.SequenceEqual(other));

        /// <inheritdoc />
        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            // TODO: TBD: probably better ways we can do this...
            // ReSharper disable PossibleMultipleEnumeration
            var excepted = SetFunc(x => x.Values.Except(other))
                .Concat(SetFunc(x => other.Except(other)));
            // ReSharper restore PossibleMultipleEnumeration

            Clear();

            foreach (var e in excepted)
            {
                Add(e);
            }
        }

        /// <inheritdoc />
        public void UnionWith(IEnumerable<T> other)
        {
            foreach (var o in other)
            {
                var added = Add(o);
            }
        }

        void ICollection<T>.Add(T item)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var added = Add(item);
        }

        // ReSharper disable once AssignNullToNotNullAttribute
        /// <inheritdoc />
        public bool Contains(T item) => SetFunc(x => x.ContainsKey(item));

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex) => SetAction(x => x.Values.CopyTo(array, arrayIndex));

        /// <inheritdoc cref="ISet{T}"/>
        public bool IsReadOnly => SetFunc(x => x.IsReadOnly);

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() => SetFunc(x => x.Values.GetEnumerator());

        #endregion

        #region Dictionary Members

        /// <inheritdoc />
        public void Add(T key, T value) => SetAction(x => x.Add(key, value));

        /// <inheritdoc />
        public bool ContainsKey(T key) => SetFunc(x => x.ContainsKey(key));

        /// <inheritdoc />
        public ICollection<T> Keys => SetFunc(x => x.Keys);

        // ReSharper disable once AssignNullToNotNullAttribute
        /// <inheritdoc cref="ISet{T}"/>
        public bool Remove(T key) => SetFunc(x => x.Remove(key));

        /// <inheritdoc />
        public bool TryGetValue(T key, out T value)
        {
            var v = value = default(T);
            if (!SetFunc(x => x.TryGetValue(key, out v)))
            {
                return false;
            }

            value = v;
            return true;
        }

        /// <inheritdoc />
        public ICollection<T> Values => SetFunc(x => x.Values);

        /// <inheritdoc />
        public T this[T key]
        {
            get => SetFunc(x => x[key]);
            set => SetAction(x => x[key] = value);
        }

        /// <inheritdoc />
        public void Add(KeyValuePair<T, T> item) => SetAction(x => x.Add(item));

        /// <inheritdoc cref="ISet{T}"/>
        public void Clear() => SetAction(x => x.Clear());

        /// <inheritdoc />
        public bool Contains(KeyValuePair<T, T> item) => SetFunc(x => x.Contains(item));

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<T, T>[] array, int arrayIndex) => SetAction(x => x.CopyTo(array, arrayIndex));

        /// <inheritdoc cref="ISet{T}"/>
        public int Count => SetFunc(x => x.Count);

        /// <inheritdoc />
        public bool Remove(KeyValuePair<T, T> item) => SetFunc(x => x.Remove(item));

        /// <inheritdoc />
        IEnumerator<KeyValuePair<T, T>> IEnumerable<KeyValuePair<T, T>>.GetEnumerator() => SetFunc(x => x.GetEnumerator());

        /// <summary>
        /// Gets the <see cref="IEnumerator"/> for the <see cref="IDictionary{T,T}.Values"/>.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() => SetFunc(x => x.Values.GetEnumerator());

        #endregion
    }
}
