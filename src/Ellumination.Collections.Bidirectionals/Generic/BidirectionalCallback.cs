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
}
