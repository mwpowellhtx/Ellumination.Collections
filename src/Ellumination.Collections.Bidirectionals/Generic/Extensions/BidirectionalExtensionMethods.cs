using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Generic
{
    /// <summary>
    /// Provides several helpful extension methods akin to the
    /// <see cref="Enumerable.ToList{TSource}"/> extension methods.
    /// </summary>
    public static class BidirectionalExtensionMethods
    {
        /// <summary>
        /// Returns a <see cref="BidirectionalList{T}"/> corresponding to the
        /// <paramref name="values"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IBidirectionalList<T> ToBidirectionalList<T>(this IEnumerable<T> values)
            => new BidirectionalList<T>(values);

        /// <summary>
        /// Returns a <see cref="BidirectionalList{T}"/> corresponding to the
        /// <paramref name="values"/>. Provides <paramref name="onAdded"/> and
        /// <paramref name="onRemoved"/> callbacks.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="onAdded"></param>
        /// <param name="onRemoved"></param>
        /// <returns></returns>
        public static IBidirectionalList<T> ToBidirectionalList<T>(this IEnumerable<T> values
            , BidirectionalListItemCallback<T> onAdded, BidirectionalListItemCallback<T> onRemoved)
            => new BidirectionalList<T>(values, onAdded, onRemoved);

        /// <summary>
        /// Returns a <see cref="BidirectionalList{T}"/> corresponding to the
        /// <paramref name="values"/>. Provides <paramref name="onAdded"/>,
        /// <paramref name="onRemoved"/>, <paramref name="onAdding"/>, and
        /// <paramref name="onRemoving"/> callbacks.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="onAdded"></param>
        /// <param name="onRemoved"></param>
        /// <param name="onAdding"></param>
        /// <param name="onRemoving"></param>
        /// <returns></returns>
        public static IBidirectionalList<T> ToBidirectionalList<T>(this IEnumerable<T> values
            , BidirectionalListItemCallback<T> onAdded, BidirectionalListItemCallback<T> onRemoved
            , BidirectionalListItemCallback<T> onAdding, BidirectionalListItemCallback<T> onRemoving)
            => new BidirectionalList<T>(values, onAdded, onRemoved, onAdding, onRemoving);

        // TODO: ToBidirectionalList extensions should allow for IList as well
        // TODO: https://github.com/mwpowellhtx/Ellumination.Collections/issues/1
        /// <summary>
        /// Returns a <see cref="BidirectionalList{T}"/> corresponding to the
        /// <paramref name="values"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IBidirectionalList<T> ToBidirectionalList<T>(this IList<T> values)
            => new BidirectionalList<T>(values);

        /// <summary>
        /// Returns a <see cref="BidirectionalList{T}"/> corresponding to the
        /// <paramref name="values"/>. Provides <paramref name="onAdded"/> and
        /// <paramref name="onRemoved"/> callbacks.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="onAdded"></param>
        /// <param name="onRemoved"></param>
        /// <returns></returns>
        public static IBidirectionalList<T> ToBidirectionalList<T>(this IList<T> values
            , BidirectionalListItemCallback<T> onAdded, BidirectionalListItemCallback<T> onRemoved)
            => new BidirectionalList<T>(values, onAdded, onRemoved);

        /// <summary>
        /// Returns a <see cref="BidirectionalList{T}"/> corresponding to the
        /// <paramref name="values"/>. Provides <paramref name="onAdded"/>,
        /// <paramref name="onRemoved"/>, <paramref name="onAdding"/>, and
        /// <paramref name="onRemoving"/> callbacks.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="onAdded"></param>
        /// <param name="onRemoved"></param>
        /// <param name="onAdding"></param>
        /// <param name="onRemoving"></param>
        /// <returns></returns>
        public static IBidirectionalList<T> ToBidirectionalList<T>(this IList<T> values
            , BidirectionalListItemCallback<T> onAdded, BidirectionalListItemCallback<T> onRemoved
            , BidirectionalListItemCallback<T> onAdding, BidirectionalListItemCallback<T> onRemoving)
            => new BidirectionalList<T>(values, onAdded, onRemoved, onAdding, onRemoving);
    }
}
