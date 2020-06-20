using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    // TODO: TBD: at present this assumes "FIFO" order and we do not sort or prioritize the queue in any way, shape, or form.
    /// <summary>
    /// Provides the set of Queue operations on <see cref="IList{T}"/>.
    /// </summary>
    /// <see cref="!:http://en.wikipedia.org/wiki/Queue_(abstract_data_type)"/>
    public static class QueueExtensionMethods
    {
        /// <summary>
        /// Enqueues the <paramref name="item"/> onto the Front of the <paramref name="list"/>.
        /// May Enqueue <paramref name="additionalItems"/> onto the Front of the list in Enqueue
        /// order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TList"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <param name="additionalItems"></param>
        /// <returns></returns>
        public static TList Enqueue<T, TList>(this TList list, T item, params T[] additionalItems)
            where TList : IList<T>
        {
            list.Insert(0, item);

            foreach (var additionalItem in additionalItems)
            {
                list.Enqueue(additionalItem);
            }

            return list;
        }

        /// <summary>
        /// Returns the Dequeued Item from the Back of the List.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TList"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T Dequeue<T, TList>(this TList list)
            where TList : IList<T>
        {
            var index = list.Count - 1;
            var item = list[index];
            list.RemoveAt(index);
            return item;
        }

        /// <summary>
        /// Tries to Dequeue the <paramref name="item"/> from the Back of the
        /// <paramref name="list"/>. Returns whether this operation was successful.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TList"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool TryDequeue<T, TList>(this TList list, out T item)
            where TList : IList<T>
        {
            var count = list.Count;
            item = count > 0 ? list.Dequeue<T, TList>() : default(T);
            return list.Count != count;
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> of Dequeued items from the Back of the
        /// <paramref name="list"/>. The returned items will be in <see cref="Dequeue{T,TList}"/>
        /// order and there may be as many as up to <paramref name="count"/> items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TList"></typeparam>
        /// <param name="list"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<T> DequeueMany<T, TList>(this TList list, int count = 1)
            where TList : IList<T>
        {
            while (count-- > 0 && list.Any())
            {
                yield return list.Dequeue<T, TList>();
            }
        }

        /// <summary>
        /// Tries to Dequeue as Many <typeparamref name="T"/> items as possible. May return with
        /// a <paramref name="result"/> that contains up to <paramref name="count"/> number of
        /// them. The returned <paramref name="result"/> will be in <see cref="Dequeue{T,TList}"/>
        /// order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TList"></typeparam>
        /// <param name="list"></param>
        /// <param name="result"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool TryDequeueMany<T, TList>(this TList list, out IEnumerable<T> result, int count = 1)
            where TList : IList<T>
        {
            result = new List<T>();

            while (count-- > 0 && list.Any())
            {
                ((IList<T>) result).Add(list.Dequeue<T, TList>());
            }

            return result.Any();
        }
    }
}
