using System;

namespace Ellumination.Collections
{
    /// <summary>
    /// Specifies whether <see cref="Expansion"/> or <see cref="Contraction"/> is allowed,
    /// <see cref="Both"/>, or <see cref="None"/>.
    /// </summary>
    [Flags]
    public enum Elasticity
    {
        /// <summary>
        /// No <see cref="Expansion"/> or <see cref="Contraction"/> to take place.
        /// </summary>
        None = 0,

        /// <summary>
        /// Provides for Expansion only. No <see cref="Contraction"/> to take place.
        /// </summary>
        Expansion = 1,

        /// <summary>
        /// Provides for Contraction only. No <see cref="Expansion"/> to take place.
        /// </summary>
        Contraction = 1 << 1,

        /// <summary>
        /// Both <see cref="Expansion"/> and <see cref="Contraction"/> to take place.
        /// </summary>
        Both = Expansion | Contraction,

        /// <summary>
        /// Use this option to convey that the operation should not throw any exception, but,
        /// rather, simply return from whatever operation is being performed. This is mostly
        /// for internal use, but may be desired for external operations.
        /// </summary>
        Silent = 1 << 2
    }
}
