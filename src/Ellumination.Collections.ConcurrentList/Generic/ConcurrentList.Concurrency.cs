using System;
using System.Collections.Generic;
using System.Threading;

namespace Ellumination.Collections.Generic
{
    public partial class ConcurrentList<T>
    {
        /// <summary>
        /// Lock backing field.
        /// </summary>
        private readonly ReaderWriterLockSlim _lock;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="list"></param>
        public ConcurrentList(IList<T> list)
            : base(list)
        {
            _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            _list = list;
        }

        /// <summary>
        /// Performs an <see cref="Action{T}"/> on the underlying <see cref="_list"/>.
        /// The default concurrent action is <see cref="ReaderWriterLockSlim.EnterWriteLock"/>,
        /// with corresponding concurrent exit.
        /// </summary>
        /// <param name="action"></param>
        private void ConcurrentAction(Action<IList<T>> action)
        {
            ConcurrentAction(x => x.EnterWriteLock(), x => x.ExitWriteLock(), action);
        }

        /// <summary>
        /// Performs an <see cref="Action{T}"/> on the underlying <see cref="_list"/>.
        /// </summary>
        /// <param name="enter"></param>
        /// <param name="exit"></param>
        /// <param name="action"></param>
        private void ConcurrentAction(Action<ReaderWriterLockSlim> enter
            , Action<ReaderWriterLockSlim> exit, Action<IList<T>> action)
        {
            try
            {
                enter(_lock);
                action(_list);
            }
            finally
            {
                exit(_lock);
            }
        }

        /// <summary>
        /// Returns a <typeparamref name="TResult"/> from the underlying <see cref="_list"/>.
        /// The default concurrent function is <see cref="ReaderWriterLockSlim.EnterReadLock"/>,
        /// with corresponding concurrent exit.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        private TResult ConcurrentFunc<TResult>(Func<IList<T>, TResult> func)
            => ConcurrentFunc(x => x.EnterReadLock(), x => x.ExitReadLock(), func);

        /// <summary>
        /// Returns a <typeparamref name="TResult"/> from the underlying <see cref="_list"/>.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="enter"></param>
        /// <param name="exit"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private TResult ConcurrentFunc<TResult>(Action<ReaderWriterLockSlim> enter
            , Action<ReaderWriterLockSlim> exit, Func<IList<T>, TResult> func)
        {
            try
            {
                enter(_lock);
                return func(_list);
            }
            finally
            {
                exit(_lock);
            }
        }
    }
}
