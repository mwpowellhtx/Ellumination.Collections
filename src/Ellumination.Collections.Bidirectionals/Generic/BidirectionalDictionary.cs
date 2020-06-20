using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Generic
{
    /// <inheritdoc />
    public class BidirectionalDictionary<TKey, TValue> : IBidirectionalDictionary<TKey, TValue>
    {
        /// <summary>
        /// Dictionary backing field.
        /// </summary>
        private readonly IDictionary<TKey, TValue> _dictionary;

        /// <summary>
        /// BeforeAdd delegate.
        /// </summary>
        private readonly BidirectionalDictionaryKeyValueCallback<TKey, TValue> _beforeAdd;

        /// <summary>
        /// AfterAdded delegate.
        /// </summary>
        private readonly BidirectionalDictionaryKeyValueCallback<TKey, TValue> _afterAdded;

        /// <summary>
        /// BeforeRemove delegate.
        /// </summary>
        private readonly BidirectionalDictionaryKeyValueCallback<TKey, TValue> _beforeRemove;

        /// <summary>
        /// AfterRemoved delegate.
        /// </summary>
        private readonly BidirectionalDictionaryKeyValueCallback<TKey, TValue> _afterRemoved;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public BidirectionalDictionary()
            : this(new Dictionary<TKey, TValue>())
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="afterAdded"></param>
        /// <param name="afterRemoved"></param>
        /// <param name="beforeAdd"></param>
        /// <param name="beforeRemove"></param>
        public BidirectionalDictionary(
            BidirectionalDictionaryKeyValueCallback<TKey, TValue> afterAdded
            , BidirectionalDictionaryKeyValueCallback<TKey, TValue> afterRemoved
            , BidirectionalDictionaryKeyValueCallback<TKey, TValue> beforeAdd = null
            , BidirectionalDictionaryKeyValueCallback<TKey, TValue> beforeRemove = null)
            : this(new Dictionary<TKey, TValue>(), afterAdded, afterRemoved, beforeAdd, beforeRemove)
        {
        }

        private static readonly BidirectionalDictionaryKeyValueCallback<TKey, TValue> DefaultCallback = (k, v) => { };

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="afterAdded"></param>
        /// <param name="afterRemoved"></param>
        /// <param name="beforeAdd"></param>
        /// <param name="beforeRemove"></param>
        public BidirectionalDictionary(IDictionary<TKey, TValue> dictionary
            , BidirectionalDictionaryKeyValueCallback<TKey, TValue> afterAdded = null
            , BidirectionalDictionaryKeyValueCallback<TKey, TValue> afterRemoved = null
            , BidirectionalDictionaryKeyValueCallback<TKey, TValue> beforeAdd = null
            , BidirectionalDictionaryKeyValueCallback<TKey, TValue> beforeRemove = null)
        {
            _dictionary = dictionary;
            _afterAdded = afterAdded ?? DefaultCallback;
            _afterRemoved = afterRemoved ?? DefaultCallback;
            _beforeAdd = beforeAdd ?? DefaultCallback;
            _beforeRemove = beforeRemove ?? DefaultCallback;
        }

        /// <summary>
        /// Performs the <paramref name="action"/> on the <see cref="_dictionary"/>.
        /// </summary>
        /// <param name="action"></param>
        private void DictionaryAction(Action<IDictionary<TKey, TValue>> action)
            => action.Invoke(_dictionary);

        /// <summary>
        /// Performs the <paramref name="func"/> on the <see cref="_dictionary"/>.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        private TResult DictionaryFunc<TResult>(Func<IDictionary<TKey, TValue>, TResult> func)
            => func.Invoke(_dictionary);

        /// <inheritdoc />
        public void Add(TKey key, TValue value) => DictionaryAction(x =>
        {
            _beforeAdd.Invoke(key, value);
            x.Add(key, value);
            _afterAdded.Invoke(key, value);
        });

        /// <inheritdoc />
        public bool ContainsKey(TKey key) => DictionaryFunc(x => x.ContainsKey(key));

        /// <inheritdoc />
        public ICollection<TKey> Keys => DictionaryFunc(x => x.Keys);

        /// <inheritdoc />
        public bool Remove(TKey key) => DictionaryFunc(x => x.Remove(key));

        /// <inheritdoc />
        public bool TryGetValue(TKey key, out TValue value)
        {
            var local = default(TValue);
            var result = DictionaryFunc(x => x.TryGetValue(key, out local));
            value = local;
            return result;
        }

        /// <inheritdoc />
        public ICollection<TValue> Values => DictionaryFunc(x => x.Values);

        /// <inheritdoc />
        public TValue this[TKey key]
        {
            get => DictionaryFunc(x => x[key]);
            set => DictionaryAction(x =>
            {
                var old = default(TValue);
                var didContainValue = ContainsKey(key);

                if (didContainValue)
                {
                    _beforeRemove.Invoke(key, old = _dictionary[key]);
                }

                _beforeAdd.Invoke(key, value);

                x[key] = value;

                if (didContainValue)
                {
                    _afterRemoved.Invoke(key, old);
                }

                _afterAdded(key, value);
            });
        }

        /// <inheritdoc />
        public void Add(KeyValuePair<TKey, TValue> item) => DictionaryAction(x =>
        {
            _beforeAdd.Invoke(item.Key, item.Value);
            x.Add(item);
            _afterAdded.Invoke(item.Key, item.Value);
        });

        /// <inheritdoc />
        public void Clear() => DictionaryAction(x =>
        {
            var items = x.ToArray();

            foreach (var item in items)
            {
                _beforeRemove.Invoke(item.Key, item.Value);
            }

            x.Clear();

            foreach (var item in items)
            {
                _afterRemoved.Invoke(item.Key, item.Value);
            }
        });

        /// <inheritdoc />
        public bool Contains(KeyValuePair<TKey, TValue> item) => DictionaryFunc(x => x.Contains(item));

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => DictionaryAction(x => x.CopyTo(array, arrayIndex));

        /// <inheritdoc />
        public int Count => DictionaryFunc(x => x.Count);

        /// <inheritdoc />
        public bool IsReadOnly => DictionaryFunc(x => x.IsReadOnly);

        /// <inheritdoc />
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (Contains(item))
            {
                _beforeRemove.Invoke(item.Key, item.Value);
            }

            var result = DictionaryFunc(x => x.Remove(item));

            if (result)
            {
                _afterRemoved.Invoke(item.Key, item.Value);
            }

            return result;
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
