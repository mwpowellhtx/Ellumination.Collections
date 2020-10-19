using System.Collections.Generic;

namespace Ellumination.Collections.Generic
{
    /// <summary>
    /// Bidirectional List interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <inheritdoc />
    public interface IBidirectionalList<T> : IList<T>
    {
        /// <summary>
        /// Gets the Collection associated with the Bidirectional assets.
        /// </summary>
        IList<T> Collection { get; }

        /// <summary>
        /// Event raised prior to Adding an Item.
        /// </summary>
        event BidirectionalListItemCallback<T> AddingItem;

        /// <summary>
        /// Event raised after an Item has been Added.
        /// </summary>
        event BidirectionalListItemCallback<T> AddedItem;

        /// <summary>
        /// Event raised prior to Removing an Item.
        /// </summary>
        event BidirectionalListItemCallback<T> RemovingItem;

        /// <summary>
        /// Event raised after an Item has been Removed.
        /// </summary>
        event BidirectionalListItemCallback<T> RemovedItem;
    }
}
