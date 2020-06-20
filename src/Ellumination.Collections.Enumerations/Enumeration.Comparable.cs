using System;

namespace Ellumination.Collections
{
    using static Enumeration.CompareToResults;

    public abstract partial class Enumeration
        : IComparable<Enumeration>
            , IEquatable<Enumeration>
    {
        /// <summary>
        /// 
        /// </summary>
        internal static class CompareToResults
        {
            internal static int Lt => LessThan;

            internal static int Gt => GreaterThan;

            internal static int Et => EqualTo;

            /// <summary>
            /// -1
            /// </summary>
            internal const int LessThan = -1;

            /// <summary>
            /// 1
            /// </summary>
            internal const int GreaterThan = 1;

            /// <summary>
            /// 0
            /// </summary>
            internal const int EqualTo = 0;
        }

        /// <summary>
        /// Returns the result after Comparing <paramref name="a"/>
        /// and <paramref name="b"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected virtual int CompareTo(string a, string b)
            => a != null && b == null
                ? Gt
                : a == null && b != null
                    ? Lt
                    : a == null
                        ? Lt
                        : string.CompareOrdinal(a.ToLower(), b.ToLower());

        /// <summary>
        /// Returns the result after Comparing <paramref name="a"/>
        /// and <paramref name="b"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected virtual int CompareTo(Enumeration a, Enumeration b)
            => a != null && b == null
                ? Gt
                : a == null && b != null
                    ? Lt
                    : a == null
                        ? Lt
                        : a.GetType() != b.GetType()
                            ? Lt
                            : CompareTo(a.Name, b.Name);

        /// <inheritdoc />
        public virtual int CompareTo(Enumeration other) => CompareTo(this, other);

        /// <inheritdoc />
        public virtual bool Equals(Enumeration other) => CompareTo(this, other) == Et;

        //// TODO: TBD: finding a home for logical operators?
        ///// <summary>
        ///// Returns whether this object is GreaterThan an <paramref name="other"/> one.
        ///// </summary>
        ///// <param name="other"></param>
        ///// <returns></returns>
        //protected bool GreaterThan(TDerived other) => CompareTo((TDerived)this, other) > 0;

        ///// <summary>
        ///// Returns whether this object is LessThan an <paramref name="other"/> one.
        ///// </summary>
        ///// <param name="other"></param>
        ///// <returns></returns>
        //protected bool LessThan(TDerived other) => CompareTo((TDerived)this, other) < 0;

        ///// <summary>
        ///// Returns whether this object is GreaterThanOrEqual an <paramref name="other"/> one.
        ///// </summary>
        ///// <param name="other"></param>
        ///// <returns></returns>
        //protected bool GreaterThanOrEqual(TDerived other) => CompareTo((TDerived)this, other) >= 0;

        ///// <summary>
        ///// Returns whether this object is LessThanOrEqual an <paramref name="other"/> one.
        ///// </summary>
        ///// <param name="other"></param>
        ///// <returns></returns>
        //protected bool LessThanOrEqual(TDerived other) => CompareTo((TDerived)this, other) <= 0;
    }
}
