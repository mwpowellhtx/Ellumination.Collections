using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Generic
{
    /// <summary>
    /// Provides helpful extension methods facilitating the Deque question.
    /// </summary>
    public static class DequeExtensionMethods
    {
        /// <summary>
        /// Indicates the FirstIndex.
        /// </summary>
        private const int FirstIndex = 0;

        /// <summary>
        /// Returns the LastIndex given <paramref name="list"/> <see cref="ICollection.Count"/>.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static int LastIndex<T>(this ICollection<T> list) => list.Count - 1;

        /// <summary>
        /// Returns the Item on the Front of the <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Front<T>(this IList<T> list) => list[FirstIndex];

        /// <summary>
        /// Returns the Item on the Back of the <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Back<T>(this IList<T> list) => list[list.LastIndex()];

        /// <summary>
        /// Tries to get the <paramref name="item"/> from the Front of the <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool TryFront<T>(this IList<T> list, out T item)
        {
            item = default(T);
            if (list.Count == 0)
            {
                return false;
            }

            item = list[FirstIndex];
            return true;
        }

        /// <summary>
        /// Tries to get the <paramref name="item"/> from the Back of the <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool TryBack<T>(this IList<T> list, out T item)
        {
            item = default(T);
            if (list.Count == 0)
            {
                return false;
            }

            item = list[list.LastIndex()];
            return true;
        }

        /// <summary>
        /// Enqueue the <paramref name="item"/> to the Front of the <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IList<T> EnqueueFront<T>(this IList<T> list, T item)
        {
            list.Insert(FirstIndex, item);
            return list;
        }

        /// <summary>
        /// Enqueue an <paramref name="item"/> to the Back of the <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IList<T> EnqueueBack<T>(this IList<T> list, T item)
        {
            list.Add(item);
            return list;
        }

        /// <summary>
        /// Tries to Dequeue an <paramref name="item"/> from the Front of the <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool TryDequeueFront<T>(this IList<T> list, out T item)
        {
            item = default(T);
            if (!list.Any())
            {
                return false;
            }

            item = list.First();
            list.RemoveAt(FirstIndex);
            return true;
        }

        /// <summary>
        /// Tries to Dequeue an <paramref name="item"/> from the Back of the <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool TryDequeueBack<T>(this IList<T> list, out T item)
        {
            item = default(T);
            if (!list.Any())
            {
                return false;
            }

            item = list.Last();
            list.RemoveAt(list.LastIndex());
            return true;
        }

        /// <summary>
        /// Dequeue from an Item the Front of the <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        /// <see cref="TryDequeueFront{T}"/>
        public static T DequeueFront<T>(this IList<T> list)
            => TryDequeueFront(list, out var item)
                ? item
                : throw new ArgumentOutOfRangeException(nameof(list), $"{nameof(list)} was empty.");

        /// <summary>
        /// Dequeue an Item from the Back of the <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        /// <see cref="TryDequeueBack{T}"/>
        public static T DequeueBack<T>(this IList<T> list)
            => TryDequeueBack(list, out var item)
                ? item
                : throw new ArgumentOutOfRangeException(nameof(list), $"{nameof(list)} was empty.");

        /// <summary>
        /// Returns the <paramref name="values"/> as an <see cref="Deque{T}"/>.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Deque<T> ToDeque<T>(this IEnumerable<T> values) => new Deque<T>(values.ToList());

        /// <summary>
        /// Enqueue the <paramref name="items"/> onto the Front of the <paramref name="deque"/>
        /// on a first come first serve <see cref="IDeque{T}.EnqueueFront"/> basis.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TDeque"></typeparam>
        /// <param name="deque"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static TDeque EnqueueFrontMany<T, TDeque>(this TDeque deque, params T[] items)
            where TDeque : IDeque<T>
        {
            foreach (var item in items)
            {
                deque.EnqueueFront(item);
            }

            return deque;
        }

        /// <summary>
        /// Enqueue the <paramref name="items"/> onto the Back of the <paramref name="deque"/>
        /// on a first come first serve <see cref="IDeque{T}.EnqueueBack"/> basis.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TDeque"></typeparam>
        /// <param name="deque"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static TDeque EnqueueBackMany<T, TDeque>(this TDeque deque, params T[] items)
            where TDeque : IDeque<T>
        {
            foreach (var item in items)
            {
                deque.EnqueueBack(item);
            }

            return deque;
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> of Dequeued Items from the Front of the
        /// <paramref name="deque"/>. The returned items will be in
        /// <see cref="IDeque{T}.DequeueFront"/> order and there may be as many as up to
        /// <paramref name="count"/> items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deque"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<T> DequeueFrontMany<T>(this IDeque<T> deque, int count = 1)
        {
            while (count-- > 0 && deque.Count > 0)
            {
                yield return deque.DequeueFront();
            }
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> of Dequeued Items from the Back of the
        /// <paramref name="deque"/>. The returned items will be in
        /// <see cref="IDeque{T}.DequeueBack"/> order and there may be as many as up to
        /// <paramref name="count"/> items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deque"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<T> DequeueBackMany<T>(this IDeque<T> deque, int count = 1)
        {
            while (count-- > 0 && deque.Count > 0)
            {
                yield return deque.DequeueBack();
            }
        }

        /// <summary>
        /// Tries to Dequeue as Many <typeparamref name="T"/> items as possible. May return with
        /// a <paramref name="result"/> that contains up to <paramref name="count"/> number of
        /// them. The returned <paramref name="result"/> will be in
        /// <see cref="IDeque{T}.DequeueFront"/> order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deque"></param>
        /// <param name="result"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool TryDequeueFrontMany<T>(this IDeque<T> deque, out IEnumerable<T> result, int count = 1)
        {
            result = new List<T>();

            while (count-- > 0 && deque.Count > 0)
            {
                ((IList<T>) result).Add(deque.DequeueFront());
            }

            return result.Any();
        }

        /// <summary>
        /// Tries to Dequeue as Many <typeparamref name="T"/> items as possible. May return with
        /// a <paramref name="result"/> that contains up to <paramref name="count"/> number of
        /// them. The returned <paramref name="result"/> will be in
        /// <see cref="IDeque{T}.DequeueBack"/> order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deque"></param>
        /// <param name="result"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool TryDequeueBackMany<T>(this IDeque<T> deque, out IEnumerable<T> result, int count = 1)
        {
            result = new List<T>();

            while (count-- > 0 && deque.Count > 0)
            {
                ((IList<T>) result).Add(deque.DequeueBack());
            }

            return result.Any();
        }
    }
}
