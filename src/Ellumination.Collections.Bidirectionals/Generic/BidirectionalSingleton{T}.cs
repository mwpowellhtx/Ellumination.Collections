using System;

namespace Ellumination.Collections.Generic
{
    /// <inheritdoc cref="IBidirectionalSingleton{T}"/>
    public class BidirectionalSingleton<T> : BidirectionalSingleton, IBidirectionalSingleton<T>
    {
        private T _value;

        /// <inheritdoc/>
        public T Value
        {
            get => this.Get();
            set => this.Set(value);
        }

        /// <inheritdoc/>
        public virtual T Get()
        {
            // Yes we do want to capture the Old value prior to Eval.
            var old = this._value;

            {
                T result = default;

                if (this.EvaluateGet?.Invoke(this._value, out result) == true)
                {
                    this._value = result;
                }
            }

            this.BeforeGet?.Invoke(old, this._value, this._value);

            return this._value;
        }

        /// <inheritdoc/>
        public virtual void Set(T value)
        {
            var old = this._value;

            // Bypass the set clause while readonly or when evaluation returns false.
            if (this.IsReadOnly || this.EvaluateSet?.Invoke(old, value) == false)
            {
                // TODO: TBD: depending on how it operates, we could be persuaded differently later on...
                // TODO: TBD: if we threw anything it would be InvalidOperationException, but not for now...

                /* We think that this should be silent, always. This is because we want to be
                 * serialization friendly. Meaning that on deserialization we may set a set occur,
                 * and we want to allow for this to be the case. */

                return;
            }

            // Not readonly and evaluation no response (null) or true, perform the set.
            this.BeforeSet?.Invoke(old, this._value, value);

            this._value = value;

            this.AfterSet?.Invoke(old, this._value, this._value);
        }

        /// <inheritdoc/>
        public virtual event BidirectionalSingletonOnClause<T> BeforeGet;

        /// <inheritdoc/>
        public virtual event BidirectionalSingletonOnClause<T> AfterSet;

        /// <inheritdoc/>
        public virtual event BidirectionalSingletonOnClause<T> BeforeSet;

        /// <inheritdoc/>
        public virtual event TryBidirectionalSingletonOnGetClause<T> EvaluateGet;

        /// <inheritdoc/>
        public virtual event TryBidirectionalSingletonOnSetClause<T> EvaluateSet;

        /// <summary>
        /// Constructs an instance of the Singleton given default value and callbacks.
        /// </summary>
        /// <remarks>Remember that defaults for delegates are null since delegates are references
        /// types. Rinse and repeat throughout the constructors and such.</remarks>
        public BidirectionalSingleton() : this(default, default, default, default, default, default)
        {
        }

        /// <summary>
        /// Constructs an instance of the Singleton given several of its value and callbacks.
        /// </summary>
        /// <param name="value">The <see cref="Value"/> under consideration.</param>
        public BidirectionalSingleton(T value) : this(value, default, default, default, default, default)
        {
        }

        /// <summary>
        /// Constructs an instance of the Singleton given several of its value and callbacks.
        /// </summary>
        /// <param name="onBeforeGet">Callback occurs On <see cref="BeforeGet"/>.</param>
        /// <param name="onEvaluateGet">Callback occurs On <see cref="EvaluateGet"/>.</param>
        public BidirectionalSingleton(
            BidirectionalSingletonOnClause<T> onBeforeGet
            , TryBidirectionalSingletonOnGetClause<T> onEvaluateGet
        ) : this(default, onBeforeGet, default, default, onEvaluateGet, default)
        {
        }

        /// <summary>
        /// Constructs an instance of the Singleton given several of its value and callbacks.
        /// </summary>
        /// <param name="value">The <see cref="Value"/> under consideration.</param>
        /// <param name="onBeforeGet">Callback occurs On <see cref="BeforeGet"/>.</param>
        /// <param name="onEvaluateGet">Callback occurs On <see cref="EvaluateGet"/>.</param>
        public BidirectionalSingleton(T value
            , BidirectionalSingletonOnClause<T> onBeforeGet
            , TryBidirectionalSingletonOnGetClause<T> onEvaluateGet
        ) : this(value, onBeforeGet, default, default, onEvaluateGet, default)
        {
        }

        /// <summary>
        /// Constructs an instance of the Singleton given several of its value and callbacks.
        /// </summary>
        /// <param name="onBeforeSet">Callback occurs On <see cref="BeforeSet"/>.</param>
        /// <param name="onAfterSet">Callback occurs On <see cref="AfterSet"/>.</param>
        /// <param name="onEvaluateSet">Callback occurs On <see cref="EvaluateSet"/>.</param>
        public BidirectionalSingleton(
            BidirectionalSingletonOnClause<T> onBeforeSet
            , BidirectionalSingletonOnClause<T> onAfterSet
            , TryBidirectionalSingletonOnSetClause<T> onEvaluateSet
        ) : this(default, default, onBeforeSet, onAfterSet, default, onEvaluateSet)
        {
        }

        /// <summary>
        /// Constructs an instance of the Singleton given several of its value and callbacks.
        /// </summary>
        /// <param name="value">The <see cref="Value"/> under consideration.</param>
        /// <param name="onBeforeSet">Callback occurs On <see cref="BeforeSet"/>.</param>
        /// <param name="onAfterSet">Callback occurs On <see cref="AfterSet"/>.</param>
        /// <param name="onEvaluateSet">Callback occurs On <see cref="EvaluateSet"/>.</param>
        public BidirectionalSingleton(T value
            , BidirectionalSingletonOnClause<T> onBeforeSet
            , BidirectionalSingletonOnClause<T> onAfterSet
            , TryBidirectionalSingletonOnSetClause<T> onEvaluateSet
        ) : this(value, default, onBeforeSet, onAfterSet, default, onEvaluateSet)
        {
        }

        /// <summary>
        /// Constructs an instance of the Singleton given several of its value and callbacks.
        /// </summary>
        /// <param name="onBeforeGet">Callback occurs On <see cref="BeforeGet"/>.</param>
        /// <param name="onBeforeSet">Callback occurs On <see cref="BeforeSet"/>.</param>
        /// <param name="onAfterSet">Callback occurs On <see cref="AfterSet"/>.</param>
        /// <param name="onEvaluateGet">Callback occurs On <see cref="EvaluateGet"/>.</param>
        /// <param name="onEvaluateSet">Callback occurs On <see cref="EvaluateSet"/>.</param>
        public BidirectionalSingleton(
            BidirectionalSingletonOnClause<T> onBeforeGet
            , BidirectionalSingletonOnClause<T> onBeforeSet
            , BidirectionalSingletonOnClause<T> onAfterSet
            , TryBidirectionalSingletonOnGetClause<T> onEvaluateGet
            , TryBidirectionalSingletonOnSetClause<T> onEvaluateSet
        ) : this(default, onBeforeGet, onBeforeSet, onAfterSet, onEvaluateGet, onEvaluateSet)
        {
        }

        /// <summary>
        /// Constructs an instance of the Singleton given several of its value and callbacks.
        /// </summary>
        /// <param name="value">The <see cref="Value"/> under consideration.</param>
        /// <param name="onBeforeGet">Callback occurs On <see cref="BeforeGet"/>.</param>
        /// <param name="onBeforeSet">Callback occurs On <see cref="BeforeSet"/>.</param>
        /// <param name="onAfterSet">Callback occurs On <see cref="AfterSet"/>.</param>
        /// <param name="onEvaluateGet">Callback occurs On <see cref="EvaluateGet"/>.</param>
        /// <param name="onEvaluateSet">Callback occurs On <see cref="EvaluateSet"/>.</param>
        public BidirectionalSingleton(T value
            , BidirectionalSingletonOnClause<T> onBeforeGet
            , BidirectionalSingletonOnClause<T> onBeforeSet
            , BidirectionalSingletonOnClause<T> onAfterSet
            , TryBidirectionalSingletonOnGetClause<T> onEvaluateGet
            , TryBidirectionalSingletonOnSetClause<T> onEvaluateSet
        ) : base(typeof(T))
        {
            this._value = value;

            // Installs the Delegate while also being mindfull that it can be null (default).
            void InstallDelegate<D>(D onCallback, Action<D> installer)
                where D : Delegate
            {
                if (onCallback != default)
                {
                    installer.Invoke(onCallback);
                }
            }

            InstallDelegate(onBeforeGet, x => this.BeforeGet += x);
            InstallDelegate(onBeforeSet, x => this.BeforeSet += x);
            InstallDelegate(onAfterSet, x => this.AfterSet += x);
            InstallDelegate(onEvaluateGet, x => this.EvaluateGet += x);
            InstallDelegate(onEvaluateSet, x => this.EvaluateSet += x);
        }

        /// <summary>
        /// Implicitly converts the <paramref name="singleton"/> using the <see cref="Value"/>
        /// property get clause.
        /// </summary>
        /// <param name="singleton"></param>
        public static implicit operator T(BidirectionalSingleton<T> singleton) => singleton._value;

        /// <summary>
        /// Implicitly converts the <paramref name="value"/> to a
        /// <see cref="BidirectionalSingleton{T}"/>. Will not receive the benefit of any
        /// callbacks under these conditions, but the caller will enjoy a fresh instance
        /// of the Singleton.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator BidirectionalSingleton<T>(T value) => new BidirectionalSingleton<T>(value);
    }
}
