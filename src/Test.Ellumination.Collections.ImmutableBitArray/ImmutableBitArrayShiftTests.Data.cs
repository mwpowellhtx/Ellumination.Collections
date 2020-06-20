using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using Calculators;
    using Xunit;
    using static Math;
    using static ImmutableBitArray;
    using static Elasticity;

    public partial class ImmutableBitArrayShiftTests
    {
        //private static IEnumerable<object[]> _boilerplateData;

        //public static IEnumerable<object[]> BoilerplateData
        //{
        //    get
        //    {
        //        IEnumerable<object[]> GetAll()
        //        {
        //            IEnumerable<object> GetOne()
        //            {
        //                yield break;
        //            }

        //            yield break;
        //        }

        //        return _boilerplateData ?? (_boilerplateData = GetAll().ToArray());
        //    }
        //}

        private static IEnumerable<object[]> _invalidStartIndexData;

        public static IEnumerable<object[]> InvalidStartIndexData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(int startIndex)
                    {
                        yield return GetRange(GetEndianAwareBytes(default(uint)).ToArray());
                        yield return startIndex;
                    }

                    // In this case virtually any StartIndex will do, provided it is Out Of Range.
                    foreach (var startIndex in GetRange(-1, sizeof(uint) * BitCount))
                    {
                        yield return GetOne(startIndex).ToArray();
                    }
                }

                return _invalidStartIndexData ?? (_invalidStartIndexData = GetAll().ToArray());
            }
        }

        private static IEnumerable<object[]> _invalidCountData;

        public static IEnumerable<object[]> InvalidCountData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(int startIndex)
                    {
                        yield return GetRange(GetEndianAwareBytes(default(uint)).ToArray());
                        yield return startIndex;
                        // Basically Count always Less Than Zero.
                        const int count = -1;
                        yield return count;
                    }

                    for (var startIndex = 0; startIndex < sizeof(uint) * BitCount; startIndex++)
                    {
                        yield return GetOne(startIndex).ToArray();
                    }
                }

                return _invalidCountData ?? (_invalidCountData = GetAll().ToArray());
            }
        }

        private static uint MakeMask(int iShift, int jShift)
        {
            var mask = 0u;

            for (var shift = iShift; shift <= jShift; shift++)
            {
                mask |= 1u << shift;
            }

            return mask;
        }

        private static IEnumerable<Elasticity?> Elasticities
            => GetRange<Elasticity?>(null, None, Contraction, Expansion, Both);

        private static IEnumerable<object[]> _shiftLeftCorrectData;

        public static IEnumerable<object[]> ShiftLeftCorrectData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    const int bitCount = sizeof(uint) * BitCount;

                    IEnumerable<object> GetOne(uint value, uint expected, int startIndex
                        , int count, Elasticity? elasticity = None)
                    {
                        yield return GetRange(GetEndianAwareBytes(value).ToArray());
                        yield return GetRange(GetEndianAwareBytes(expected).ToArray());
                        yield return startIndex;
                        yield return count;
                        yield return elasticity;
                        yield return bitCount + (elasticity.Contains(Expansion) ? count : 0);
                    }

                    var primeNumbers = new PrimeNumberCollection(100).ToArray();

                    /* Apparently, it is too much to ask for the language level to shift by some arbitrary number.
                     * http://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/right-shift-operator
                     * So, we must help the issue out a little. While there is Count remaining, do some Shifting. */
                    uint ShiftLeftWhileCount(uint value, int count)
                    {
                        const int maxShift = 0x1f;
                        while (count > 0)
                        {
                            value <<= Min(count, maxShift);
                            count -= maxShift;
                        }

                        return value;
                    }

                    // TODO: TBD: for now concerned with the inelastic use case... fill in the gaps later
                    foreach (var shift in primeNumbers.Where(x => x < bitCount))
                    {
                        var x = 1u << shift;

                        foreach (var startIndex in primeNumbers.Where(prime => prime < bitCount))
                        {
                            var startMask = startIndex == 0 ? 0 : MakeMask(0, startIndex - 1);

                            foreach (var count in primeNumbers)
                            {
                                /* Note that not all Expected Values can be practically determined, for the same reason
                                 that accounting for the bits shifting mid-element is a daunting problem to approach.
                                 The best we do is approach this by jumping into IEnumerable<bool> territory, since,
                                 practically speaking, this is our worst case anyway. In fact, if we tried to operate
                                 at the element level, that would be a far worse use case. */

                                // So... When Elasticity is practical to work with, then we can ostensibly know Expected.
                                var expected = (ShiftLeftWhileCount(x & ~startMask, count) & ~startMask)
                                               | (x & startMask);

                                foreach (var elasticity in Elasticities)
                                {
                                    yield return GetOne(x, expected, startIndex, count, elasticity).ToArray();
                                }
                            }
                        }
                    }
                }

                return _shiftLeftCorrectData ?? (_shiftLeftCorrectData = GetAll().ToArray());
            }
        }

        private static IEnumerable<object[]> _shiftRightCorrectData;

        public static IEnumerable<object[]> ShiftRightCorrectData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    const int bitCount = sizeof(uint) * BitCount;

                    IEnumerable<object> GetOne(uint value, uint expected, int startIndex
                        , int count, Elasticity? elasticity = None)
                    {
                        yield return GetRange(GetEndianAwareBytes(value).ToArray());
                        yield return GetRange(GetEndianAwareBytes(expected).ToArray());
                        yield return startIndex;
                        yield return count;
                        yield return elasticity;
                        yield return elasticity.Contains(Contraction)
                            ? startIndex + Max(0, bitCount - startIndex - count)
                            : bitCount;
                    }

                    var primeNumbers = new PrimeNumberCollection(100).ToArray();

                    /* Apparently, it is too much to ask for the language level to shift by some arbitrary number.
                     * http://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/right-shift-operator
                     * So, we must help the issue out a little. While there is Count remaining, do some Shifting. */
                    uint ShiftRightWhileCount(uint value, int count)
                    {
                        const int maxShift = 0x1f;
                        while (count > 0)
                        {
                            value >>= Min(count, maxShift);
                            count -= maxShift;
                        }

                        return value;
                    }

                    // TODO: TBD: for now concerned with the inelastic use case... fill in the gaps later
                    foreach (var shift in primeNumbers.Where(x => x < bitCount))
                    {
                        var value = 1u << shift;

                        foreach (var startIndex in primeNumbers.Where(x => x < bitCount))
                        {
                            var mask = MakeMask(startIndex, bitCount - 1);

                            foreach (var count in primeNumbers)
                            {
                                foreach (var elasticity in Elasticities)
                                {
                                    // Shift the Masked Value, then Merge it with the original Value.
                                    var expected = (ShiftRightWhileCount(value & mask, count) & mask) | (value & ~mask);
                                    yield return GetOne(value, expected, startIndex, count, elasticity).ToArray();
                                }
                            }
                        }
                    }
                }

                return _shiftRightCorrectData ?? (_shiftRightCorrectData = GetAll().ToArray());
            }
        }

        private static IEnumerable<object[]> _removeCorrectData;

        public static IEnumerable<object[]> RemoveCorrectData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(uint value, bool item, Func<uint, bool> expected
                        , VerifyBooleanArrayCallback verify)
                    {
                        yield return value;
                        yield return item;
                        yield return expected(value);
                        yield return verify;
                    }

                    const int bitCount = sizeof(uint) * BitCount;

                    const uint zero = 0;

                    yield return GetOne(zero, true, x => x != default(uint)
                        , bits =>
                        {
                            // ReSharper disable once PossibleMultipleEnumeration
                            Assert.Equal(bitCount, bits.Count());
                            // ReSharper disable once PossibleMultipleEnumeration
                            Assert.All(bits, Assert.False);
                        }).ToArray();

                    yield return GetOne(zero, false, x => ~x != default(uint)
                        , bits =>
                        {
                            // ReSharper disable once PossibleMultipleEnumeration
                            Assert.Equal(bitCount - 1, bits.Count());
                            // ReSharper disable once PossibleMultipleEnumeration
                            Assert.All(bits, Assert.False);
                        }).ToArray();

                    yield return GetOne(~zero, false, x => ~x != default(uint)
                        , bits =>
                        {
                            // ReSharper disable once PossibleMultipleEnumeration
                            Assert.Equal(bitCount, bits.Count());
                            // ReSharper disable once PossibleMultipleEnumeration
                            Assert.All(bits, Assert.True);
                        }).ToArray();

                    yield return GetOne(~zero, true, x => x != default(uint)
                        , bits =>
                        {
                            // ReSharper disable once PossibleMultipleEnumeration
                            Assert.Equal(bitCount - 1, bits.Count());
                            // ReSharper disable once PossibleMultipleEnumeration
                            Assert.All(bits, Assert.True);
                        }).ToArray();

                    for (var shift = 0; shift < bitCount; shift++)
                    {
                        var value = 1u << shift;

                        yield return GetOne(value, true, x => ~x != default(uint)
                            , bits =>
                            {
                                // ReSharper disable once PossibleMultipleEnumeration
                                Assert.Equal(bitCount - 1, bits.Count());
                                // ReSharper disable once PossibleMultipleEnumeration
                                Assert.All(bits, Assert.False);
                            }).ToArray();

                        yield return GetOne(value, false, x => x != default(uint)
                            , bits =>
                            {
                                // ReSharper disable once PossibleMultipleEnumeration
                                Assert.Equal(bitCount - 1, bits.Count());
                                // ReSharper disable once PossibleMultipleEnumeration
                                Assert.Equal(1, bits.Count(x => x));
                                // ReSharper disable once PossibleMultipleEnumeration
                                Assert.Equal(bitCount - 2, bits.Count(x => !x));
                            }).ToArray();

                        yield return GetOne(~value, false, x => ~x != default(uint)
                            , bits =>
                            {
                                // ReSharper disable once PossibleMultipleEnumeration
                                Assert.Equal(bitCount - 1, bits.Count());
                                // ReSharper disable once PossibleMultipleEnumeration
                                Assert.All(bits, Assert.True);
                            }).ToArray();

                        yield return GetOne(~value, true, x => x != default(uint)
                            , bits =>
                            {
                                // ReSharper disable once PossibleMultipleEnumeration
                                Assert.Equal(bitCount - 1, bits.Count());
                                // ReSharper disable once PossibleMultipleEnumeration
                                Assert.Equal(1, bits.Count(x => !x));
                                // ReSharper disable once PossibleMultipleEnumeration
                                Assert.Equal(bitCount - 2, bits.Count(x => x));
                            }).ToArray();
                    }
                }

                return _removeCorrectData ?? (_removeCorrectData = GetAll().ToArray());
            }
        }
    }
}
