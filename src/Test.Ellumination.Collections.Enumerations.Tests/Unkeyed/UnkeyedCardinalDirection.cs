namespace Ellumination.Collections.Unkeyed
{
    // ReSharper disable once PartialTypeWithSinglePart
    // ReSharper disable UnusedMember.Global
    public partial class UnkeyedCardinalDirection : Enumeration<UnkeyedCardinalDirection>
    {
        public static readonly UnkeyedCardinalDirection North = new UnkeyedCardinalDirection();

        public static readonly UnkeyedCardinalDirection West = new UnkeyedCardinalDirection();

        public static readonly UnkeyedCardinalDirection South = new UnkeyedCardinalDirection();

        public static readonly UnkeyedCardinalDirection East = new UnkeyedCardinalDirection();

        private UnkeyedCardinalDirection()
        {
        }
    }
    // ReSharper restore UnusedMember.Global
}
