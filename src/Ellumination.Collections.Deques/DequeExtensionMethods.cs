using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    /// <summary>
    /// Provides helpful extension methods facilitating the Deque question.
    /// </summary>
    public static class DequeExtensionMethods
    {
        /// <summary>
        /// Returns the <paramref name="values"/> as an <see cref="Deque"/>.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Deque ToDeque(this IEnumerable values) => new Deque(values);

        /// <summary>
        /// Enqueue the <paramref name="items"/> onto the Front of the <paramref name="deque"/>
        /// on a first come first serve <see cref="Generic.IDeque{T}.EnqueueFront"/> basis.
        /// </summary>
        /// <typeparam name="TDeque"></typeparam>
        /// <param name="deque"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static TDeque EnqueueFrontMany<TDeque>(this TDeque deque, params object[] items)
            where TDeque : IDeque
        {
            foreach (var item in items)
            {
                deque.EnqueueFront(item);
            }

            return deque;
        }

        /// <summary>
        /// Enqueue the <paramref name="items"/> onto the Back of the <paramref name="deque"/>
        /// on a first come first serve <see cref="Generic.IDeque{T}.EnqueueBack"/> basis.
        /// </summary>
        /// <typeparam name="TDeque"></typeparam>
        /// <param name="deque"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static TDeque EnqueueBackMany<TDeque>(this TDeque deque, params object[] items)
            where TDeque : IDeque
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
        /// <see cref="Generic.IDeque{T}.DequeueFront"/> order and there may be as many as
        /// up to <paramref name="count"/> items.
        /// </summary>
        /// <param name="deque"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<object> DequeueFrontMany(this IDeque deque, int count = 1)
        {
            while (count-- > 0 && deque.Count > 0)
            {
                yield return deque.DequeueFront();
            }
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> of Dequeued Items from the Back of the
        /// <paramref name="deque"/>. The returned items will be in
        /// <see cref="Generic.IDeque{T}.DequeueBack"/> order and there may be as many as
        /// up to <paramref name="count"/> items.
        /// </summary>
        /// <param name="deque"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<object> DequeueBackMany(this IDeque deque, int count = 1)
        {
            while (count-- > 0 && deque.Count > 0)
            {
                yield return deque.DequeueBack();
            }
        }

        /// <summary>
        /// Tries to Dequeue as Many <see cref="object"/> Items as possible. May return with
        /// a <paramref name="result"/> that contains up to <paramref name="count"/> number of
        /// them. The returned <paramref name="result"/> will be in
        /// <see cref="Generic.IDeque{T}.DequeueFront"/> order.
        /// </summary>
        /// <param name="deque"></param>
        /// <param name="result"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool TryDequeueFrontMany(this IDeque deque, out IEnumerable<object> result, int count = 1)
        {
            result = new List<object>();

            while (count-- > 0 && deque.Count > 0)
            {
                ((IList<object>) result).Add(deque.DequeueFront());
            }

            return result.Any();
        }

        /// <summary>
        /// Tries to Dequeue as Many <see cref="object"/> Items as possible. May return with
        /// a <paramref name="result"/> that contains up to <paramref name="count"/> number of
        /// them. The returned <paramref name="result"/> will be in
        /// <see cref="Generic.IDeque{T}.DequeueBack"/> order.
        /// </summary>
        /// <param name="deque"></param>
        /// <param name="result"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool TryDequeueBackMany(this IDeque deque, out IEnumerable<object> result, int count = 1)
        {
            result = new List<object>();

            while (count-- > 0 && deque.Count > 0)
            {
                ((IList<object>) result).Add(deque.DequeueBack());
            }

            return result.Any();
        }
    }
}
