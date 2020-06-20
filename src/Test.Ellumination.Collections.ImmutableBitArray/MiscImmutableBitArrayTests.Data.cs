using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using Calculators;
    using static DateTime;
    using static ImmutableBitArray;

    // ReSharper disable once UnusedMember.Global
    public partial class MiscImmutableBitArrayTests
    {
        private static Random Rnd { get; } = new Random((int) (UtcNow.Ticks % int.MaxValue));

        private static IEnumerable<byte> GetBytes(int count)
        {
            while (count-- > 0)
            {
                yield return (byte)(Rnd.Next() % byte.MaxValue);
            }
        }

        private static IEnumerable<object[]> _lengthCountData;

        public static IEnumerable<object[]> LengthCountData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(IEnumerable<byte> bytes, int lengthDelta)
                    {
                        yield return GetRange(bytes.ToArray());
                        yield return lengthDelta;
                    }

                    const byte maxByte = byte.MaxValue;

                    for (var byteCount = 0; byteCount < sizeof(uint); byteCount++)
                    {
                        /* TODO: TBD: furnishing Prime Numbers here is completely arbitrary; I
                         * just want something that is guaranteed not to coincide with a bit/byte
                         * boundary for test purposes. */

                        /* Poor man's range of Prime Numbers for test purposes. Could be any
                         * range, just as long as it exercises exceeding the width of the Bits in
                         * a Byte, and then some, for test purposes. */

                        // Additionally, Zero (no-change) is especially just as valid a thing to verify.
                        foreach (var lengthDelta in new PrimeNumberCollection(40))
                        {
                            /* Starting with some Random Test Cases.
                             * But let us also better verify that boundary bytes are masked correctly.*/
                            yield return GetOne(GetBytes(byteCount), lengthDelta).ToArray();
                            yield return GetOne(GetRange(new byte[byteCount].Select(_ => maxByte).ToArray()), lengthDelta).ToArray();

                            /* Bypass Deltas that would result in Exceptions thrown.
                             * This is captured by a separate unit test. */
                            if (byteCount * BitCount - lengthDelta < 0)
                            {
                                continue;
                            }

                            yield return GetOne(GetBytes(byteCount), -lengthDelta).ToArray();
                            yield return GetOne(GetRange(new byte[byteCount].Select(_ => maxByte).ToArray()), -lengthDelta).ToArray();
                        }
                    }
                }

                return _lengthCountData ?? (_lengthCountData = GetAll().ToArray());
            }
        }

        private static IEnumerable<object[]> _invalidLengthData;

        public static IEnumerable<object[]> InvalidLengthData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(int newLength)
                    {
                        yield return newLength;
                    }

                    yield return GetOne(-1).ToArray();
                }

                return _invalidLengthData ?? (_invalidLengthData = GetAll().ToArray());
            }
        }

        private static IEnumerable<object[]> _containsData;

        public static IEnumerable<object[]> ContainsData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    const byte maxByte = byte.MaxValue;

                    IEnumerable<object> GetOne(IEnumerable<byte> bytes, bool expectedItem)
                    {
                        bool ByteContainingExpectedItem(byte x)
                            => default(byte) != (byte) (maxByte & (expectedItem ? x : ~x));

                        var values = GetRange(bytes.ToArray());
                        yield return values;
                        yield return expectedItem;
                        yield return values.Any(ByteContainingExpectedItem);
                    }

                    const byte defaultByte = default(byte);

                    for (var byteCount = 0; byteCount < sizeof(uint); byteCount++)
                    {
                        foreach (var expectedItem in GetRange(true, false))
                        {
                            // Furnish several Random ones just for good measure.
                            yield return GetOne(GetBytes(byteCount), expectedItem).ToArray();
                            yield return GetOne(GetBytes(byteCount), expectedItem).ToArray();
                            yield return GetOne(GetBytes(byteCount), expectedItem).ToArray();

                            // Especially remember the edge use cases.
                            yield return GetOne(new byte[byteCount].Select(_ => defaultByte), expectedItem).ToArray();
                            yield return GetOne(new byte[byteCount].Select(_ => maxByte), expectedItem).ToArray();
                        }
                    }
                }

                return _containsData ?? (_containsData = GetAll().ToArray());
            }
        }

        private static IEnumerable<object[]> _copyToData;

        public static IEnumerable<object[]> CopyToData
        {
            get
            {
                // TODO: TBD: this was Contains data; change it up for Copyto purposes...
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(IEnumerable<byte> bytes, int desiredArrayLength, int bitArrayIndex)
                    {
                        yield return GetRange(bytes.ToArray());
                        yield return desiredArrayLength;
                        yield return bitArrayIndex;
                    }

                    // TODO: TBD: this one may be worthwhile scoping more broadly...
                    uint GetNext()
                    {
                        uint result;
                        do
                        {
                            result = (uint) Rnd.Next();
                        } while (result != 0 && 0 == ~result);

                        return result;
                    }

                    /* Allowing more than enough window extending beyond the range of a Bit Array.
                     * This is pretty exhaustive, I think. We likely do not need to perform half
                     * as many as we do here. */

                    var primeNumbers = new PrimeNumberCollection(sizeof(uint) * BitCount * 2).ToArray();

                    for (var byteCount = 0; byteCount < sizeof(uint); byteCount++)
                    {
                        var maxCount = byteCount;

                        /* Any Array Length should be fine. Not all are necessarily filled,
                         * and the test itself makes some informed Overlapping decisions. */

                        foreach (var desiredArrayLength in primeNumbers)
                        {
                            /* We are not here, however, to violate the upper edge of the array.
                             We leave that for another unit test to exercise those conditions. */

                            foreach (var bitArrayIndex in primeNumbers.Where(prime => prime < maxCount * BitCount))
                            {
                                // TODO: TBD: "breaking" parameters are captured by other test cases.
                                // TODO: TBD: will need to refine this around the criteria for working CopyTo parameters
                                var next = GetEndianAwareBytes(GetNext()).Take(byteCount);
                                yield return GetOne(next, desiredArrayLength, bitArrayIndex).ToArray();
                            }
                        }
                    }
                }

                return _copyToData ?? (_copyToData = GetAll().ToArray());
            }
        }

        private static IEnumerable<object[]> _invalidCopyToArrayIndexData;

        public static IEnumerable<object[]> InvalidCopyToArrayIndexData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(int arrayIndex)
                    {
                        yield return arrayIndex;
                    }

                    /* We could get more elaborate with actual Bytes for Bit Array content,
                     * but this is perfectly sufficient to the task at hand. */
                    yield return GetOne(-1).ToArray();

                    /* Zero works here due to the fact we are testing against a Zero Length
                     Bit Array. Basically, no internal Byte elements whatsoever, so even a
                     Zero index works. */
                    yield return GetOne(0).ToArray();
                }

                return _invalidCopyToArrayIndexData  ?? (_invalidCopyToArrayIndexData = GetAll().ToArray());
            }
        }
    }
}
