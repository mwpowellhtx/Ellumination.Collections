using System.Collections.Generic;

namespace Ellumination.Collections.Generic
{
    /// <summary>
    /// Provides an interface concerning Bidirectional Dictionary.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IBidirectionalDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
    }
}