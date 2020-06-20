namespace Ellumination.Collections
{
    using Keyed.Flags;

    /// <summary>
    /// We will expect that actual Code Generation occurs for this definition. Which, there should
    /// be artifacts of that Code Generation vetted via
    /// <see cref="InSituFlagsEnumerationCodeGenerationTestsBase{T}"/>.
    /// </summary>
    /// <inheritdoc />
    [FlagsEnumeration]
    public partial class CardinalDirection : Enumeration<CardinalDirection>
    {
        /// <summary>
        /// North
        /// </summary>
        public static readonly CardinalDirection North = new CardinalDirection();

        /// <summary>
        /// East
        /// </summary>
        public static readonly CardinalDirection East = new CardinalDirection();

        /// <summary>
        /// South
        /// </summary>
        public static readonly CardinalDirection South = new CardinalDirection();

        /// <summary>
        /// West
        /// </summary>
        public static readonly CardinalDirection West = new CardinalDirection();

        static CardinalDirection() => InitializeKeys(Values);

        /// <inheritdoc />
        private CardinalDirection()
        {
        }
    }
}
