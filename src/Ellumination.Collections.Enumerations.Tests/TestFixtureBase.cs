using System;
using System.Collections.Generic;

namespace Ellumination.Collections
{
    using Xunit.Abstractions;

    /// <summary>
    /// 
    /// </summary>
    public abstract class TestFixtureBase : IDisposable
    {
        /// <summary>
        /// Gets the OutputHelper.
        /// </summary>
        protected ITestOutputHelper OutputHelper { get; }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="outputHelper"></param>
        /// <inheritdoc />
        protected TestFixtureBase(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        /// <summary>
        /// Returns the Single <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        protected static T GetSingle<T>(T value) => value;

        /// <summary>
        /// Returns a Range of <see cref="IEnumerable{T}"/> corresponding
        /// to the <paramref name="values"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        protected static IEnumerable<T> GetRange<T>(params T[] values)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var x in values)
            {
                yield return x;
            }
        }

        /// <summary>
        /// Gets whether the object IsDisposed.
        /// </summary>
        protected bool IsDisposed { get; private set; }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            IsDisposed = true;
        }
    }
}
