using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    /// <summary>
    /// Provides the set of Stack operations on <see cref="IList{T}"/>.
    /// </summary>
    /// <see cref="!:http://en.wikipedia.org/wiki/Stack_(abstract_data_type)"/>
    public static class StackExtensionMethods
    {
        /// <summary>
        /// Pushes the <paramref name="item"/> onto the Front of the <paramref name="list"/>.
        /// May Push <paramref name="additionalItems"/> onto the Front of the list in Push order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TList"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <param name="additionalItems"></param>
        /// <returns></returns>
        public static TList Push<T, TList>(this TList list, T item, params T[] additionalItems)
            where TList : IList<T>
        {
            list.Insert(0, item);

            foreach (var additionalItem in additionalItems)
            {
                list.Push(additionalItem);
            }

            return list;
        }

        /// <summary>
        /// Returns the Popped Item from the Front of the List.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TList"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T Pop<T, TList>(this TList list)
            where TList : IList<T>
        {
            var item = list[0];
            list.RemoveAt(0);
            return item;
        }

        /// <summary>
        /// Tries to Pop the <paramref name="item"/> from the Front of the
        /// <paramref name="list"/>. Returns whether this operation was successful.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TList"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool TryPop<T, TList>(this TList list, out T item)
            where TList : IList<T>
        {
            var count = list.Count;
            item = count > 0 ? list.Pop<T, TList>() : default(T);
            return count != list.Count;
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> of Popped items from the Front of the
        /// <paramref name="list"/>. The returned items will be in <see cref="Pop{T,TList}"/>
        /// order and there may be as many as up to <paramref name="count"/> items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TList"></typeparam>
        /// <param name="list"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<T> PopMany<T, TList>(this TList list, int count = 1)
            where TList : IList<T>
        {
            while (count-- > 0 && list.Any())
            {
                yield return list.Pop<T, TList>();
            }
        }

        /// <summary>
        /// Tries to Pop as Many <typeparamref name="T"/> items as possible. May return with a
        /// <paramref name="result"/> that contains up to <paramref name="count"/> number of them.
        /// The returned <paramref name="result"/> will be in <see cref="Pop{T,TList}"/> order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TList"></typeparam>
        /// <param name="list"></param>
        /// <param name="result"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool TryPopMany<T, TList>(this TList list, out IEnumerable<T> result, int count = 1)
            where TList : IList<T>
        {
            result = new List<T>();

            while (count-- > 0 && list.Any())
            {
                ((IList<T>) result).Add(list.Pop<T, TList>());
            }

            return result.Any();
        }
    }
}
