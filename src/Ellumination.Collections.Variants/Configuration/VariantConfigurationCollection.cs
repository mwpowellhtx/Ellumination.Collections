using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Variants
{
    /// <inheritdoc />
    public class VariantConfigurationCollection : IVariantConfigurationCollection
    {
        private readonly IList<IVariantConfiguration> _items;

        /// <summary>
        /// Returns a newly Created <see cref="VariantConfigurationCollection"/> instance
        /// containing the <paramref name="items"/>.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static VariantConfigurationCollection Create(params IVariantConfiguration[] items)
            => new VariantConfigurationCollection(items);

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <inheritdoc />
        public VariantConfigurationCollection()
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            : this(new List<IVariantConfiguration> { })
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="items"></param>
        public VariantConfigurationCollection(IEnumerable<IVariantConfiguration> items)
        {
            _items = items.ToList();
        }

        private void ListAction(Action<IList<IVariantConfiguration>> action) => action.Invoke(_items);

        private TResult ListFunc<TResult>(Func<IList<IVariantConfiguration>, TResult> func) => func.Invoke(_items);

        /// <inheritdoc />
        public IEnumerator<IVariantConfiguration> GetEnumerator() => ListFunc(x => x.GetEnumerator());

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        public void Add(IVariantConfiguration item) => ListAction(x => x.Add(item));

        /// <inheritdoc />
        public void Clear() => ListAction(x => x.Clear());

        /// <inheritdoc />
        public bool Contains(IVariantConfiguration item) => ListFunc(x => x.Contains(item));

        /// <inheritdoc />
        public void CopyTo(IVariantConfiguration[] array, int arrayIndex) => ListAction(x => x.CopyTo(array, arrayIndex));

        /// <inheritdoc />
        public bool Remove(IVariantConfiguration item) => ListFunc(x => x.Remove(item));

        /// <inheritdoc />
        public int Count => ListFunc(x => x.Count);

        /// <inheritdoc />
        public bool IsReadOnly => ListFunc(x => x.IsReadOnly);
    }
}
