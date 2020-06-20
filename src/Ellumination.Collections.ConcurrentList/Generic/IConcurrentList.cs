using System.Collections.Generic;

namespace Ellumination.Collections.Generic
{
    // TODO: TBD: could look at implementing IProducerConsumerCollection: i.e. tryadd, trytake, etc
    /// <summary>
    /// Provides a Thread safe <see cref="IList{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IConcurrentList<T> : IList<T>
    {
        /// <summary>
        /// Allows for the ability to Add <paramref name="values"/>.
        /// </summary>
        /// <param name="values"></param>
        void AddRange(IEnumerable<T> values);
    }
}
