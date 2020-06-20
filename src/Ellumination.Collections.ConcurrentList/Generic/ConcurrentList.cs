using System.Collections;
using System.Collections.Generic;

namespace Ellumination.Collections.Generic
{
    // TODO: TBD: potentially refactor away from "Football" per se, has better use in a general library
    /// <inheritdoc cref="CollectionBase{T}" />
    /// <see cref="!:http://stackoverflow.com/questions/6601611/no-concurrentlistt-in-net-4-0"/>
    /// <see cref="!:http://msdn.microsoft.com/en-us/library/dd460718.aspx">Data Structures for Parallel Programming</see>
    public partial class ConcurrentList<T> : CollectionBase<T>, IConcurrentList<T>
    {
        /// <summary>
        /// List backing field.
        /// </summary>
        private readonly IList<T> _list;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ConcurrentList()
            : this(new List<T>())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="capacity"></param>
        public ConcurrentList(int capacity)
            : this(new List<T>(capacity))
        {
        }

        /// <inheritdoc />
        public virtual void AddRange(IEnumerable<T> values)
        {
            ConcurrentAction(l =>
            {
                foreach (var value in values)
                {
                    l.Add(value);
                }
            });
        }

        /// <inheritdoc cref="Add(T)"/>>
        public override void Add(T item) => ConcurrentAction(l => l.Add(item));

        /// <inheritdoc />
        public override bool Remove(T item) => ConcurrentFunc(l => l.Remove(item));

        /// <inheritdoc cref="Clear" />
        public override void Clear() => ConcurrentAction(l => l.Clear());

        /// <inheritdoc />
        public override bool Contains(T item) => ConcurrentFunc(l => l.Contains(item));

        /// <inheritdoc />
        public override void CopyTo(T[] array, int arrayIndex) => ConcurrentAction(l => l.CopyTo(array, arrayIndex));

        /// <inheritdoc />
        public override int Count => ConcurrentFunc(l => l.Count);

        /// <inheritdoc />
        public override bool IsReadOnly => ConcurrentFunc(l => l.IsReadOnly);

        /// <inheritdoc />
        public int IndexOf(T item) => ConcurrentFunc(l => l.IndexOf(item));

        /// <inheritdoc />
        public void Insert(int index, T item) => ConcurrentAction(l => l.Insert(index, item));

        /// <inheritdoc />
        public void RemoveAt(int index) => ConcurrentAction(l => l.RemoveAt(index));

        /// <inheritdoc />
        public T this[int index]
        {
            get => ConcurrentFunc(l => l[index]);
            set => ConcurrentAction(l => l[index] = value);
        }

        /// <inheritdoc />
        public new IEnumerator GetEnumerator() => new ConcurrentEnumerator<T>(_list, _lock);
    }
}
