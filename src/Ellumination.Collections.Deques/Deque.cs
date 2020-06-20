using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    /// <inheritdoc cref="Generic.Deque{T}"/>
    public class Deque : Generic.Deque<object>, IDeque
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public Deque()
        {
        }

        /// <summary>
        /// Public Constructor.
        /// </summary>
        /// <param name="values"></param>
        public Deque(IEnumerable values)
            : base(values.OfType<object>().ToList())
        {
        }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <paramref name="comparer"/>
        public Deque(IEqualityComparer<object> comparer)
            : base(comparer)
        {
        }

        /// <summary>
        /// Public Constructor.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="comparer"></param>
        public Deque(IEnumerable values, IEqualityComparer<object> comparer)
            : base(values.OfType<object>().ToList(), comparer)
        {
        }
    }
}
