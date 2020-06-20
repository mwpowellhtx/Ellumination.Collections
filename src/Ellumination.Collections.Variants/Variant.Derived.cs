using System;

namespace Ellumination.Collections.Variants
{
    /// <inheritdoc cref="Variant" />
    public class Variant<T> : Variant, IVariant<T>
    {
        /// <summary>
        /// Occurs when the Strongly Typed <typeparamref name="T"/> Value changed.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnValueChanged(T value) => VariantType = typeof(T);

        /// <inheritdoc />
        public new T Value
        {
            get => (T)ProtectedValue;
            set
            {
                ProtectedValue = value;
                OnValueChanged(value);
            }
        }

        /// <inheritdoc />
        public Variant(Type variantType, object value, IVariantConfigurationCollection configuration)
            : base(variantType, value, configuration)
        {
        }

        /// <inheritdoc />
        public Variant(T value, IVariantConfigurationCollection configuration)
            : base(typeof(T), value, configuration)
        {
        }

        /// <inheritdoc />
        public Variant(IVariantConfigurationCollection configuration)
            : base(typeof(T), default(T), configuration)
        {
        }

        /// <inheritdoc />
        public bool Equals(IVariant<T> other) => Equals(this, other as Variant<T>);

        /// <inheritdoc />
        public int CompareTo(IVariant<T> other) => CompareTo(this, other as Variant<T>);
    }
}
