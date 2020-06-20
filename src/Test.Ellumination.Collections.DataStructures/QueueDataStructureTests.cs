using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using Xunit;
    using static Math;

    /// <summary>
    /// Provides a nominal set of <see cref="int"/> based <see cref="QueueExtensionMethods"/>
    /// unit tests.
    /// </summary>
    /// <inheritdoc />
    public class QueueDataStructureTests : IntegerBasedDataStructureTestsBase<List<int>>
    {
        protected override List<int> Add(List<int> subject, int item, params int[] additionalItems)
            => Verify(subject).Enqueue(item, additionalItems);

        protected override List<int> ToDataStructure(IEnumerable<int> values)
            => values.ToList();

        protected override int GetCount(List<int> subject)
            => subject.Count;

        protected override int GetRemoveExpected(int item, IList<int> additionalItems)
            => item;

        protected override int Remove(List<int> subject)
            => subject.Dequeue<int, IList<int>>();

        protected override bool TryRemove(List<int> subject, out int result)
            => subject.TryDequeue(out result);

        protected override IEnumerable<int> GetRemoveManyExpected(int item, IList<int> additionalItems, int count)
            => new[] {item}.Concat(additionalItems)
                .Take(Min(additionalItems.Count + 1, count > 0 ? count : 0));

        protected override IEnumerable<int> RemoveMany(List<int> subject, int count)
            => subject.DequeueMany<int, IList<int>>(count);

        protected override bool TryRemoveMany(List<int> subject, out IEnumerable<int> result, int count)
            => subject.TryDequeueMany(out result, count);

        protected override void VerifyInternalList(List<int> subject, IEnumerable<int> expected)
            => subject.AssertEqual(expected);
    }
}
