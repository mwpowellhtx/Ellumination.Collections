using System;

namespace Ellumination.Collections.Generic
{
    /// <summary>
    /// Provides the basic Singleton interface.
    /// </summary>
    public interface IBidirectionalSingleton
    {
        /// <summary>
        /// Gets the ValueType.
        /// </summary>
        Type ValueType { get; }

        /// <summary>
        /// Gets the SingletonType.
        /// </summary>
        Type SingletonType { get; }

        /// <summary>
        /// Gets or Sets whether the Singleton IsReadOnly.
        /// </summary>
        bool IsReadOnly { get; set; }
    }
}
