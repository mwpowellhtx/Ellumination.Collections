using System;
using System.Linq;
using System.Reflection;

namespace Ellumination.Collections.Variants
{
    using static String;

    // TODO: TBD: next, consider splitting some bits into separate files, organize in the project...
    // TODO: TBD: need tests on the interface bits, equatable, comparable, etc
    /// <inheritdoc />
    public abstract class Variant : IVariant
    {
        /// <summary>
        /// &quot;.&quot;
        /// </summary>
        private const string Dot = ".";

        /// <summary>
        /// Gets or sets the Configuration.
        /// </summary>
        protected IVariantConfigurationCollection Configuration { get; set; }

        /// <inheritdoc />
        public Type VariantType { get; protected set; }

        /// <summary>
        /// Override in order to do something different than simply setting the <see cref="VariantType"/>.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnValueChanged(object value)
        {
            if (VariantType != null)
            {
                return;
            }

            VariantType = value?.GetType();
        }

        /// <summary>
        /// Gets or sets the <see cref="Value"/> sans the <see cref="VariantType"/>.
        /// </summary>
        protected internal object ProtectedValue { get; protected set; }

        /// <inheritdoc />
        public object Value
        {
            get => ProtectedValue;
            set
            {
                if (ReferenceEquals(value, ProtectedValue))
                {
                    /* Just return silently in this instance.
                     * Already set, or at least consistent (i.e. nulls), nothing further to do. */
                    return;
                }

                // TODO: TBD: there is probably a compatible types helper function in here somewhere...
                if (!(ProtectedValue == null || value == null)
                    && !(value.GetType() == VariantType
                        // ReSharper disable once UseMethodIsInstanceOfType
                        || VariantType.IsAssignableFrom(value.GetType())))
                {
                    var valueType = value.GetType();
                    var message = $"Unable to change type from '{VariantType.FullName}' midstream."
                                  + $"Create a new Variant for the desired type '{valueType.FullName}'.";

                    throw new InvalidOperationException(message)
                    {
                        Data =
                        {
                            {Join(Dot, typeof(Variant), nameof(VariantType)), VariantType},
                            {Join(Dot, typeof(Variant), nameof(ProtectedValue)), ProtectedValue},
                            {nameof(valueType), valueType},
                            {nameof(value), value}
                        }
                    };
                }

                // Clear to re-set the Value.
                ProtectedValue = value;
                OnValueChanged(ProtectedValue);
            }
        }

        /// <summary>
        /// Verifies that the <see cref="Configuration"/>, which should already have been
        /// set by this point, may support the requested <paramref name="variantType"/>.
        /// </summary>
        /// <param name="variantType"></param>
        private void VerifyConfiguration(Type variantType)
        {
            var configuration = Configuration;

            if (configuration.Any(x => x.VariantType == variantType))
            {
                return;
            }

            var message = $"'{variantType.FullName}' configuration must be specified.";

            throw new ArgumentException(message, nameof(configuration))
            {
                Data =
                {
                    {Join(Dot, typeof(Variant).FullName, nameof(VariantType)), VariantType},
                    {nameof(variantType), variantType},
                    {nameof(ProtectedValue), ProtectedValue},
                    {nameof(configuration), configuration}
                }
            };
        }

        /// <summary>
        /// Verifies that the <paramref name="variantType"/> is Aligned with the
        /// <see cref="ProtectedValue"/> and <see cref="VariantType"/>, which should
        /// already have been set by this point.
        /// </summary>
        /// <param name="variantType"></param>
        private void VerifyVariantTypeAlignment(Type variantType)
        {
            if (variantType == VariantType || variantType.IsAssignableFrom(VariantType))
            {
                return;
            }

            var message = $"'{variantType.FullName}' is incompatible with the"
                          + $" '{Join(Dot, typeof(Variant).FullName, nameof(ProtectedValue))}'"
                          + $" type '{VariantType.FullName}'.";

            // We could see this throwing InvalidOperationException just the same.
            throw new ArgumentException(message, nameof(variantType))
            {
                Data =
                {
                    {Join(Dot, typeof(Variant).FullName, nameof(VariantType)), VariantType},
                    {nameof(variantType), variantType},
                    {nameof(ProtectedValue), ProtectedValue},
                    {nameof(Configuration), Configuration}
                }
            };
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="variantType"></param>
        /// <param name="value"></param>
        /// <param name="configuration"></param>
        /// <inheritdoc />
        protected Variant(Type variantType, object value, IVariantConfigurationCollection configuration)
            : this(value, configuration)
        {
            /* The Value property, and therefore the VariantType, will have already
             * been set. Now, verify whether the intended VariantType is compatible. */

            VerifyConfiguration(variantType);
            VerifyVariantTypeAlignment(variantType);

            VariantType = variantType;
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="configuration"></param>
        private Variant(object value, IVariantConfigurationCollection configuration)
        {
            Value = value;
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration))
            {
                Data =
                {
                    {nameof(VariantType), VariantType},
                    {nameof(value), value},
                    // ReSharper disable once ExpressionIsAlwaysNull
                    {nameof(configuration), configuration}
                }
            };
        }

        /// <summary>
        /// Creates a new <see cref="Variant{T}"/> given a default <typeparamref name="T"/>
        /// instance. If possible, will invoke the default public constructor.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static Variant<T> Create<T>(IVariantConfigurationCollection configuration)
        {
            // ReSharper disable once IdentifierTypo
            bool TryInvokeParameterlessConstructor(out T value)
            {
                value = default(T);
                var type = typeof(T);
                // ReSharper disable once InvertIf
                if (type.IsClass && !(type.IsAbstract || type.IsInterface))
                {
                    var ctor = type.GetConstructor(new Type[] { });
                    value = (T) ctor?.Invoke(new object[] { });
                }

                return value != null;
            }

            return Create(TryInvokeParameterlessConstructor(out var x) ? x : default(T), configuration);
        }

        /// <summary>
        /// Creates a new <see cref="Variant{T}"/> given the Strongly Typed
        /// <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        /// <see cref="Variant{T}"/>
        /// <see cref="Create(Type,object,IVariantConfigurationCollection)"/>
        public static Variant<T> Create<T>(T value, IVariantConfigurationCollection configuration)
        {
            var variantType = typeof(T);
            var madeVariantType = typeof(Variant<>).MakeGenericType(variantType);
            var ctor = madeVariantType.GetConstructor(new[] {variantType, typeof(IVariantConfigurationCollection)});
            object obj;
            try
            {
                obj = ctor?.Invoke(new object[] {value, configuration});
            }
            // ReSharper disable once IdentifierTypo
            catch (TargetInvocationException tiex)
            {
                throw tiex.InnerException ?? tiex;
            }

            return (Variant<T>) obj;
        }

        /// <summary>
        /// Creates a new <see cref="Variant"/> given the associated
        /// <paramref name="variantType"/> and <paramref name="value"/>.
        /// </summary>
        /// <param name="variantType"></param>
        /// <param name="value"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        /// <see cref="Variant{T}"/>
        public static Variant Create(Type variantType, object value, IVariantConfigurationCollection configuration)
        {
            var madeVariantType = typeof(Variant<>).MakeGenericType(variantType);
            var ctor = madeVariantType.GetConstructor(new[] {typeof(Type), typeof(object), typeof(IVariantConfigurationCollection)});
            object obj;
            try
            {
                obj = ctor?.Invoke(new[] {variantType, value, configuration});
            }
            // ReSharper disable once IdentifierTypo
            catch (TargetInvocationException tiex)
            {
                throw tiex.InnerException ?? tiex;
            }

            return (Variant) obj;
        }

        /// <summary>
        /// Returns whether <paramref name="a"/> Equals <paramref name="b"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <see cref="Configuration"/>
        protected static bool Equals(Variant a, Variant b)
            => !(a == null || b == null)
               && (ReferenceEquals(a, b)
                   || (a.VariantType == b.VariantType
                       && (new[] {a, b}.Select(x => x.Configuration.SingleOrDefault(y => y.VariantType == x.VariantType))
                               .FirstOrDefault(x => x != null && x.VariantType == a.VariantType)
                               ?.EquatableCallback?.Invoke(a.Value, b.Value) ?? false)));

        /// <inheritdoc />
        public bool Equals(IVariant other) => Equals(this, other as Variant);

        /// <summary>
        /// -1
        /// </summary>
        private const int DefaultCompareToResult = -1;

        /// <summary>
        /// Returns the Comparison of <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <see cref="Configuration"/>
        protected static int CompareTo(Variant a, Variant b)
            => (!(a == null || b == null)
                && (ReferenceEquals(a, b)
                    || a.VariantType == b.VariantType))
                ? new[] {a, b}.Select(x => x.Configuration.SingleOrDefault(y => y.VariantType == x.VariantType))
                      .FirstOrDefault(x => x != null && x.VariantType == a.VariantType)
                      ?.ComparableCallback?.Invoke(a.Value, b.Value) ?? DefaultCompareToResult
                : DefaultCompareToResult;

        /// <inheritdoc />
        public int CompareTo(IVariant other) => CompareTo(this, other as Variant);
    }
}
