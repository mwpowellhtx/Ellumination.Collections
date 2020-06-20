using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    public partial class ImmutableBitArray
    {
        private delegate byte CalculateBinaryCallback(byte x, byte y);

        private static IEnumerable<byte> CalculateBinaryOperator(IEnumerable<byte> a
            , IEnumerable<byte> b, CalculateBinaryCallback calc)
        {
            // Ensuring that A and B are both the same size.
            void NormalizeArray(IEnumerable<byte> x, ref IEnumerable<byte> y)
            {
                // ReSharper disable once PossibleMultipleEnumeration
                if (x.Count()
                    // ReSharper disable once PossibleMultipleEnumeration
                    > y.Count())
                {
                    // ReSharper disable once PossibleMultipleEnumeration
                    y = y.Concat(new byte[
                        // ReSharper disable once PossibleMultipleEnumeration
                        x.Count()
                        // ReSharper disable once PossibleMultipleEnumeration
                        - y.Count()]).ToArray();
                }
            }

            // ReSharper disable once PossibleMultipleEnumeration
            NormalizeArray(a, ref b);
            // ReSharper disable once PossibleMultipleEnumeration
            NormalizeArray(b
                // ReSharper disable once PossibleMultipleEnumeration
                , ref a);

            // Then simply Zip the two collections and apply the Calculation.
            // ReSharper disable once PossibleMultipleEnumeration
            return a.Zip(b, (x, y) => calc(x, y)).ToArray();
        }

        /// <inheritdoc />
        public ImmutableBitArray And(ImmutableBitArray other) => And(GetRange(other));

        /// <inheritdoc />
        public ImmutableBitArray Or(ImmutableBitArray other) => Or(GetRange(other));

        /// <inheritdoc />
        public ImmutableBitArray Xor(ImmutableBitArray other) => Xor(GetRange(other));

        /// <summary>
        /// Performs the Bitwise And operation on the current instance, <paramref name="other"/>,
        /// and <paramref name="others"/> instances. Returns a new instance containing the result.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="others"></param>
        /// <returns>A new instance containing the result.</returns>
        public ImmutableBitArray And(ImmutableBitArray other
            , params ImmutableBitArray[] others)
            => And(GetRange(other).Concat(others).ToArray());

        /// <summary>
        /// Performs the Bitwise Or operation on the current instance, <paramref name="other"/>,
        /// and <paramref name="others"/> instances. Returns a new instance containing the result.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="others"></param>
        /// <returns>A new instance containing the result.</returns>
        public ImmutableBitArray Or(ImmutableBitArray other
            , params ImmutableBitArray[] others)
            => Or(GetRange(other).Concat(others).ToArray());

        /// <summary>
        /// Performs the Bitwise Exclusive Or operation on the current instance,
        /// <paramref name="other"/>, and <paramref name="others"/> instances. Returns a new
        /// instance containing the result.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="others"></param>
        /// <returns>A new instance containing the result.</returns>
        public ImmutableBitArray Xor(ImmutableBitArray other
            , params ImmutableBitArray[] others)
            => Xor(GetRange(other).Concat(others).ToArray());

        /// <inheritdoc />
        public ImmutableBitArray And(IEnumerable<ImmutableBitArray> others)
        {
            var bytes = others.Aggregate(_bytes.ToArray()
                , (g, other) => CalculateBinaryOperator(
                    g, other._bytes, (x, y) => (byte) (x & y)).ToArray());

            return new ImmutableBitArray(bytes);
        }

        /// <inheritdoc />
        public ImmutableBitArray Or(IEnumerable<ImmutableBitArray> others)
        {
            var bytes = others.Aggregate(_bytes.ToArray()
                , (g, other) => CalculateBinaryOperator(
                    g, other._bytes, (x, y) => (byte) (x | y)).ToArray());

            return new ImmutableBitArray(bytes);
        }

        /// <inheritdoc />
        public ImmutableBitArray Xor(IEnumerable<ImmutableBitArray> others)
        {
            var bytes = others.Aggregate(_bytes.ToArray()
                , (g, other) => CalculateBinaryOperator(
                    g, other._bytes, (x, y) => (byte) (x ^ y)).ToArray());

            return new ImmutableBitArray(bytes);
        }

        /// <inheritdoc />
        public ImmutableBitArray Not() => new ImmutableBitArray(_bytes.Select(x => (byte) ~x));
    }
}
