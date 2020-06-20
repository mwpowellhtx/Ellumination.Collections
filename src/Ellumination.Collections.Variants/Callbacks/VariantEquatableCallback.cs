using System;

namespace Ellumination.Collections.Variants
{
    /// <summary>
    /// Callback for use during comparisons involving <see cref="IEquatable{T}"/>, where
    /// Equatable Generic Type is either of <see cref="IVariant"/> or <see cref="IVariant{T}"/>.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public delegate bool VariantEquatableCallback(object a, object b);
}
