using System;

namespace Ellumination.Collections.Generic
{
    // TODO: TBD: may want to implement IEnumerable? or possibly even ICollection... but not more than we need for Deque
    /// <summary>
    /// Provides a Strongly Typed Double Ended Queue interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <see cref="!:https://en.wikipedia.org/wiki/Double-ended_queue"/>
    public interface IDeque<T> : IEquatable<IDeque<T>>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        T Front();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        T Back();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool TryFront(out T item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool TryBack(out T item);

        /// <summary>
        /// Enqueue an <paramref name="item"/> on the front of the deque.
        /// </summary>
        /// <param name="item"></param>
        void EnqueueFront(T item);

        /// <summary>
        /// Enqueue an <paramref name="item"/> on the back of the deque.
        /// </summary>
        /// <param name="item"></param>
        void EnqueueBack(T item);

        /// <summary>
        /// Dequeue an item from the front of the deque.
        /// </summary>
        /// <returns></returns>
        T DequeueFront();

        /// <summary>
        /// Dequeue an item from the back of the deque.
        /// </summary>
        /// <returns></returns>
        T DequeueBack();

        /// <summary>
        /// Tries to Dequeue an <paramref name="item"/> from the front of the deque.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool TryDequeueFront(out T item);

        /// <summary>
        /// Tries to Dequeue an <paramref name="item"/> from the back of the deque.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool TryDequeueBack(out T item);

        /// <summary>
        /// Gets the count of items.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Returns whether contains the <paramref name="item"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Contains(T item);
    }
}
