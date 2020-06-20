using System;
using System.Collections.Generic;

namespace Ellumination.Collections.Generic
{
    using static Randomizer;

    /// <summary>
    /// Test cases based on the
    /// <see cref="BidirectionalExtensionMethods.ToBidirectionalList{T}(IEnumerable{T})"/>
    /// extension methods.
    /// </summary>
    /// <inheritdoc />
    public class BidirectionalListAddCallbackExtensionMethodTests : BidirectionalListAddCallbackTestFixtureBase<int>
    {
        protected override int NewItem => Rnd.Next();

        protected override IBidirectionalList<int> CreateBidirectionalList(Func<IEnumerable<int>> getValues)
            => getValues().ToBidirectionalList();

        protected override IBidirectionalList<int> CreateBidirectionalList(Func<IEnumerable<int>> getValues
            , BidirectionalListItemCallback<int> beforeCallback, BidirectionalListItemCallback<int> afterCallback)
            => getValues().ToBidirectionalList(onAdded: afterCallback, onRemoved: null
                , onAdding: beforeCallback, onRemoving: null);
    }
}
