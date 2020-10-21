namespace Ellumination.Collections.Generic
{
    /// <summary>
    /// Provides a handful of useful Singleton extension methods.
    /// </summary>
    /// <see cref="BidirectionalSingleton{T}"/>
    public static class SingletonExtensionMethods
    {
        /// <summary>
        /// Returns an instance of the Singleton given several of its value and callbacks.
        /// </summary>
        /// <param name="value">The <see cref="BidirectionalSingleton{T}.Value"/> under consideration.</param>
        public static BidirectionalSingleton<T> ToBidirectionalSingleton<T>(this T value) =>
            new BidirectionalSingleton<T>(value);

        /// <summary>
        /// Returns an instance of the Singleton given several of its value and callbacks
        /// from the perspective of any root <typeparamref name="R"/> instance.
        /// </summary>
        /// <param name="_">From the perspective of any Root anchor.</param>
        /// <param name="onBeforeGet">Callback occurs On <see cref="BidirectionalSingleton{T}.BeforeGet"/>.</param>
        /// <param name="onEvaluateGet">Callback occurs On <see cref="BidirectionalSingleton{T}.EvaluateGet"/>.</param>
        public static BidirectionalSingleton<T> ToBidirectionalSingleton<R, T>(this R _
            , BidirectionalSingletonOnClause<T> onBeforeGet
            , TryBidirectionalSingletonOnGetClause<T> onEvaluateGet) =>
            new BidirectionalSingleton<T>(onBeforeGet, onEvaluateGet);

        /// <summary>
        /// Returns an instance of the Singleton given several of its value and callbacks.
        /// </summary>
        /// <param name="value">The <see cref="BidirectionalSingleton{T}.Value"/> under consideration.</param>
        /// <param name="onBeforeGet">Callback occurs On <see cref="BidirectionalSingleton{T}.BeforeGet"/>.</param>
        /// <param name="onEvaluateGet">Callback occurs On <see cref="BidirectionalSingleton{T}.EvaluateGet"/>.</param>
        public static BidirectionalSingleton<T> ToBidirectionalSingleton<T>(this T value
            , BidirectionalSingletonOnClause<T> onBeforeGet
            , TryBidirectionalSingletonOnGetClause<T> onEvaluateGet) =>
            new BidirectionalSingleton<T>(value, onBeforeGet, onEvaluateGet);

        /// <summary>
        /// Returns an instance of the Singleton given several of its value and callbacks
        /// from the perspective of any root <typeparamref name="R"/> instance.
        /// </summary>
        /// <param name="_">From the perspective of any Root anchor.</param>
        /// <param name="onBeforeSet">Callback occurs On <see cref="BidirectionalSingleton{T}.BeforeSet"/>.</param>
        /// <param name="onAfterSet">Callback occurs On <see cref="BidirectionalSingleton{T}.AfterSet"/>.</param>
        /// <param name="onEvaluateSet">Callback occurs On <see cref="BidirectionalSingleton{T}.EvaluateSet"/>.</param>
        public static BidirectionalSingleton<T> ToBidirectionalSingleton<R, T>(this R _
            , BidirectionalSingletonOnClause<T> onBeforeSet
            , BidirectionalSingletonOnClause<T> onAfterSet
            , TryBidirectionalSingletonOnSetClause<T> onEvaluateSet) =>
            new BidirectionalSingleton<T>(onBeforeSet, onAfterSet, onEvaluateSet);

        /// <summary>
        /// Returns an instance of the Singleton given several of its value and callbacks.
        /// </summary>
        /// <param name="value">The <see cref="BidirectionalSingleton{T}.Value"/> under consideration.</param>
        /// <param name="onBeforeSet">Callback occurs On <see cref="BidirectionalSingleton{T}.BeforeSet"/>.</param>
        /// <param name="onAfterSet">Callback occurs On <see cref="BidirectionalSingleton{T}.AfterSet"/>.</param>
        /// <param name="onEvaluateSet">Callback occurs On <see cref="BidirectionalSingleton{T}.EvaluateSet"/>.</param>
        public static BidirectionalSingleton<T> ToBidirectionalSingleton<T>(this T value
            , BidirectionalSingletonOnClause<T> onBeforeSet
            , BidirectionalSingletonOnClause<T> onAfterSet
            , TryBidirectionalSingletonOnSetClause<T> onEvaluateSet) =>
            new BidirectionalSingleton<T>(value, onBeforeSet, onAfterSet, onEvaluateSet);

        /// <summary>
        /// Returns an instance of the Singleton given several of its value and callbacks
        /// from the perspective of any root <typeparamref name="R"/> instance.
        /// </summary>
        /// <param name="_">From the perspective of any Root anchor.</param>
        /// <param name="onBeforeGet">Callback occurs On <see cref="BidirectionalSingleton{T}.BeforeGet"/>.</param>
        /// <param name="onBeforeSet">Callback occurs On <see cref="BidirectionalSingleton{T}.BeforeSet"/>.</param>
        /// <param name="onAfterSet">Callback occurs On <see cref="BidirectionalSingleton{T}.AfterSet"/>.</param>
        /// <param name="onEvaluateGet">Callback occurs On <see cref="BidirectionalSingleton{T}.EvaluateGet"/>.</param>
        /// <param name="onEvaluateSet">Callback occurs On <see cref="BidirectionalSingleton{T}.EvaluateSet"/>.</param>
        public static BidirectionalSingleton<T> ToBidirectionalSingleton<R, T>(this R _
            , BidirectionalSingletonOnClause<T> onBeforeGet
            , BidirectionalSingletonOnClause<T> onBeforeSet
            , BidirectionalSingletonOnClause<T> onAfterSet
            , TryBidirectionalSingletonOnGetClause<T> onEvaluateGet
            , TryBidirectionalSingletonOnSetClause<T> onEvaluateSet) =>
            new BidirectionalSingleton<T>(onBeforeGet, onBeforeSet, onAfterSet, onEvaluateGet, onEvaluateSet);

        /// <summary>
        /// Returns an instance of the Singleton given several of its value and callbacks.
        /// </summary>
        /// <param name="value">The <see cref="BidirectionalSingleton{T}.Value"/> under consideration.</param>
        /// <param name="onBeforeGet">Callback occurs On <see cref="BidirectionalSingleton{T}.BeforeGet"/>.</param>
        /// <param name="onBeforeSet">Callback occurs On <see cref="BidirectionalSingleton{T}.BeforeSet"/>.</param>
        /// <param name="onAfterSet">Callback occurs On <see cref="BidirectionalSingleton{T}.AfterSet"/>.</param>
        /// <param name="onEvaluateGet">Callback occurs On <see cref="BidirectionalSingleton{T}.EvaluateGet"/>.</param>
        /// <param name="onEvaluateSet">Callback occurs On <see cref="BidirectionalSingleton{T}.EvaluateSet"/>.</param>
        public static BidirectionalSingleton<T> ToBidirectionalSingleton<T>(this T value
            , BidirectionalSingletonOnClause<T> onBeforeGet
            , BidirectionalSingletonOnClause<T> onBeforeSet
            , BidirectionalSingletonOnClause<T> onAfterSet
            , TryBidirectionalSingletonOnGetClause<T> onEvaluateGet
            , TryBidirectionalSingletonOnSetClause<T> onEvaluateSet) =>
            new BidirectionalSingleton<T>(value, onBeforeGet, onBeforeSet, onAfterSet, onEvaluateGet, onEvaluateSet);
    }
}
