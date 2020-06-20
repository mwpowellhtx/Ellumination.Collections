namespace Ellumination.Collections.Generic
{
    using static Randomizer;

    /// <summary>
    /// We do not have to do anything that fancy here. Just focus on an <see cref="int"/> based
    /// approach here.
    /// </summary>
    /// <inheritdoc />
    public class IntegerBidirectionalListRemoveCallbackTests : BidirectionalListRemoveCallbackTestFixtureBase<int>
    {
        protected override int NewItem => Rnd.Next();

    }
}
