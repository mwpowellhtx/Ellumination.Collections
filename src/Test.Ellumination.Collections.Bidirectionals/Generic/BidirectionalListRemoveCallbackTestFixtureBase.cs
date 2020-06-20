using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Generic
{
    using Xunit;

    /// <summary>
    /// Provides a set of Tests for the <see cref="IBidirectionalList{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BidirectionalListRemoveCallbackTestFixtureBase<T> : BidirectionalListTestFixtureBase<T>
        where T : IEquatable<T>
    {
        protected override void ConnectCallbacks(IBidirectionalList<T> list
            , BidirectionalListItemCallback<T> onCallingBack, BidirectionalListItemCallback<T> onCalledBack)
        {
            Assert.NotNull(onCallingBack);
            Assert.NotNull(onCalledBack);

            list.RemovingItem += onCallingBack;
            list.RemovedItem += onCalledBack;
        }

        protected override IBidirectionalList<T> CreateBidirectionalList(Func<IEnumerable<T>> getValues)
            => new BidirectionalList<T>(getValues());

        protected override IBidirectionalList<T> CreateBidirectionalList(Func<IEnumerable<T>> getValues
            , BidirectionalListItemCallback<T> beforeCallback, BidirectionalListItemCallback<T> afterCallback)
            => new BidirectionalList<T>(getValues(), onRemoved: afterCallback, onRemoving: beforeCallback);

        protected override void PrepareList(IList<T> list, T expectedItem)
        {
            base.PrepareList(list, expectedItem);

            // Ensures that the List is Prepared with a Unique Item.
            T GetUniqueItem()
            {
                T x;
                while (list.Contains(x = NewItem))
                {
                }

                return x;
            }

            list.Add(ExpectedItem = GetUniqueItem());
        }

        [Fact]
        public void RemoveCallbacksWorkCorrectly() => VerifyListCallbacks((ref IBidirectionalList<T> list, T x) =>
            Assert.True(list.Remove(x))
        );

        [Fact]
        public void RemoveAtCallbacksWorkProperly() => VerifyListCallbacks((ref IBidirectionalList<T> list, T x) =>
        {
            var index = list.IndexOf(x);
            Assert.True(index >= 0);
            var count = list.Count;
            list.RemoveAt(index);
            Assert.Equal(count - 1, list.Count);
        });

        [Fact]
        public void ClearCallbacksWorkProperly()
        {
            // We will ignore x in this instance.
            VerifyListCallbacks((ref IBidirectionalList<T> list, T _) =>
            {
                // We have to keep track of the Values prior to preparation, etc.
                var values = list.ToArray();
                int removingIndex = -1, removedIndex = -1;
                list.RemovingItem += y => Assert.Equal(values[++removingIndex], y);
                list.RemovedItem += y => Assert.Equal(values[++removedIndex], y);
                list.Clear();
                Assert.Empty(list);
                Assert.Equal(removedIndex + 1, values.Length);
            });
        }

        [Fact]
        public void IndexerCallbacksWorkProperly() => VerifyListCallbacks((ref IBidirectionalList<T> list, T x) =>
        {
            var expectedValue = list[Index];
            bool? callingBackCalled = null, calledBackCalled = null;
            list.RemovingItem += y =>
            {
                callingBackCalled = true;
                Assert.Equal(expectedValue, y);
            };
            list.RemovedItem += y =>
            {
                calledBackCalled = true;
                Assert.Equal(expectedValue, y);
            };
            var count = list.Count;
            list[Index] = x;
            Assert.Equal(count, list.Count);
            Assert.True(callingBackCalled);
            Assert.True(calledBackCalled);
        });
    }
}
