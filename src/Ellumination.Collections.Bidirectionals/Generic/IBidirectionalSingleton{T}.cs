using System;

namespace Ellumination.Collections.Generic
{
    /// <summary>
    /// Some Singleton concerns may also affect a Bidirectional relationship.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBidirectionalSingleton<T> : IBidirectionalSingleton
    {
        /// <summary>
        /// Gets or Sets the Value associated with the Singleton.
        /// </summary>
        /// <see cref="Get"/>
        /// <see cref="Set"/>
        T Value { get; set; }

        /// <summary>
        /// Gets the <see cref="Value"/> from the Singleton.
        /// </summary>
        /// <returns>Returns the actual Value corresponding to the property Get clause.</returns>
        T Get();

        /// <summary>
        /// Sets the <see cref="Value"/> of the Singleton.
        /// </summary>
        /// <param name="value">Should correspond to the Value property Set clause built-in variable.</param>
        void Set(T value);

        /// <summary>
        /// Event occurs prior to the <see cref="Value"/> Get being performed.
        /// </summary>
        event BidirectionalSingletonOnClause<T> BeforeGet;

        /// <summary>
        /// Event occurs after to the <see cref="Value"/> Set being performed.
        /// </summary>
        event BidirectionalSingletonOnClause<T> AfterSet;

        /// <summary>
        /// Event occurs prior to the <see cref="Value"/> Set being performed.
        /// </summary>
        event BidirectionalSingletonOnClause<T> BeforeSet;

        /// <summary>
        /// Event occurs when Evaluating the Value Getter prior to get.
        /// </summary>
        event TryBidirectionalSingletonOnGetClause<T> EvaluateGet;

        /// <summary>
        /// Event occurs when Evaluating the Value Setter prior to set.
        /// </summary>
        event TryBidirectionalSingletonOnSetClause<T> EvaluateSet;
    }
}
