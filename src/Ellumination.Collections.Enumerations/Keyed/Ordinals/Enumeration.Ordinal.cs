using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Keyed.Ordinals
{
    public abstract partial class Enumeration<TKey, T>
    {
        /// <summary>
        /// Gets the <see cref="Enumeration.DisplayName"/> for Debugging purposes.
        /// Also includes the <see cref="Ordinal"/> value.
        /// </summary>
        protected internal override string DebuggerDisplayName => $"{DisplayName} (Ordinal: {Ordinal})";

        /// <summary>
        /// Gets the Ordinal value. Acts as the proxy for <see cref="Enumeration{TKey}.Key"/>.
        /// </summary>
        public virtual TKey Ordinal
        {
            get => base.Key;
            protected set => base.Key = value;
        }

        /// <summary>
        /// Returns the FirstOrDefault <see cref="Keyed.Enumeration{TKey,T}.Values"/>
        /// value corresponding to the given <paramref name="ordinal"/>.
        /// </summary>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        public static T FromOrdinal(TKey ordinal) => Values.FirstOrDefault(x => x.Ordinal.CompareTo(ordinal) == default(int));

        /// <summary>
        /// Callback used in order to Increment the <paramref name="ordinal"/>.
        /// </summary>
        /// <param name="ordinal">The Ordinal value prior to the Callback.</param>
        /// <returns>The <paramref name="ordinal"/> following the Callback invocation.</returns>
        protected delegate TKey IncrementOrdinalCallback(TKey ordinal);

        /// <summary>
        /// Initializes the <see cref="Ordinal"/> Values.
        /// </summary>
        /// <param name="values">The Values being Initialized.</param>
        /// <param name="callback">The Callback used to Increment the Ordinal value.</param>
        /// <param name="defaultOrdinal">The Default Ordinal value used to initialize the request.</param>
        protected static void InitializeOrdinals(IEnumerable<T> values, IncrementOrdinalCallback callback, TKey defaultOrdinal = default(TKey))
        {
            values.ToArray().Aggregate(defaultOrdinal, (o, x) => x.Ordinal = callback.Invoke(o));
        }

        /// <inheritdoc />
        public override IEnumerable<T> EnumeratedValues
        {
            /* In this case, there is nothing to unpack where Ordinal-based Enumerations are
             * concerned. That is to say, an Ordinal-Enumerated Value is a composite of One. */
            get { yield return (T) this; }
        }
    }
}
