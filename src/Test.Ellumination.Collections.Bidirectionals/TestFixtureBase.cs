using System;
using System.Collections.Generic;

namespace Ellumination.Collections
{
    /// <summary>
    /// These tests do not need to be complicated, we do not need to involve an actual
    /// Parent-Child relationship, even though that is at least one strong use case candidate.
    /// We can even base the tests upon <see cref="int"/>, just as long as the Item Type is
    /// <see cref="IEquatable{T}"/>.
    /// </summary>
    /// <inheritdoc />
    public abstract class TestFixtureBase : IDisposable
    {
        protected bool IsDisposed { get; private set; }

        /// <summary>
        /// Override this method in order to extend behavior <see cref="IDisposable"/>
        /// into a more test fixture context.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// Gets the <see cref="IEnumerable{T}"/> Range corresponding to the
        /// <paramref name="values"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        protected static IEnumerable<T> Range<T>(params T[] values)
        {
            foreach (var value in values)
            {
                yield return value;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            IsDisposed = true;
        }
    }
}
