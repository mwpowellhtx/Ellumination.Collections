using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Variants
{
    using static Collections;

    internal class CompareSubjectVariantClassDataBase : VariantClassDataBase
    {
        private static IEnumerable<object[]> _privateData;

        protected override IEnumerable<object[]> ProtectedData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    foreach (var x in GetVariantValues(IntegerValues))
                    {
                        yield return GetRange<object>(x, (Variant) x).ToArray();

                        foreach (var y in GetVariantValues(IntegerValues))
                        {
                            yield return GetRange<object>(x, (Variant) y).ToArray();
                        }

                        foreach (var y in GetVariantValues(BoolValues))
                        {
                            yield return GetRange<object>(x, (Variant) y).ToArray();
                        }
                    }
                }

                return _privateData ?? (_privateData = GetAll().ToArray());
            }
        }
    }
}
