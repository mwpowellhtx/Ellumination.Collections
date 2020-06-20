using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Keyed.Flags
{
    using static Math;

    public abstract partial class Enumeration<T>
    {
        /// <summary>
        /// Returns the BitwiseNot value from this instance.
        /// </summary>
        /// <returns></returns>
        /// <remarks>The only risk of supporting the bitwise not operator is to ensure that the
        /// bit masks are ubiquitous for a given set of enumerated values. Their lengths should
        /// all agree for this to make any sense at all.</remarks>
        protected virtual T BitwiseNot()
        {
            // TODO: be careful with this one: converting to/from bytes is causing extra bits to be tacked on
            // TODO: which, when twos complemented, is actually inverting a whole nibble improperly
            // TODO: actually, I'm not sure how the lengths are ending up like they are to begin with

            // TODO: TBD: are we getting the value that we want? or are we getting a new one?
            // Be cautious that these are not operating in place.
            var result = FromBytes(Key.ToBytes(false).ToArray());
            // Ensure that any extra padding that got tacked on is truncated.
            result.Key = Key.Not();
            return result;
        }

        /// <summary>
        /// Binary Calculator Callback involving <paramref name="a"/> and <paramref name="b"/> Operands.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected internal delegate ImmutableBitArray BinaryCalculatorCallback(ImmutableBitArray a, ImmutableBitArray b);

        /// <summary>
        /// Facilitates the Binary <paramref name="callback"/> involving the
        /// <paramref name="root"/> and <paramref name="other"/> Operands.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="other"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        /// <see cref="BinaryCalculatorCallback"/>
        private static T BitwiseFunc(T root, T other, BinaryCalculatorCallback callback)
        {
            // TODO: lengths may be an issue here ...
            var length = Min(root.Key.Length, other.Key.Length);
            var funced = callback.Invoke(root.Key, other.Key);
            var result = FromBytes(funced.ToBytes(false).ToArray());
            result.Key.Length = length;
            return result;
        }

        /// <summary>
        /// Returns the Bitwise Or value from this instance combined with the <paramref name="other"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        /// <see cref="BitwiseFunc"/>
        protected internal virtual T BitwiseOr(T other) => BitwiseFunc(this as T, other, (r, o) => r.Or(o));

        /// <summary>
        /// Returns the Bitwise And value from this instance combined with the <paramref name="other"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        /// <see cref="BitwiseFunc"/>
        protected internal virtual T BitwiseAnd(T other) => BitwiseFunc(this as T, other, (r, o) => r.And(o));

        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// Returns the Bitwise Exclusive Or value from this instance combined with  the <paramref name="other"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        protected internal virtual T BitwiseXor(T other) => BitwiseFunc(this as T, other, (r, o) => r.Xor(o));

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> representing the Bitwise Decomposition of the Current Instance.
        /// </summary>
        /// <see cref="KeyLookup"/>
        /// <see cref="SetKeyBitCount"/>
        public override IEnumerable<T> EnumeratedValues
        {
            get
            {
                {
                    var lookup = KeyLookup;

                    /* This is the early return use case. Do not need to iterate the rest if we have
                     * this. Looking the value up is key, but so is ensuring this really has only one
                     * bit set. Otherwise, we may just look it up. */

                    if (SetKeyBitCount <= 1 && lookup.TryGetValue((T) this, out var result))
                    {
                        yield return result;
                        yield break;
                    }
                }

                // Otherwise we need to dive into the rest.
                foreach (var y in Values.Where(x => x.SetKeyBitCount >= 1 && x.Equals(BitwiseAnd(x))))
                {
                    yield return y;
                }
            }
        }
    }
}
