namespace Ellumination.Collections.Keyed.Ordinals
{
    /// <summary>
    /// Provides an <see cref="int"/> based <see cref="Enumeration{TKey,T}"/> base class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract partial class IntegerOrdinalEnumeration<T> : Enumeration<int, T>
        where T : IntegerOrdinalEnumeration<T>
    {
        /// <summary>
        /// Protected Default Constructor.
        /// </summary>
        protected IntegerOrdinalEnumeration()
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="name"></param>
        protected IntegerOrdinalEnumeration(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        protected IntegerOrdinalEnumeration(string name, string displayName)
            : base(name, displayName)
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="ordinal"></param>
        protected IntegerOrdinalEnumeration(int ordinal)
            : base(ordinal)
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="ordinal"></param>
        /// <param name="name"></param>
        protected IntegerOrdinalEnumeration(int ordinal, string name)
            : base(ordinal, name)
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="ordinal"></param>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        protected IntegerOrdinalEnumeration(int ordinal, string name, string displayName)
            : base(ordinal, name, displayName)
        {
        }
    }
}
