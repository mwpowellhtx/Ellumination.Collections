using System;

namespace Ellumination.Collections.Variants
{
    internal static class DataExtensionMethods
    {
        public static Tuple<T1, T2> ToTuple<T1, T2>(this object[] datum, int xIndex = 0, int yIndex = 1)
            => Tuple.Create((T1) datum[xIndex], (T2) datum[yIndex]);
    }
}
