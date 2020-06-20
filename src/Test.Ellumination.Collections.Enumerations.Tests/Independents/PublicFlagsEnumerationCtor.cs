namespace Ellumination.Collections.Independents
{
    // ReSharper disable UnusedMember.Global
    public class PublicFlagsEnumerationCtor : Keyed.Flags.Enumeration<PublicFlagsEnumerationCtor>
    {
        // ReSharper disable once ParameterTypeCanBeEnumerable.Local
        public PublicFlagsEnumerationCtor(byte[] bytes)
            : base(bytes)
        {
        }

        //static PublicFlagsEnumerationCtor()
        //{
        //    InitializeKeyLengths(Values, 2 * BitsPerByte);
        //}

        public static readonly PublicFlagsEnumerationCtor Null = null;

        public static readonly PublicFlagsEnumerationCtor First = new PublicFlagsEnumerationCtor(new byte[] {1});

        public static readonly PublicFlagsEnumerationCtor Duplicate = new PublicFlagsEnumerationCtor(new byte[] {1});

        public static readonly PublicFlagsEnumerationCtor Second = new PublicFlagsEnumerationCtor(new byte[] {2, 3});
    }
    // ReSharper restore UnusedMember.Global
}
