using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using Xunit;
    //using static RandomIntValuesAttribute;

    public partial class ImmutableBitArrayCtorTests
    {
        /// <summary>
        /// Returns a range of Sizes leveraged for each of the
        /// <see cref="TheoryAttribute"/> <see cref="MemberDataAttribute"/> properties.
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<uint[]> GetRandomValues()
        {
            var values = new uint[] { 1, 2, 3, 4 };

            for (var size = 0; size < values.Length; size++)
            {
                yield return values.Take(size).ToArray();
            }
        }

        private static IEnumerable<object[]> _ctorUInt32ArrayData;

        public static IEnumerable<object[]> CtorUInt32ArrayData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(bool expectedMsb, params uint[] values)
                    {
                        yield return GetRange(values).ToArray();
                        yield return expectedMsb;
                        var expectedBytes = values.SelectMany(GetEndianAwareBytes);
                        // Be careful we coordinate with the Theory Test arguments.
                        yield return GetRange((expectedMsb ? expectedBytes.Reverse() : expectedBytes).ToArray()).ToArray();
                    }

                    foreach (var expectedMsb in GetRange(true, false))
                    {
                        foreach (var values in GetRandomValues())
                        {
                            yield return GetOne(expectedMsb, values).ToArray();
                        }
                    }
                }

                return _ctorUInt32ArrayData ?? (_ctorUInt32ArrayData = GetAll().ToArray());
            }
        }

        private static IEnumerable<object[]> _ctorByteArrayData;

        public static IEnumerable<object[]> CtorByteArrayData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(bool expectedMsb, params uint[] values)
                    {
                        yield return values.SelectMany(GetEndianAwareBytes).ToArray();
                        yield return expectedMsb;
                    }

                    foreach (var expectedMsb in GetRange(false, true))
                    {
                        foreach (var values in GetRandomValues())
                        {
                            yield return GetOne(expectedMsb, values).ToArray();
                        }
                    }
                }

                return _ctorByteArrayData ?? (_ctorByteArrayData = GetAll().ToArray());
            }
        }

        private static IEnumerable<object[]> _ctorByteIEnumerableData;

        public static IEnumerable<object[]> CtorByteIEnumerableData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<object> GetOne(bool expectedMsb, params uint[] values)
                    {
                        yield return GetRange(values.SelectMany(GetEndianAwareBytes).ToArray());
                        yield return expectedMsb;
                    }

                    foreach (var expectedMsb in GetRange(false, true))
                    {
                        foreach (var values in GetRandomValues())
                        {
                            yield return GetOne(expectedMsb, values).ToArray();
                        }
                    }
                }

                return _ctorByteIEnumerableData ?? (_ctorByteIEnumerableData = GetAll().ToArray());
            }
        }
    }
}
