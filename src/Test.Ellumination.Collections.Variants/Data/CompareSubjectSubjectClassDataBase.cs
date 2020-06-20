using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Variants
{
    using static Collections;

    /// <summary>
    /// Furnishes Class Data whereby the left hand side and right hand side operands are both the same Strongly Typed <see cref="IVariant{T}"/>.
    /// </summary>
    internal abstract class CompareSubjectSubjectClassDataBase : VariantClassDataBase
    {
        private static IEnumerable<object[]> _privateData;

        protected override IEnumerable<object[]> ProtectedData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    foreach (var x in GetVariantValues(IntegerValues).ToArray())
                    {
                        yield return GetRange<object>(x, x).ToArray();

                        foreach (var y in GetVariantValues(IntegerValues).ToArray())
                        {
                            yield return GetRange<object>(x, y).ToArray();
                        }

                        foreach (var y in GetVariantValues(IntegerGreaterThan).ToArray())
                        {
                            yield return GetRange<object>(x, y).ToArray();
                            yield return GetRange<object>(y, x).ToArray();
                        }
                    }
                }

                return _privateData ?? (_privateData = GetAll().ToArray());
            }
        }

        private static IEnumerable<object[]> _counterData;

        protected IEnumerable<object[]> CounterData
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    foreach (var x in GetVariantValues(IntegerValues))
                    {
                        foreach (var y in GetVariantValues(BoolValues))
                        {
                            yield return GetRange<object>(x, y).ToArray();
                        }
                    }
                }

                return _counterData ?? (_counterData = GetAll().ToArray());
            }
        }
    }
}
