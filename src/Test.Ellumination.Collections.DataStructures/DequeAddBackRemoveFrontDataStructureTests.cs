using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using Generic;
    using Xunit;
    using static Math;

    /// <summary>
    /// Provides a nominal set of <see cref="int"/> based <see cref="DequeExtensionMethods"/>
    /// unit tests. We will only focus on one of the Deque, or Double-ended Queue, permutations
    /// for test coverage purposes.
    /// </summary>
    /// <inheritdoc />
    public class DequeAddBackRemoveFrontDataStructureTests : IntegerBasedDataStructureTestsBase<Deque<int>>
    {
        // ReSharper disable once RedundantTypeArgumentsOfMethod
        protected override Deque<int> ToDataStructure(IEnumerable<int> values)
            => values.ToDeque<int>();

        protected override Deque<int> Add(Deque<int> subject, int item, params int[] additionalItems)
        {
            subject.EnqueueBack(item);
            // ReSharper disable once RedundantTypeArgumentsOfMethod
            subject.EnqueueBackMany<int, Deque<int>>(additionalItems);
            return subject;
        }

        protected override int GetCount(Deque<int> subject)
            => subject.Count;

        protected override int GetRemoveExpected(int item, IList<int> additionalItems)
            => item;

        protected override int Remove(Deque<int> subject)
            => subject.DequeueFront();

        protected override bool TryRemove(Deque<int> subject, out int result)
            => subject.TryDequeueFront(out result);

        protected override IEnumerable<int> GetRemoveManyExpected(int item, IList<int> additionalItems, int count)
            => new[] {item}.Concat(additionalItems)
                .Take(Min(additionalItems.Count + 1, count > 0 ? count : 0));

        protected override IEnumerable<int> RemoveMany(Deque<int> subject, int count)
            => subject.DequeueFrontMany(count);

        protected override bool TryRemoveMany(Deque<int> subject, out IEnumerable<int> result, int count)
            => subject.TryDequeueFrontMany(out result, count);

        protected override void VerifyInternalList(Deque<int> subject, IEnumerable<int> expected)
            => subject.InternalList.AssertEqual(expected.Reverse());
    }
}
