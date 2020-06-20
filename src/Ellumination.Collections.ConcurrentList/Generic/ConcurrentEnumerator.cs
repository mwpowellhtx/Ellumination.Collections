using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Ellumination.Collections.Generic
{
    /// <summary>
    /// Implements <see cref="IEnumerator{T}"/> in a Concurrent manner.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ConcurrentEnumerator<T> : IEnumerator<T>
    {
        /// <summary>
        /// Lock backing field.
        /// </summary>
        private readonly ReaderWriterLockSlim _lock;

        /// <summary>
        /// Enumerator backing field.
        /// </summary>
        private readonly IEnumerator<T> _enumerator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target"></param>
        /// <param name="lock"></param>
        internal ConcurrentEnumerator(IEnumerable<T> target, ReaderWriterLockSlim @lock)
        {
            _lock = @lock;
            _lock.EnterReadLock();
            _enumerator = target.GetEnumerator();
        }

        /// <inheritdoc />
        public T Current => _enumerator.Current;

        /// <inheritdoc />
        object IEnumerator.Current => Current;

        /// <inheritdoc />
        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        /// <inheritdoc />
        public void Reset()
        {
            _enumerator.Reset();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _lock.ExitReadLock();
        }
    }
}
