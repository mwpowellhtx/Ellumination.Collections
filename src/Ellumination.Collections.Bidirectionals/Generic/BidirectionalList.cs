using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Generic
{
    // ReSharper disable once UnusedMember.Global
    /// <summary>
    /// Bidirectional List implementation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <inheritdoc />
    public class BidirectionalList<T> : IBidirectionalList<T>
    {
        /// <inheritdoc/>
        public IList<T> Collection { get; private set; }

        private delegate TResult ListFuncCallback<out TResult>(IList<T> collection);

        private TResult ListFunc<TResult>(ListFuncCallback<TResult> callback) => callback.Invoke(Collection);

        private delegate void ListActionCallback(IList<T> collection);

        private void ListAction(ListActionCallback callback) => callback(Collection);

        /// <summary>
        /// Gets a Default Item Callback.
        /// </summary>
        private static BidirectionalListItemCallback<T> DefaultItemCallback => _ => { };

        /// <inheritdoc />
        public event BidirectionalListItemCallback<T> AddingItem;

        /// <inheritdoc />
        public event BidirectionalListItemCallback<T> AddedItem;

        /// <inheritdoc />
        public event BidirectionalListItemCallback<T> RemovingItem;

        /// <inheritdoc />
        public event BidirectionalListItemCallback<T> RemovedItem;

        // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <inheritdoc />
        public BidirectionalList()
            : this(new List<T> { }, null, null)
        {
        }

        /// <summary>
        /// Values Constructor.
        /// </summary>
        /// <param name="values"></param>
        public BidirectionalList(IEnumerable<T> values)
            : this(values, null, null)
        {
        }

        /// <summary>
        /// Values Constructor.
        /// </summary>
        /// <param name="values"></param>
        public BidirectionalList(IList<T> values)
            : this(values, null, null)
        {
        }

        // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
        /// <summary>
        /// Construct a <see cref="IBidirectionalList{T}"/> given the set of
        /// <see cref="BidirectionalListItemCallback{T}"/>.
        /// </summary>
        /// <param name="onAdded"></param>
        /// <param name="onRemoved"></param>
        /// <param name="onAdding"></param>
        /// <param name="onRemoving"></param>
        /// <inheritdoc />
        public BidirectionalList(
            BidirectionalListItemCallback<T> onAdded
            , BidirectionalListItemCallback<T> onRemoved
            , BidirectionalListItemCallback<T> onAdding = null
            , BidirectionalListItemCallback<T> onRemoving = null)
            : this(new List<T> { }, onAdded, onRemoved, onAdding, onRemoving)
        {
        }

        /// <summary>
        /// Constructs a <see cref="IBidirectionalList{T}"/> given <paramref name="values"/>.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="onAdded"></param>
        /// <param name="onRemoved"></param>
        /// <param name="onAdding"></param>
        /// <param name="onRemoving"></param>
        /// <inheritdoc />
        public BidirectionalList(IEnumerable<T> values
            , BidirectionalListItemCallback<T> onAdded
            , BidirectionalListItemCallback<T> onRemoved
            , BidirectionalListItemCallback<T> onAdding = null
            , BidirectionalListItemCallback<T> onRemoving = null)
            : this(values.ToList(), onAdded, onRemoved, onAdding, onRemoving)
        {
        }

        /// <summary>
        /// Constructs a <see cref="IBidirectionalList{T}"/> given <paramref name="values"/>.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="onAdded"></param>
        /// <param name="onRemoved"></param>
        /// <param name="onAdding"></param>
        /// <param name="onRemoving"></param>
        public BidirectionalList(IList<T> values
            , BidirectionalListItemCallback<T> onAdded
            , BidirectionalListItemCallback<T> onRemoved
            , BidirectionalListItemCallback<T> onAdding = null
            , BidirectionalListItemCallback<T> onRemoving = null)
        {
            var defaultItemCallback = DefaultItemCallback;

            // Connect the Bidirectional Item callback conditionally when there is a Callback.
            void ConnectBidiCallback(BidirectionalListItemCallback<T> callback
                , Action<BidirectionalList<T>, BidirectionalListItemCallback<T>> onConnect)
            {
                if (callback != null)
                {
                    onConnect.Invoke(this, callback);
                }
            }

            ConnectBidiCallback(onAdded, (x, callback) => x.AddedItem += callback);
            ConnectBidiCallback(onAdding, (x, callback) => x.AddingItem += callback);
            ConnectBidiCallback(onRemoved, (x, callback) => x.RemovedItem += callback);
            ConnectBidiCallback(onRemoving, (x, callback) => x.RemovingItem += callback);

            // Effectively we are also Adding the items to the initial Collection instance.
            foreach (var x in values)
            {
                AddingItem?.Invoke(x);
            }

            Collection = values;

            foreach (var x in values)
            {
                AddedItem?.Invoke(x);
            }
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() => ListFunc(x => x.GetEnumerator());

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        public void Add(T item)
        {
            AddingItem?.Invoke(item);
            ListAction(x => x.Add(item));
            AddedItem?.Invoke(item);
        }

        /// <summary>
        /// Clears the Bidirectional List of its Items. This operation is a bit expensive
        /// because a local collection must be maintained in order to properly connect the
        /// callback methods.
        /// </summary>
        /// <inheritdoc />
        public void Clear()
        {
            // We need to maintain a Local Collection for this one.
            var clearing = new List<T>(Collection);

            void ReportClearing(BidirectionalListItemCallback<T> callback)
            {
                foreach (var x in clearing)
                {
                    callback.Invoke(x);
                }
            }

            ReportClearing(x => RemovingItem?.Invoke(x));
            ListAction(x => x.Clear());
            ReportClearing(x => RemovedItem?.Invoke(x));
        }

        /// <inheritdoc />
        public bool Contains(T item) => ListFunc(x => x.Contains(item));

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex) => ListAction(x => x.CopyTo(array, arrayIndex));

        /// <inheritdoc />
        public bool Remove(T item)
        {
            RemovingItem?.Invoke(item);
            var removed = ListFunc(x => x.Remove(item));
            RemovedItem?.Invoke(item);
            return removed;
        }

        /// <inheritdoc />
        public int Count => ListFunc(x => x.Count);

        /// <inheritdoc />
        public bool IsReadOnly => ListFunc(x => x.IsReadOnly);

        /// <inheritdoc />
        public int IndexOf(T item) => ListFunc(x => x.IndexOf(item));

        /// <inheritdoc />
        public void Insert(int index, T item)
        {
            AddingItem?.Invoke(item);
            ListAction(x => x.Insert(index, item));
            AddedItem?.Invoke(item);
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            var item = this[index];
            RemovingItem?.Invoke(item);
            ListAction(x => x.RemoveAt(index));
            RemovedItem?.Invoke(item);
        }

        /// <inheritdoc />
        public T this[int index]
        {
            get => ListFunc(x => x[index]);
            set
            {
                var item = this[index];
                AddingItem?.Invoke(value);
                RemovingItem?.Invoke(item);
                ListAction(x => x[index] = value);
                RemovedItem?.Invoke(item);
                AddedItem?.Invoke(value);
            }
        }
    }
}
