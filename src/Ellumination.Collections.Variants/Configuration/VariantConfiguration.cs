using System;

namespace Ellumination.Collections.Variants
{
    /// <inheritdoc /> 
    public class VariantConfiguration : IVariantConfiguration
    {
        /// <inheritdoc /> 
        public Type VariantType { get; }

        /// <inheritdoc /> 
        public VariantEquatableCallback EquatableCallback { get; }

        /// <inheritdoc /> 
        public VariantComparableCallback ComparableCallback { get; }

        /// <inheritdoc /> 
        public VariantConfiguration(Type variantType, VariantEquatableCallback equatableCallback = null, VariantComparableCallback comparableCallback = null)
        {
            VariantType = variantType;
            EquatableCallback = equatableCallback ?? ((_, __) => false);
            ComparableCallback = comparableCallback ?? ((_, __) => -1);
        }

        /// <summary>
        /// Configures a new <see cref="VariantConfiguration"/> instance.
        /// </summary>
        /// <param name="variantType"></param>
        /// <param name="equatableCallback"></param>
        /// <param name="comparableCallback"></param>
        /// <returns></returns>
        public static VariantConfiguration Configure(Type variantType, VariantEquatableCallback equatableCallback = null, VariantComparableCallback comparableCallback = null)
            => new VariantConfiguration(variantType, equatableCallback, comparableCallback);

        /// <summary>
        /// Configures a new <see cref="VariantConfiguration"/> instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="equatableCallback"></param>
        /// <param name="comparableCallback"></param>
        /// <returns></returns>
        public static VariantConfiguration Configure<T>(VariantEquatableCallback equatableCallback = null, VariantComparableCallback comparableCallback = null)
            => Configure(typeof(T), equatableCallback, comparableCallback);
    }
}
