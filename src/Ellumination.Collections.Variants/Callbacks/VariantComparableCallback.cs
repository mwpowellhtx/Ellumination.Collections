using System;

namespace Ellumination.Collections.Variants
{
    /// <summary>
    /// Callback for use during comparisons involving <see cref="IComparable{T}"/>, where
    /// Comparable Generic Type is either of <see cref="IVariant"/> or <see cref="IVariant{T}"/>.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public delegate int VariantComparableCallback(object a, object b);
}
