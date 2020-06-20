namespace Ellumination.Collections
{
    /// <summary>
    /// Represents the basic Enumeration concern.
    /// </summary>
    public abstract partial class Enumeration
    {
        /// <summary>
        /// Protected Default Constructor.
        /// </summary>
        protected Enumeration()
            : this(null, null)
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        protected Enumeration(string name, string displayName)
        {
            _name = name;
            _displayName = displayName;
        }
    }
}
