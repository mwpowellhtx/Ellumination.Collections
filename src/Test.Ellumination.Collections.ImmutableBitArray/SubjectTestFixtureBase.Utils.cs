using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using static BitConverter;

    public abstract partial class SubjectTestFixtureBase<T>
    {
        protected static IEnumerable<TItem> GetRange<TItem>(params TItem[] values)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var value in values)
            {
                yield return value;
            }
        }

        // TODO: TBD: this is a bit self-defeating? in the sense I am potentially re-writing the code under test? and/or maybe a method, extension, should be exposed internally?
        protected static IEnumerable<byte> GetEndianAwareBytes(uint x)
            => IsLittleEndian ? GetBytes(x) : GetBytes(x).Reverse();

        protected static ImmutableBitArray CreateBitArray(params uint[] values)
            => new ImmutableBitArray(values);

        protected static ImmutableBitArray CreateBitArrayWithArray(params byte[] bytes)
            => new ImmutableBitArray(bytes);

        /// <summary>
        /// Pretty straightforward. Perform the <paramref name="action"/> When
        /// the <paramref name="predicate"/> indicates we should.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="action"></param>
        protected static void When(Func<bool> predicate = null, Action action = null)
        {
            if (predicate?.Invoke() == true)
            {
                action?.Invoke();
            }
        }
    }
}
