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
        private readonly IList<T> _collection;

        private delegate TResult ListFuncCallback<out TResult>(IList<T> collection);

        private TResult ListFunc<TResult>(ListFuncCallback<TResult> callback) => callback.Invoke(_collection);

        private delegate void ListActionCallback(IList<T> collection);

        private void ListAction(ListActionCallback callback) => callback(_collection);

        private static BidirectionalListItemCallback<T> DefaultCallback() => _ => { };

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
            : this(new List<T> { })
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
        public BidirectionalList(BidirectionalListItemCallback<T> onAdded
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
            , BidirectionalListItemCallback<T> onAdded = null
            , BidirectionalListItemCallback<T> onRemoved = null
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
            , BidirectionalListItemCallback<T> onAdded = null
            , BidirectionalListItemCallback<T> onRemoved = null
            , BidirectionalListItemCallback<T> onAdding = null
            , BidirectionalListItemCallback<T> onRemoving = null)
        {
            AddedItem += onAdded ?? DefaultCallback();
            AddingItem += onAdding ?? DefaultCallback();
            RemovedItem += onRemoved ?? DefaultCallback();
            RemovingItem += onRemoving ?? DefaultCallback();

            // Effectively we are also Adding the items to the initial Collection instance.
            foreach (var x in values)
            {
                AddingItem?.Invoke(x);
            }

            _collection = values;

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
            var clearing = new List<T>(_collection);

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
