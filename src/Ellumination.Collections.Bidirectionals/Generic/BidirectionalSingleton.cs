using System;

namespace Ellumination.Collections.Generic
{
    /// <inheritdoc cref="IBidirectionalSingleton"/>
    public abstract class BidirectionalSingleton : IBidirectionalSingleton
    {
        /// <inheritdoc/>
        public Type ValueType { get; }

        /// <inheritdoc/>
        public Type SingletonType { get; }

        /// <inheritdoc/>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets the UnboundSingletonType.
        /// </summary>
        public static Type UnboundSingletonType { get; } = typeof(BidirectionalSingleton<>);

        /// <summary>
        /// Protected Internal Constructor.
        /// </summary>
        /// <param name="valueType"></param>
        protected internal BidirectionalSingleton(Type valueType)
        {
            this.ValueType = valueType;
            this.SingletonType = UnboundSingletonType.MakeGenericType(valueType);
        }
    }
}