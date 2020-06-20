using System;

namespace Ellumination.Collections
{
    using static Math;

    public partial class ImmutableBitArray
    {
        /// <summary>
        /// <see cref="Length"/> backing field.
        /// </summary>
        /// <see cref="Length"/>
        private int _length;

        /* TODO: TBD: for now, Length responds with Elasticity (default); future direction,
         * Elasticity should be a property of the bit array itself, which would potentially
         * constrain the behavior accordingly as to whether Contraction/Expansion is permitted. */

        /// <summary>
        /// Gets or Sets the Length of the BitArray.
        /// Getting the Length refers to <see cref="Count"/>.
        /// Setting the Length sets the actual length to the nearest <see cref="byte"/>.
        /// </summary>
        /// <inheritdoc />
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <see cref="Max(byte,byte)"/>
        /// <see cref="MakeMask(int,int)"/>
        public int Length
        {
            get => _length;
            set
            {
                // Do nothing use case.
                if (value == _length)
                {
                    return;
                }

                // Values Less Than Zero should Never Ever be allowed past this point.
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value)
                        , value, $"Negative value '{value}' is invalid");
                }

                // In any event we want the DesiredCount.
                var desiredCount = value / BitCount + Min(1, value % BitCount);

                /* Either Iterates Removing the Undesired Bytes,
                 or Adds a Range of Bytes to the end wholesale. */

                while (_bytes.Count != desiredCount)
                {
                    if (_bytes.Count < desiredCount)
                    {
                        _bytes.AddRange(new byte[desiredCount - _bytes.Count]);
                    }
                    else if (_bytes.Count > desiredCount)
                    {
                        _bytes.RemoveAt(_bytes.Count - 1);
                    }
                }

                /* Only in the event we have a Mid-Byte Length do we shave any undesired Bits
                 * from the Byte. Should work regardless whether Expanding or Contracting. */
                if (value % BitCount is int bitIndex && bitIndex != 0)
                {
                    _bytes[desiredCount - 1] = (byte)(_bytes[desiredCount - 1] & MakeMask(0, bitIndex - 1));
                }

                // Then assign the Length after we have our ducks in a row.
                _length = value;
            }
        }

        /// <summary>
        /// Gets the actual <see cref="Length"/>. This needs to be precise, otherwise we end up
        /// with false positives in the direction of the MSB.
        /// </summary>
        /// <inheritdoc />
        public int Count => Length;
    }
}
