using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using Xunit;

    /// <summary>
    /// The Collection Patterns are pretty clear. Each of the Extensions can Add items to the
    /// Collection. Conversely, each of the Patterns can Remove items from the Collection. It
    /// does not matter whether we are talking about front, back, many, tried, etc. The Patterns
    /// are all Consistent. With that being established, the usage of this test suite basically
    /// involves simply defining each of the respective Callbacks. You have to provide the test
    /// cases hooks as well satisfying both the compiler and the Unit Test framework Test Case
    /// discovery, but this is a minor point by comparison. The hard work of actually defining
    /// the test cases is also provided.
    /// </summary>
    public abstract class IntegerBasedDataStructureTestsBase<TDataStructure>
        : DataStructureTestsBase<int, TDataStructure>
        where TDataStructure : class, new()
    {
        static IntegerBasedDataStructureTestsBase()
        {
            BasicTestCases = ProtectedBasicTestCases;
            ManyItemsTestCases = ProtectedManyItemsTestCases;
        }

        protected abstract TDataStructure ToDataStructure(IEnumerable<int> values);

        //protected abstract bool GetDataStructureEquals(TDataStructure a, TDataStructure b);

        protected abstract int GetCount(TDataStructure subject);

        protected abstract TDataStructure Add(TDataStructure subject, int item, params int[] additionalItems);

        protected abstract int Remove(TDataStructure subject);

        protected abstract int GetRemoveExpected(int item, IList<int> additionalItems);

        protected abstract bool TryRemove(TDataStructure subject, out int result);

        protected abstract IEnumerable<int> GetRemoveManyExpected(int item, IList<int> additionalItems, int count);

        protected abstract IEnumerable<int> RemoveMany(TDataStructure subject, int count);

        protected abstract bool TryRemoveMany(TDataStructure subject, out IEnumerable<int> result, int count);

        protected abstract void VerifyInternalList(TDataStructure subject, IEnumerable<int> expected);

        protected static IEnumerable<object[]> ProtectedBasicTestCases
        {
            get
            {
                yield return new object[] {1, new ItemList()};
                yield return new object[] {1, new ItemList(2)};
                yield return new object[] {1, new ItemList(2, 3)};
                yield return new object[] {1, new ItemList(2, 3, 4)};
            }
        }

        public static IEnumerable<object[]> BasicTestCases { get; protected set; }

        [Theory, MemberData(nameof(BasicTestCases))]
        public virtual void Verify_can_Add(int item, ItemList additionalItems)
        {
            var sub = Add(Subject, item, additionalItems.ToArray());
            GetCount(sub).AssertEqual(additionalItems.Count + 1);
            VerifyInternalList(sub, new[] {item}.Concat(additionalItems).Reverse());
        }

        [Theory, MemberData(nameof(BasicTestCases))]
        public void Verify_can_Remove(int item, ItemList additionalItems)
        {
            var sub = Add(Subject, item, additionalItems.ToArray());
            GetCount(sub).AssertEqual(additionalItems.Count + 1);
            var expected = GetRemoveExpected(item, additionalItems);
            Remove(sub).AssertEqual(expected);
        }

        [Fact]
        public void Verify_that_Remove_empty_throws()
        {
            Action Get() => () => Remove(Subject);
            const string index = nameof(index);
            const string list = nameof(list);
            var paramNames = new[] {index, list}.ToList();
            Get().AssertThrows<ArgumentOutOfRangeException>().Verify(
                ex => ex.ParamName.AssertTrue(x => paramNames.Contains(x))
            );
        }

        [Theory, MemberData(nameof(BasicTestCases))]
        public void Verify_that_TryRemove_correct(int item, ItemList additionalItems)
        {
            var sub = Add(Subject, item, additionalItems.ToArray());
            GetCount(sub).AssertEqual(additionalItems.Count + 1);
            var expected = GetRemoveExpected(item, additionalItems);
            // Wow for C# 7 ...
            TryRemove(sub, out var actual).AssertTrue();
            actual.AssertEqual(expected);
        }

        [Fact]
        public void Verify_that_TryRemove_empty_false()
        {
            TryRemove(Verify(Subject), out var actual).AssertFalse();
            actual.AssertEqual(default(int));
        }

        protected static IEnumerable<object[]> ProtectedManyItemsTestCases
        {
            get
            {
                yield return new object[] {1, new ItemList(), 2};
                yield return new object[] {1, new ItemList(2), 2};
                yield return new object[] {1, new ItemList(2, 3), 2};
                yield return new object[] {1, new ItemList(2, 3, 4), 2};
                yield return new object[] {1, new ItemList(), -2};
                yield return new object[] {1, new ItemList(2), -2};
                yield return new object[] {1, new ItemList(2, 3), -2};
                yield return new object[] {1, new ItemList(2, 3, 4), -2};
            }
        }

        public static IEnumerable<object[]> ManyItemsTestCases { get; protected set; }


        [Theory, MemberData(nameof(ManyItemsTestCases))]
        public void Verify_that_RemoveMany_correct(int item, ItemList additionalItems, int count)
        {
            var sub = Add(Subject, item, additionalItems.ToArray());
            var actual = RemoveMany(sub, count).ToArray();
            var expected = GetRemoveManyExpected(item, additionalItems, count).ToArray();
            actual.AssertEqual(expected);
        }

        [Theory, MemberData(nameof(ManyItemsTestCases))]
        public void Verify_that_TryRemoveMany_correct(int item, ItemList additionalItems, int count)
        {
            var sub = Add(Subject, item, additionalItems.ToArray());
            TryRemoveMany(sub, out var actual, count).AssertEqual(count > 0);
            var expected = GetRemoveManyExpected(item, additionalItems, count);
            actual.AssertEqual(expected);
        }
    }
}
