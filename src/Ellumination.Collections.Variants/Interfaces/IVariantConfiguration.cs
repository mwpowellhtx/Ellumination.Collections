using System;

namespace Ellumination.Collections.Variants
{
    // TODO: TBD: dictionary or collection of configurations...
    // TODO: TBD: how to relay that to variant...
    // TODO: TBD: at the time of new variant? variant.create? default config?
    /// <summary>
    /// Configuration used to inform <see cref="Variant"/> of the types it can support.
    /// </summary>
    public interface IVariantConfiguration
    {
        /// <summary>
        /// Gets the <see cref="Variant.VariantType"/>.
        /// </summary>
        Type VariantType { get; }

        /// <summary>
        /// gets the <see cref="VariantEquatableCallback"/> Callback.
        /// </summary>
        VariantEquatableCallback EquatableCallback { get; }

        /// <summary>
        /// Gets the <see cref="VariantComparableCallback"/> Callback.
        /// </summary>
        VariantComparableCallback ComparableCallback { get; }
    }
}
