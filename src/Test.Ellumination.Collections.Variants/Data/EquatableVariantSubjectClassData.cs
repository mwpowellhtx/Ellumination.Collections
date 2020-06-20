using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Variants
{
    using static Collections;

    internal class EquatableVariantSubjectClassData : CompareVariantSubjectClassDataBase
    {
        private static IEnumerable<object[]> _privateData;

        protected override IEnumerable<object[]> ProtectedData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    foreach (var (x, y) in base.ProtectedData.Select(datum => datum.ToTuple<Variant, Variant<int>>()))
                    {
                        switch (x)
                        {
                            case Variant<int> z:
                                yield return GetRange<object>(x, y, DoesVariantEqual(z, y)).ToArray();
                                break;
                            case Variant<bool> _:
                                yield return GetRange<object>(x, y, VariantDoesNotEqualResult).ToArray();
                                break;
                        }
                    }
                }

                return _privateData ?? (_privateData = GetAll().ToArray());
            }
        }
    }
}
