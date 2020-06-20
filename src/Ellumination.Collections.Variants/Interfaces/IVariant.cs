using System;

namespace Ellumination.Collections.Variants
{
    /// <summary>
    /// Represents a Variant.
    /// </summary>
    /// <inheritdoc cref="IEquatable{T}" />
    /// <inheritdoc cref="IComparable{T}" />
    public interface IVariant : IEquatable<IVariant>, IComparable<IVariant>
    {
        /// <summary>
        /// Gets the <see cref="Type"/> represented by the <see cref="Value"/>.
        /// </summary>
        Type VariantType { get; }

        /// <summary>
        /// Gets or sets the <see cref="object"/> Value.
        /// </summary>
        object Value { get; set; }
    }

    /// <inheritdoc cref="IVariant" />
    /// <inheritdoc cref="IEquatable{T}" />
    /// <inheritdoc cref="IComparable{T}" />
    /// <typeparam name="T"></typeparam>
    public interface IVariant<T> : IVariant, IEquatable<IVariant<T>>, IComparable<IVariant<T>>
    {
        /// <summary>
        /// Gets or sets the Strongly Typed <typeparamref name="T"/> Value.
        /// </summary>
        new T Value { get; set; }
    }
}
