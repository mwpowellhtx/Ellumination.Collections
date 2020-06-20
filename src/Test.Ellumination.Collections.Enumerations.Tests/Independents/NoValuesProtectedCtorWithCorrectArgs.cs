namespace Ellumination.Collections.Independents
{
    // ReSharper disable once UnusedMember.Global
    public class NoValuesProtectedCtorWithCorrectArgs : Keyed.Ordinals.IntegerOrdinalEnumeration<NoValuesProtectedCtorWithCorrectArgs>
    {
        // TODO: TBD: reminding us that the intermediate class ctors also need to be present...
        /// <summary>
        /// The Accessibility can be any other thing than Private here in order to demonstrate
        /// that we are testing for the right thing.
        /// </summary>
        /// <inheritdoc />
        protected NoValuesProtectedCtorWithCorrectArgs(int ordinal)
            : base(ordinal)
        {
        }
    }
}
