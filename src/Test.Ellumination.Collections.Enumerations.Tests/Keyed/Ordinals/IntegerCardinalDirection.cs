using System.Collections.Generic;

namespace Ellumination.Collections.Keyed.Ordinals
{
    // ReSharper disable once PartialTypeWithSinglePart
    // ReSharper disable UnusedMember.Global
    public partial class IntegerCardinalDirection : IntegerOrdinalEnumeration<IntegerCardinalDirection>
    {
        public static readonly IntegerCardinalDirection North = new IntegerCardinalDirection();

        public static readonly IntegerCardinalDirection NorthWest = new IntegerCardinalDirection();

        public static readonly IntegerCardinalDirection West = new IntegerCardinalDirection();

        public static readonly IntegerCardinalDirection SouthWest = new IntegerCardinalDirection();

        public static readonly IntegerCardinalDirection South = new IntegerCardinalDirection();

        public static readonly IntegerCardinalDirection SouthEast = new IntegerCardinalDirection();

        public static readonly IntegerCardinalDirection East = new IntegerCardinalDirection();

        public static readonly IntegerCardinalDirection NorthEast = new IntegerCardinalDirection();

        static IntegerCardinalDirection()
        {
            InitializeValueKeys(Values, default(int), x => x + 1);
        }

        private IntegerCardinalDirection()
        {
        }

        /// <inheritdoc />
        public override IEnumerable<IntegerCardinalDirection> EnumeratedValues => GetValues<IntegerCardinalDirection>();
    }
    // ReSharper restore UnusedMember.Global
}
