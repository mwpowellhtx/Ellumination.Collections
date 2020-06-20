using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using Xunit;
    using static Math;

    /// <summary>
    /// Provides a nominal set of <see cref="int"/> based <see cref="StackExtensionMethods"/>
    /// unit tests.
    /// </summary>
    /// <inheritdoc />
    public class StackDataStructureTests : IntegerBasedDataStructureTestsBase<List<int>>
    {
        protected override List<int> Add(List<int> subject, int item, params int[] additionalItems)
            => Verify(subject).Push(item, additionalItems);

        protected override int GetCount(List<int> subject)
            => subject.Count;

        protected override List<int> ToDataStructure(IEnumerable<int> values)
            => values.ToList();

        protected override int GetRemoveExpected(int item, IList<int> additionalItems)
            => additionalItems.Count > 0 ? additionalItems.Last() : item;

        protected override int Remove(List<int> subject)
            => subject.Pop<int, IList<int>>();

        protected override bool TryRemove(List<int> subject, out int result)
            => subject.TryPop(out result);

        protected override IEnumerable<int> GetRemoveManyExpected(int item, IList<int> additionalItems, int count)
            => new[] {item}.Concat(additionalItems).Reverse()
                .Take(Min(additionalItems.Count + 1, count > 0 ? count : 0));

        protected override IEnumerable<int> RemoveMany(List<int> subject, int count) => subject.PopMany<int, IList<int>>(count);

        protected override bool TryRemoveMany(List<int> subject, out IEnumerable<int> result, int count)
            => subject.TryPopMany(out result, count);

        protected override void VerifyInternalList(List<int> subject, IEnumerable<int> expected)
            => subject.AssertEqual(expected);
    }
}
