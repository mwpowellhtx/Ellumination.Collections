using System;
using System.Collections.Generic;

namespace Ellumination.Collections
{
    public partial class ImmutableBitArray
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<bool> GetBits() => GetBooleans(_bytes, Length);

        /// <summary>
        /// Returns whether the Bit at <paramref name="index"/> is Set to <value>true</value>.
        /// The default behavior involves no <see cref="Elasticity"/>.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <remarks>The default behavior involves no <see cref="Elasticity"/>.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">Occurs when <paramref name="index"/>
        /// is Less Than Zero or Greater Than or Equal to <see cref="Length"/>.</exception>
        /// <inheritdoc />
        public bool Get(int index)
        {
            // ReSharper disable once InconsistentNaming
            var this_Length = Length;

            if (index < 0 || index >= this_Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), index
                    , $"'{typeof(ImmutableBitArray).FullName}.{nameof(Get)}'"
                      + $" argument '{nameof(index)}' value '{index}' out of range.");
            }

            return ListFunc(b => b[index / 8] & (1 << (index % 8))) != 0;
        }

        // TODO: TBD: ditto Elastic Set / capture this interface as well...
        /// <inheritdoc />
        public bool Get(int index, Elasticity elasticity)
            // TODO: TBD: what to do in the use case for Expansion ...
            => (elasticity.Contains(Elasticity.Expansion) || index < Length) && Get(index);

        /// <summary>
        /// Sets the Bit at <paramref name="index"/> to <paramref name="value"/>.
        /// The default behavior involves no <see cref="Elasticity"/>.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <remarks>The default behavior involves no <see cref="Elasticity"/>.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is
        /// Less Than Zero or Greater Than Or Equal To <see cref="Length"/>.</exception>
        /// <inheritdoc />
        public void Set(int index, bool value)
        {
            if (index < 0 || index >= Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index)
                    , index, $"Argument '{nameof(index)}' value '{index}' out of range");
            }

            /* We do not actually need the Functional result.
             * This is just a convenience method given the approach we are taking here. */
            ListFunc(b =>
            {
                var mask = (byte)(1 << (index % 8));
                return value ? (b[index / 8] |= mask) : (b[index / 8] &= (byte)~mask);
            });
        }

        // TODO: TBD: consolidate the Set interface to the Elasticity one with default Elasticity...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="elasticity"></param>
        /// <inheritdoc />
        public void Set(int index, bool value, Elasticity elasticity)
        {
            if (elasticity.Contains(Elasticity.Expansion)
                && index >= Length
                && _bytes.Count < index / BitCount + 1)
            {
                _bytes.AddRange(new byte[index / BitCount + 1]);
                // TODO: TBD: or simply pass it through Length ?
                _length = index + 1;
            }

            Set(index, value);
        }

        /// <inheritdoc />
        public void SetAll(bool value)
            => ListAction(a =>
            {
                var len = Length;

                // Set all but the Last Byte.
                for (var i = 0; i < len - 1; i++)
                {
                    a[i] = value ? byte.MaxValue : byte.MinValue;
                }

                ListFunc(b =>
                {
                    var mask = MakeMask(0, len % 8);
                    return value ? (b[len / 8] |= mask) : (b[len / 8] &= (byte)~mask);
                });
            });

        /// <inheritdoc />
        /// <see cref="Get(int)"/>
        /// <see cref="Set(int,bool)"/>
        public bool this[int index]
        {
            get => Get(index);
            set => Set(index, value);
        }
    }
}
