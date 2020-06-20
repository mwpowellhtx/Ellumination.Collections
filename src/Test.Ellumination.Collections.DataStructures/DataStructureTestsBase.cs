using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using Xunit;
    using static String;

    public abstract class DataStructureTestsBase<T, TDataStructure> : IDisposable
        where TDataStructure : class, new()
    {
        // TODO: TBD: what was the point of this one? I'm not sure we need a stringifying ItemList any longer...
        public class ItemList : IList<T>
        {
            private readonly IList<T> _list;

            internal ItemList(params T[] items)
            {
                _list = items.ToList();
            }

            private void ListAction(Action<IList<T>> action) => action(_list);

            private TResult ListFunc<TResult>(Func<IList<T>, TResult> func) => func(_list);

            public IEnumerator GetEnumerator() => ListFunc(x => x.GetEnumerator());

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => ListFunc(x => x.GetEnumerator());

            public void Add(T item) => ListAction(x => x.Add(item));

            public void Clear() => ListAction(x => x.Clear());

            public bool Contains(T item) => ListFunc(x => x.Contains(item));

            public void CopyTo(T[] array, int arrayIndex) => ListAction(x => x.CopyTo(array, arrayIndex));

            public bool Remove(T item) => ListFunc(x => x.Remove(item));

            public int Count => ListFunc(x => x.Count);

            public bool IsReadOnly => false;

            public int IndexOf(T item) => ListFunc(x => x.IndexOf(item));

            public void Insert(int index, T item) => ListAction(x => x.Insert(index, item));

            public void RemoveAt(int index) => ListAction(x => x.RemoveAt(index));

            public T this[int index]
            {
                get { return ListFunc(x => x[index]); }
                set { ListAction(x => x[index] = value); }
            }

            /// <summary>
            /// Everything else is plumbing with the inner <see cref="IList{T}"/>. What this
            /// actually does it to inform the unit test runner what the discovered test
            /// signature actually looks like.
            /// </summary>
            /// <returns></returns>
            public override string ToString() => Join(Join(", ", from x in _list select $"{x}"), "{", "}");
        }

        protected TDataStructure Subject { get; private set; }

        protected DataStructureTestsBase()
        {
            Subject = new TDataStructure();
        }

        protected static TDataStructure Verify(TDataStructure subject, Action<TDataStructure> verify = null)
        {
            subject.AssertNotNull();
            verify?.Invoke(subject);
            return subject;
        }

        protected bool IsDisposed { get; private set; }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed || !disposing)
            {
                return;
            }

            (Subject as IDisposable)?.Dispose();
            Subject = null;
        }

        public void Dispose()
        {
            Dispose(true);
            IsDisposed = true;
        }
    }
}
