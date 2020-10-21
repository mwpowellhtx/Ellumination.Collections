namespace Ellumination.Collections.Generic
{
    /// <summary>
    /// Item Callback that occurs Before or After either Adding or Removing.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="item"></param>
    public delegate void BidirectionalListItemCallback<in T>(T item);

    /// <summary>
    /// Key Value pair Callback that occurs Before or After either Adding or Removing.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public delegate void BidirectionalDictionaryKeyValueCallback<in TKey, in TValue>(TKey key, TValue value);

    /// <summary>
    /// Callback evaluates approaching <see cref="BidirectionalSingleton{T}.Value"/> Get clause.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="old"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public delegate bool TryBidirectionalSingletonOnGetClause<T>(T old, out T result);

    /// <summary>
    /// Callback occurs at strategic moments during <see cref="BidirectionalSingleton{T}.Value"/>
    /// Set or Get clauses.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="old"></param>
    /// <param name="current"></param>
    /// <param name="value"></param>
    public delegate void BidirectionalSingletonOnClause<in T>(T old, T current, T value);

    /// <summary>
    /// Callback occurs approaching <see cref="BidirectionalSingleton{T}.Value"/> Set clause.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="old"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public delegate bool TryBidirectionalSingletonOnSetClause<in T>(T old, T value);
}
