namespace Ellumination.Collections.Independents
{
    public class ProtectedBitwiseCtor : Keyed.Flags.Enumeration<ProtectedBitwiseCtor>
    {
        // ReSharper disable once ParameterTypeCanBeEnumerable.Local
        protected ProtectedBitwiseCtor(byte[] bytes)
            : base(bytes)
        {
        }
    }
}
