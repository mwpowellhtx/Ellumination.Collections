using System;

namespace Ellumination.Collections
{
    using static AttributeTargets;

    /// <summary>
    /// Marks whether DeclaresEnumeratedValues.
    /// </summary>
    /// <inheritdoc />
    [AttributeUsage(Class, Inherited = false)]
    public class DeclaresEnumeratedValuesAttribute : Attribute
    {
    }
}
