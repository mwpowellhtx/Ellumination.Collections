using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Generic
{
    using Xunit;

    public abstract class BidirectionalListTestFixtureBase<T> : TestFixtureBase
        where T : IEquatable<T>
    {
        /// <summary>
        /// 0
        /// </summary>
        protected const int Index = 0;

        /// <summary>
        /// Override to Get a NewItem of <typeparamref name="T"/> for use throughout the
        /// Test Cases.
        /// </summary>
        protected abstract T NewItem { get; }

        /// <summary>
        /// Returns the <see cref="IEnumerable{T}"/> corresponding to the
        /// <paramref name="values"/>. We do this intentionally as a Yielded Return so that the
        /// result is not literally the instance we were given.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        protected static IEnumerable<T> GetRange(params T[] values)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var x in values)
            {
                yield return x;
            }
        }

        private IBidirectionalList<T> _targetList;


        protected virtual IEnumerable<T> GetDefaultValues()
        {
            yield break;
        }

        /// <summary>
        /// Returns a new <see cref="IBidirectionalList{T}"/> assuming New Constructor usage.
        /// Override in order to exercise different aspects of the Collection framework.
        /// </summary>
        /// <param name="getValues"></param>
        /// <returns></returns>
        protected abstract IBidirectionalList<T> CreateBidirectionalList(Func<IEnumerable<T>> getValues);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="getValues"></param>
        /// <param name="beforeCallback"></param>
        /// <param name="afterCallback"></param>
        /// <returns></returns>
        protected abstract IBidirectionalList<T> CreateBidirectionalList(Func<IEnumerable<T>> getValues
            , BidirectionalListItemCallback<T> beforeCallback, BidirectionalListItemCallback<T> afterCallback);

        /// <summary>
        /// Gets or Sets the Target List given a couple of extensible hooks.
        /// </summary>
        protected virtual IBidirectionalList<T> TargetList
        {
            get => _targetList ?? (_targetList = CreateBidirectionalList(GetDefaultValues));
            set
            {
                _targetList?.Clear();
                _targetList = value ?? CreateBidirectionalList(GetDefaultValues);
            }
        }

        /// <summary>
        /// Default Protected Constructor.
        /// </summary>
        protected BidirectionalListTestFixtureBase() => InitializeCollection();

        private void InitializeCollection()
        {
            var collection = TargetList;
            collection.Add(NewItem);
            collection.Add(NewItem);
            collection.Add(NewItem);
        }

        protected abstract void ConnectCallbacks(IBidirectionalList<T> list
            , BidirectionalListItemCallback<T> onCallingBack, BidirectionalListItemCallback<T> onCalledBack);

        protected override void Dispose(bool disposing)
        {
            if (disposing && !IsDisposed)
            {
                //// TODO: TBD: raises an interesting point: should a bidi-list be disposable in some fashion?
                //// TODO: TBD: if for nothing else than to disconnect any callbacks that were registered?
                //TargetList = null;
            }

            base.Dispose(disposing);
        }

        protected delegate void VerifyListCallback(ref IBidirectionalList<T> list, T item);

        protected T ExpectedItem { get; set; }

        protected int Order { get; set; }

        // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
        protected IList<int> CalledBackCallOrders { get; } = new List<int> { };

        // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
        protected IList<int> CallingBackCallOrders { get; } = new List<int> { };

        protected void OnCalledBack(T _) => CalledBackCallOrders.Add(++Order);

        protected void OnCallingBack(T _) => CallingBackCallOrders.Add(++Order);

        /// <summary>
        /// Override to Prepare the <paramref name="list"/> with the
        /// <paramref name="expectedItem"/>. The Default operation is essentially a No-Op.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="expectedItem"></param>
        protected virtual void PrepareList(IList<T> list, T expectedItem)
        {
        }

        /// <summary>
        /// Verifies whether the Quality of the List Action being Approached, or having been
        /// Approached, works correctly. It is up to the Caller to provide more context as to
        /// what Approaching or Approached actually means.
        /// </summary>
        /// <param name="arrange"></param>
        /// <param name="verify"></param>
        protected void VerifyListCallbacks(VerifyListCallback arrange, ListVerificationCallback verify = null)
        {
            Assert.NotNull(arrange);

            ExpectedItem = NewItem;

            Order = 0;

            CallingBackCallOrders.Clear();
            CalledBackCallOrders.Clear();

            ConnectCallbacks(TargetList, OnCallingBack, OnCalledBack);

            var list = TargetList;

            PrepareList(list, ExpectedItem);

            arrange.Invoke(ref list, ExpectedItem);

            // Replace the TargetList instance if necessary.
            if (!ReferenceEquals(TargetList, list))
            {
                TargetList = list;
            }

            (verify ?? DefaultListVerificationCallback).Invoke(TargetList);
        }

        protected delegate void ListVerificationCallback(IBidirectionalList<T> list);

        protected void NoOpListVerificationCallback(IBidirectionalList<T> _)
        {
        }

        protected void DefaultListVerificationCallback(IBidirectionalList<T> list)
        {
            /* We should be able to determine qualitatively that not only Adding and Added
             * happened, but also whether Calling did in fact occur prior to Called. */

            Assert.True(CallingBackCallOrders.Count > 0);
            Assert.True(CalledBackCallOrders.Count > 0);

            Assert.All(CallingBackCallOrders, o => Assert.True(o > 0));
            Assert.All(CalledBackCallOrders, o => Assert.True(o > 0));

            Assert.Equal(CallingBackCallOrders.Count, CalledBackCallOrders.Count);

            Assert.All(CallingBackCallOrders.Zip(CalledBackCallOrders
                    , (x, y) => new {CallingBackCallOrder = x, CalledBackCallOrder = y})
                , pair => Assert.True(pair.CalledBackCallOrder > pair.CallingBackCallOrder));
        }
    }
}
