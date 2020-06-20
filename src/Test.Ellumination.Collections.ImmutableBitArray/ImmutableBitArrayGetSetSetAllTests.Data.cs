using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using static ImmutableBitArray;

    public partial class ImmutableBitArrayGetSetSetAllTests
    {
        private static IEnumerable<object[]> _inelasticXetData;

        /// <summary>
        /// Provides an exhaustive set of bits set to either <value>true</value> or
        /// <value>false</value>. We name the Data X-Ray accordingly due to the fact
        /// that it applies for both Get as well as Set unit testing.
        /// </summary>
        public static IEnumerable<object[]> InelasticXetData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(uint value, int i)
                    {
                        yield return value;
                        yield return i;
                        yield return (value & (1 << i)) != 0;
                    }

                    const int bitCount = sizeof(uint) * BitCount;

                    uint current = 1;

                    // Exhaustively determine that we can Index each of the Positions.
                    for (var i = 0; i < bitCount; i++, current <<= 1)
                    {
                        yield return GetOne(0, i).ToArray();
                        yield return GetOne(current, i).ToArray();
                    }
                }

                return _inelasticXetData ?? (_inelasticXetData = GetAll());
            }
        }

        private static IEnumerable<object[]> _badInelasticXetData;

        /// <summary>
        /// Represents a set of Bad Inelastic Data. We name the Data X-Ray accordingly
        /// due to the fact that it applies for both Get as well as Set unit testing.
        /// </summary>
        public static IEnumerable<object[]> BadInelasticXetData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(uint value, int i)
                    {
                        yield return value;
                        yield return i;
                    }

                    const int bitCount = sizeof(uint) * BitCount;

                    for (var i = 0; i < 3; i++)
                    {
                        if (i > 0)
                        {
                            yield return GetOne(0, -i).ToArray();
                        }

                        yield return GetOne(0, bitCount + i).ToArray();
                    }
                }

                return _badInelasticXetData ?? (_badInelasticXetData = GetAll());
            }
        }
    }
}
