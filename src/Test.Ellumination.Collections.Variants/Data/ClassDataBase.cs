using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Variants
{
    using static Collections;

    internal abstract class ClassDataBase : IEnumerable<object[]>
    {
        protected static IEnumerable<object[]> MergeDataDimension(IEnumerable<object[]> data, object[] dimension)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var datum in data)
            {
                foreach (var x in dimension)
                {
                    yield return datum.Concat(GetRange(x).ToArray()).ToArray();
                }
            }
        }

        protected abstract IEnumerable<object[]> ProtectedData { get; }

        public IEnumerator<object[]> GetEnumerator() => ProtectedData.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
