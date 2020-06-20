using System;
using System.Collections;
using System.Collections.Generic;

namespace Ellumination.Collections.Generic
{
    /// <summary>
    /// Provides the base <see cref="ICollection{T}"/> implementation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectionBase<T> : ICollection<T>
    {
        /// <summary>
        /// Collection backing field.
        /// </summary>
        private readonly ICollection<T> _collection;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="collection"></param>
        protected CollectionBase(ICollection<T> collection)
        {
            _collection = collection;
        }

        /// <summary>
        /// Collection functional helper method.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        private TResult CollectionFunction<TResult>(Func<ICollection<T>, TResult> func)
        {
            return func(_collection);
        }

        /// <summary>
        /// Collection action helper method.
        /// </summary>
        /// <param name="action"></param>
        private void CollectionAction(Action<ICollection<T>> action)
        {
            action(_collection);
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() => CollectionFunction(x => x.GetEnumerator());

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc cref="Add(T)" />
        public virtual void Add(T item) => CollectionAction(x => x.Add(item));

        /// <inheritdoc />
        void ICollection<T>.Add(T item) => Add(item);

        /// <inheritdoc cref="Clear" />
        public virtual void Clear() => CollectionAction(x => x.Clear());

        /// <inheritdoc />
        void ICollection<T>.Clear() => Clear();

        /// <inheritdoc />
        public virtual bool Contains(T item) => CollectionFunction(x => x.Contains(item));

        /// <inheritdoc />
        public virtual void CopyTo(T[] array, int arrayIndex) => CollectionAction(x => x.CopyTo(array, arrayIndex));

        /// <inheritdoc />
        public virtual bool Remove(T item) => CollectionFunction(x => x.Remove(item));

        /// <inheritdoc />
        public virtual int Count => CollectionFunction(x => x.Count);

        /// <inheritdoc />
        public virtual bool IsReadOnly => CollectionFunction(x => x.IsReadOnly);
    }
}
