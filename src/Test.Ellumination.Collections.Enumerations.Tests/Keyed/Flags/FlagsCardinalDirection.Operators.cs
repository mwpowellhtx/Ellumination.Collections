namespace Ellumination.Collections.Keyed.Flags
{
    public partial class FlagsCardinalDirection
    {
        // ReSharper disable once UnusedMember.Local, ParameterTypeCanBeEnumerable.Local
        /// <summary>
        /// Private <paramref name="bytes"/> wise Constructor.
        /// </summary>
        /// <param name="bytes"></param>
        private FlagsCardinalDirection(byte[] bytes)
            : base(bytes)
        {
        }

        public static FlagsCardinalDirection operator &(FlagsCardinalDirection a, FlagsCardinalDirection b) => a?.BitwiseAnd(b);

        public static FlagsCardinalDirection operator |(FlagsCardinalDirection a, FlagsCardinalDirection b) => a?.BitwiseOr(b);

        public static FlagsCardinalDirection operator ^(FlagsCardinalDirection a, FlagsCardinalDirection b) => a?.BitwiseXor(b);

        public static FlagsCardinalDirection operator ~(FlagsCardinalDirection other) => other?.BitwiseNot();
    }
}
