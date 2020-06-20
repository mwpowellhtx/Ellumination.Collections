using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using static ImmutableBitArray;

    // ReSharper disable once UnusedMember.Global
    public partial class ImmutableBitArrayTests
    {
        private class RollingShift
        {
            internal int Shift { get; private set; } = default(int);

            private RollingShift Add(int delta) => new RollingShift {Shift = (Shift + delta) % BitCount};


            public static RollingShift operator +(RollingShift other, int delta) => other.Add(delta);

            public static RollingShift operator -(RollingShift other, int delta) => other.Add(-delta);

            public static RollingShift operator ++(RollingShift other) => other + 1;

            public static RollingShift operator --(RollingShift other) => other - 1;

            public byte Apply(byte value) => (byte) (value << Shift);
        }

        private static IEnumerable<object[]> _toBytesData;

        public static IEnumerable<object[]> ToBytesData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(IEnumerable<byte> bytes, bool msb)
                    {
                        /* This line is critical because ... watch what is happening here: Bytes
                         * evalutes now before being consumed, never mind evaluated, during the
                         * Unit Test Assertions. Otherwise, Bytes is evaluated TWICE, and with
                         * unexpected results, although perfectly reasonable when you stop to
                         * consider how the runtime evaluates Enumerable collections. */

                        yield return GetRange(bytes.ToArray());
                        yield return msb;
                    }

                    var rollingShift = new RollingShift();

                    IEnumerable<byte> GetBytes(int count)
                    {
                        const byte one = 1;

                        while (count-- > 0)
                        {
                            yield return rollingShift++.Apply(one);
                        }
                    }

                    for (var count = 0; count < sizeof(uint); count++)
                    {
                        var bytes = GetBytes(count);

                        foreach (var expectedMsb in GetRange(true, false))
                        {
                            // ReSharper disable once PossibleMultipleEnumeration
                            yield return GetOne(bytes, expectedMsb).ToArray();
                        }
                    }
                }

                return _toBytesData ?? (_toBytesData = GetAll().ToArray());
            }
        }

        private static IEnumerable<object[]> _toIntsData;

        public static IEnumerable<object[]> ToIntsData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(IEnumerable<uint> values, bool msb)
                    {
                        /* This line is critical because ... watch what is happening here: Bytes
                         * evalutes now before being consumed, never mind evaluated, during the
                         * Unit Test Assertions. Otherwise, Bytes is evaluated TWICE, and with
                         * unexpected results, although perfectly reasonable when you stop to
                         * consider how the runtime evaluates Enumerable collections. */

                        yield return GetRange(values.ToArray());
                        yield return msb;
                    }

                    IEnumerable<uint> GetValues(int count)
                    {
                        var values = new uint[] { 1, 2, 3, 4 };

                        while (count-- > 0)
                        {
                            foreach (var x in values)
                            {
                                yield return x;
                            }
                        }
                    }

                    for (var count = 0; count < 3; count++)
                    {
                        foreach (var expectedMsb in GetRange(true, false))
                        {
                            yield return GetOne(GetValues(count), expectedMsb).ToArray();
                        }
                    }
                }

                return _toIntsData ?? (_toIntsData = GetAll().ToArray());
            }
        }

        private static IEnumerable<object[]> _binaryAndData;

        public static IEnumerable<object[]> BinaryAndData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(uint x, uint y
                        , CalculateBinaryOperationCallback<uint> expectedOp
                        , CalculateBinaryOperationCallback<ImmutableBitArray> actualOp)
                    {
                        yield return x;
                        yield return y;
                        yield return expectedOp;
                        yield return actualOp;
                    }

                    var values = new uint[] { 1, 2, 3, 4 };

                    foreach (var x in values)
                    {
                        foreach (var y in values)
                        {
                            yield return GetOne(x, y, (a, b) => a & b, (a, b) => a.And(b)).ToArray();
                        }
                    }
                }

                return _binaryAndData ?? (_binaryAndData = GetAll().ToArray());
            }
        }

        private static IEnumerable<object[]> _binaryOrData;

        public static IEnumerable<object[]> BinaryOrData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(uint x, uint y
                        , CalculateBinaryOperationCallback<uint> expectedOp
                        , CalculateBinaryOperationCallback<ImmutableBitArray> actualOp)
                    {
                        yield return x;
                        yield return y;
                        yield return expectedOp;
                        yield return actualOp;
                    }

                    var values = new uint[] { 1, 2, 3, 4 };

                    foreach (var x in values)
                    {
                        foreach (var y in values)
                        {
                            yield return GetOne(x, y, (a, b) => a | b, (a, b) => a.Or(b)).ToArray();
                        }
                    }
                }

                return _binaryOrData ?? (_binaryOrData = GetAll().ToArray());
            }
        }

        private static IEnumerable<object[]> _binaryXorData;

        public static IEnumerable<object[]> BinaryXorData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(uint x, uint y
                        , CalculateBinaryOperationCallback<uint> expectedOp
                        , CalculateBinaryOperationCallback<ImmutableBitArray> actualOp)
                    {
                        yield return x;
                        yield return y;
                        yield return expectedOp;
                        yield return actualOp;
                    }

                    var values = new uint[] { 1, 2, 3, 4 };

                    foreach (var x in values)
                    {
                        foreach (var y in values)
                        {
                            yield return GetOne(x, y, (a, b) => a ^ b, (a, b) => a.Xor(b)).ToArray();
                        }
                    }
                }

                return _binaryXorData ?? (_binaryXorData = GetAll().ToArray());
            }
        }

        private static IEnumerable<object[]> _unaryNotData;

        public static IEnumerable<object[]> UnaryNotData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(uint x
                        , CalculateUnaryOperationCallback<uint> expectedOp
                        , CalculateUnaryOperationCallback<ImmutableBitArray> actualOp)
                    {
                        yield return x;
                        yield return expectedOp;
                        yield return actualOp;
                    }

                    var values = new uint[] { 1, 2, 3, 4 };

                    foreach (var x in values)
                    {
                        yield return GetOne(x, a => ~a, a => a.Not()).ToArray();
                    }
                }

                return _unaryNotData ?? (_unaryNotData = GetAll().ToArray());
            }
        }

        private const int ElementBitCount = sizeof(uint) * BitCount;

        private static uint GetElement(int shift) => (uint) (1 << shift);

        private static IEnumerable<object[]> _equalsData;

        public static IEnumerable<object[]> EqualsData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(IEnumerable<uint> x, IEnumerable<uint> y, bool expected)
                    {
                        yield return x;
                        yield return y;
                        yield return expected;
                    }

                    var zero = GetRange<uint>(0).ToArray();

                    yield return GetOne(zero, zero, true).ToArray();
                    yield return GetOne(zero, zero.Concat(zero), true).ToArray();
                    yield return GetOne(zero.Concat(zero), zero, true).ToArray();

                    // TODO: TBD: could get fancier, perhaps, masking more bits into the test elements, etc...
                    for (var i = 0; i < ElementBitCount - 1; i++)
                    {
                        var element = GetRange(GetElement(i)).ToArray();

                        yield return GetOne(element, element, true).ToArray();
                        yield return GetOne(element, element.Concat(zero), true).ToArray();
                        yield return GetOne(element.Concat(zero), element, true).ToArray();
                        yield return GetOne(element.Concat(zero), element.Concat(zero), true).ToArray();

                        yield return GetOne(zero.Concat(element), element, false).ToArray();
                        yield return GetOne(element, zero.Concat(element), false).ToArray();

                        yield return GetOne(zero.Concat(element), zero.Concat(element), true).ToArray();
                    }
                }

                return _equalsData ?? (_equalsData = GetAll().ToArray());
            }
        }

        private static IEnumerable<object[]> _compareToData;

        public static IEnumerable<object[]> CompareToData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(IEnumerable<uint> x, IEnumerable<uint> y, int expected)
                    {
                        yield return x;
                        yield return y;
                        yield return expected;
                    }

                    const int equal = 0;
                    const uint defaultValue = default(uint);

                    yield return GetOne(GetRange(defaultValue), GetRange(defaultValue), equal).ToArray();
                    yield return GetOne(GetRange(defaultValue), GetRange(defaultValue, defaultValue), equal).ToArray();
                    yield return GetOne(GetRange(defaultValue, defaultValue), GetRange(defaultValue), equal).ToArray();

                    const int lesser = -1;
                    const int greater = 1;

                    for (var i = 0; i < ElementBitCount - 1; i++)
                    {
                        var element = GetElement(i);
                        var elementPlusOne = GetElement(i + 1);

                        yield return GetOne(GetRange(element), GetRange(element), equal).ToArray();
                        yield return GetOne(GetRange(element), GetRange(element, defaultValue), equal).ToArray();
                        yield return GetOne(GetRange(element, defaultValue), GetRange(element), equal).ToArray();

                        yield return GetOne(GetRange(element), GetRange(elementPlusOne), lesser).ToArray();
                        yield return GetOne(GetRange(element), GetRange(elementPlusOne, defaultValue), lesser).ToArray();
                        yield return GetOne(GetRange(element, defaultValue), GetRange(elementPlusOne), lesser).ToArray();

                        yield return GetOne(GetRange(elementPlusOne), GetRange(element), greater).ToArray();
                        yield return GetOne(GetRange(elementPlusOne), GetRange(element, defaultValue), greater).ToArray();
                        yield return GetOne(GetRange(elementPlusOne, defaultValue), GetRange(element), greater).ToArray();

                        yield return GetOne(GetRange(element), GetRange(defaultValue, element), lesser).ToArray();
                        yield return GetOne(GetRange(defaultValue, element), GetRange(element), greater).ToArray();
                    }
                }

                return _compareToData ?? (_compareToData = GetAll().ToArray());
            }
        }
    }
}
