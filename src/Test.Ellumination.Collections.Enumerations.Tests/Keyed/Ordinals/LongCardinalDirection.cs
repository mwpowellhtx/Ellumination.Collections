using System.Collections.Generic;

namespace Ellumination.Collections.Keyed.Ordinals
{
    // ReSharper disable once PartialTypeWithSinglePart
    // ReSharper disable UnusedMember.Global
    public partial class LongCardinalDirection : LongOrdinalEnumeration<LongCardinalDirection>
    {
        public static readonly LongCardinalDirection North = new LongCardinalDirection();

        public static readonly LongCardinalDirection NorthWest = new LongCardinalDirection();

        public static readonly LongCardinalDirection West = new LongCardinalDirection();

        public static readonly LongCardinalDirection SouthWest = new LongCardinalDirection();

        public static readonly LongCardinalDirection South = new LongCardinalDirection();

        public static readonly LongCardinalDirection SouthEast = new LongCardinalDirection();

        public static readonly LongCardinalDirection East = new LongCardinalDirection();

        public static readonly LongCardinalDirection NorthEast = new LongCardinalDirection();

        static LongCardinalDirection()
        {
            InitializeValueKeys(Values, default(long), x => x + 1);
        }

        private LongCardinalDirection()
        {
        }

        /// <inheritdoc />
        public override IEnumerable<LongCardinalDirection> EnumeratedValues => GetValues<LongCardinalDirection>();
    }
    // ReSharper restore UnusedMember.Global
}
