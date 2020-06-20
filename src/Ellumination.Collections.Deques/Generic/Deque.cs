using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Generic
{
    /// <inheritdoc />
    public class Deque<T> : IDeque<T>
    {
        /// <summary>
        /// Gets the Comparer involved during the Deque life cycle.
        /// </summary>
        /// <see cref="EqualityComparer{T}.Default"/>
        private IEqualityComparer<T> Comparer { get; } = EqualityComparer<T>.Default;

        /// <summary>
        /// Gets the InternalList.
        /// </summary>
        internal IList<T> InternalList { get; }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public Deque()
            : this(new List<T>())
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="list"></param>
        public Deque(IList<T> list)
        {
            InternalList = list;
        }

        /// <summary>
        /// Public Constructor.
        /// </summary>
        /// <param name="comparer"></param>
        public Deque(IEqualityComparer<T> comparer)
            : this(new List<T>())
        {
            Comparer = comparer;
        }

        /// <summary>
        /// Public Constructor.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="comparer"></param>
        public Deque(IList<T> list, IEqualityComparer<T> comparer)
        {
            InternalList = list;
            Comparer = comparer;
        }

        private void DequeAction(Action<IList<T>> action) => action.Invoke(InternalList);

        private TResult DequeFunction<TResult>(Func<IList<T>, TResult> func) => func.Invoke(InternalList);

        /// <inheritdoc />
        public T Front() => DequeFunction(x => x.Front());

        /// <inheritdoc />
        public T Back() => DequeFunction(x => x.Back());

        /// <inheritdoc />
        public void EnqueueFront(T item) => DequeAction(x => x.EnqueueFront(item));

        /// <inheritdoc />
        public void EnqueueBack(T item) => DequeAction(x => x.EnqueueBack(item));

        /// <inheritdoc />
        public T DequeueFront() => DequeFunction(x => x.DequeueFront());

        /// <inheritdoc />
        public T DequeueBack() => DequeFunction(x => x.DequeueBack());

        /// <inheritdoc />
        public int Count => DequeFunction(x => x.Count);

        /// <inheritdoc />
        public bool Contains(T item) => DequeFunction(x => x.Contains(item));

        /// <inheritdoc />
        public bool TryFront(out T item) => InternalList.TryFront(out item);

        /// <inheritdoc />
        public bool TryBack(out T item) => InternalList.TryBack(out item);

        /// <inheritdoc />
        public bool TryDequeueFront(out T item) => InternalList.TryDequeueFront(out item);

        /// <inheritdoc />
        public bool TryDequeueBack(out T item) => InternalList.TryDequeueBack(out item);

        private bool Equals(Deque<T> a, Deque<T> b)
            => ReferenceEquals(a, b)
               || (a.Count == b.Count
                   && a.InternalList.Zip(b.InternalList, (x, y) => Comparer?.Equals(x, y)).All(z => z == true));

        /// <summary>
        /// Returns whether this Deque Equals the <paramref name="other"/> Deque.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IDeque<T> other) => Equals(this, (Deque<T>) other);
    }
}
