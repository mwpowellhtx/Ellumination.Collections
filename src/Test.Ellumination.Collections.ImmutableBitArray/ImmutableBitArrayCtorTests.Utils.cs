using System.Collections.Generic;

namespace Ellumination.Collections
{
    using Xunit;

    public partial class ImmutableBitArrayCtorTests
    {
        private static ImmutableBitArray CreateBitArray(IEnumerable<byte> bytes)
        {
            Assert.NotNull(bytes);
            return new ImmutableBitArray(bytes);
        }
    }
}
