using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Variants
{
    using static Collections;

    internal abstract class CompareVariantSubjectClassDataBase : VariantClassDataBase
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
                        yield return GetRange<object>((Variant) x, x).ToArray();

                        foreach (var y in GetVariantValues(IntegerValues))
                        {
                            yield return GetRange<object>((Variant) y, x).ToArray();
                        }

                        foreach (var y in GetVariantValues(BoolValues))
                        {
                            yield return GetRange<object>((Variant) y, x).ToArray();
                        }
                    }
                }

                return _privateData ?? (_privateData = GetAll().ToArray());
            }
        }
    }
}
