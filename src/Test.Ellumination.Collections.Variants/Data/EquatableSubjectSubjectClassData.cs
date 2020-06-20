using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Variants
{
    using static Collections;

    internal class EquatableSubjectSubjectClassData : CompareSubjectSubjectClassDataBase
    {
        private static IEnumerable<object[]> _privateData;

        protected override IEnumerable<object[]> ProtectedData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    foreach (var (x, y) in base.ProtectedData.Select(datum => datum.ToTuple<Variant<int>, Variant<int>>()))
                    {
                        yield return GetRange<object>(x, y, DoesVariantEqual(x, y)).ToArray();
                    }
                }

                return _privateData ?? (_privateData = GetAll().ToArray());
            }
        }
    }
}
