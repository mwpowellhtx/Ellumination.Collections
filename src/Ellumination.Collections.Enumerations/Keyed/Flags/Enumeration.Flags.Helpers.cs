using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ellumination.Collections.Keyed.Flags
{
    using static Type;
    using static BindingFlags;

    public abstract partial class Enumeration<T>
    {
        /// <summary>
        /// Initializes the <see cref="Enumeration{TKey}.Key" /> in the <paramref name="values"/>.
        /// Assumes that the intended order of initialization has been resolved approaching a call
        /// to this helper method.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="value"></param>
        /// <param name="shiftCount"></param>
        protected static void InitializeKeys(IEnumerable<T> values, uint value = 0x1, int shiftCount = 0)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            const Elasticity expansion = Elasticity.Expansion;

            var starting = ImmutableBitArray.FromInts(new[] {value}).ShiftLeft(shiftCount, expansion);

            values.ToList().Aggregate(starting, (key, x) =>
            {
                x.Key = key;
                return key.ShiftLeft(elasticity: expansion);
            });
        }

        /// <summary>
        /// Call ths method during the derived constructor when there is any question
        /// as to the consistency of the <see cref="ImmutableBitArray.Length"/> of the
        /// <see cref="Enumeration{TKey}.Key"/>. The <paramref name="values"/>
        /// collection is not necessarily the same one as was passed into the
        /// <see cref="InitializeKeys"/> method.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="length">Default length is 8 times 32, or 0x100, or 256 bits.</param>
        /// <remarks>This method cannot be called from the scope of this class static constructor
        /// due to the fact that derived enumerated fields will not have been initialized at that
        /// moment. Therefore, this is the next best thing to helping ourselves out.</remarks>
        /// <see cref="BitsPerByte"/>
        protected static void InitializeKeyLengths(IEnumerable<T> values, int length = BitsPerByte * 32)
        {
            // ReSharper disable PossibleMultipleEnumeration
            if (values == null || !values.Any())
            {
                throw new ArgumentNullException(nameof(values));
            }

            var valuesList = values.ToList();
            var firstValue = values.First();

            if (firstValue.Key.Length < length && !valuesList.Any(x => x.Key.Length > length))
            {
                firstValue.Key.Length = length;
            }

            var maxLength = values.Max(x => x.Key.Length);

            valuesList.ForEach(x => x.Key.Length = maxLength);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Returns a new instance generated from the <typeparamref name="T"/> Bytes
        /// constructor. Whether the constructor is used directly or not, it is required
        /// to facilitate complete operation of the bitwise enumeration.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static T FromBytesCtor(IEnumerable bytes)
        {
            var derivedType = typeof(T);
            const BindingFlags flags = Instance | NonPublic;
            //TODO: might want to break this out into an extension method
            var ctor = derivedType.GetConstructor(flags, DefaultBinder, new[] { typeof(byte[]) }, null);
            var instance = ctor?.Invoke(new object[] {bytes});
            return instance as T;
        }

        /// <summary>
        /// Returns the <typeparamref name="T"/> derived instance corresponding to the
        /// <paramref name="bits"/>.
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        /// <see cref="FromBytesCtor"/>
        public static T FromBitArray(ImmutableBitArray bits)
        {
            // This seems a bit redundant since the value is its own key...
            var @default = FromBytesCtor(bits.ToBytes(false).ToArray());

            //TODO: dictionary would be preferred, but for some reason it's not "catching" on the correct values...
            // We either want the enumerated value itself or the factory created version.
            var result = Values.FirstOrDefault(x => x.Equals(@default));

            return result ?? @default;
        }

        /// <summary>
        /// Returns the <typeparamref name="T"/> instance derived from the <paramref name="bytes"/>.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        /// <see cref="FromBitArray"/>
        public static T FromBytes(byte[] bytes)
        {
            // Rule out exceptional conditions up front.
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes), $"Bytes values are required for an {typeof(T)}");
            }

            if (!bytes.Any())
            {
                throw new ArgumentException($"Bytes values are required for an {typeof(T)}", nameof(bytes));
            }

            // Make sure that the bytes are dealt with in LSB order.
            return FromBitArray(ImmutableBitArray.FromBytes(bytes, false));
        }

        /// <summary>
        /// Gets a transient <see cref="IDictionary{TKey,TValue}"/> instance Keyed
        /// on the Flags Enumerated Value itself.
        /// </summary>
        private static IDictionary<T, T> KeyLookup => Values.ToDictionary(x => x);
    }
}
