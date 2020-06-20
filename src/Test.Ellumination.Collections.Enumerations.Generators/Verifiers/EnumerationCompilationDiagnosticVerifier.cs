using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Ellumination.Collections
{
    using CodeAnalysis.Verifiers;
    using Microsoft.CodeAnalysis;

    public class EnumerationCompilationDiagnosticVerifier : DiagnosticVerifier, IDisposable
    {
        public EnumerationCompilationDiagnosticVerifier()
        {
            MetadataReferencesRequired += Verifier_MetadataReferencesRequired;
        }

        #region We must also reference bits from the deliverable assemblies

        private static readonly MetadataReference ImmutableBitArrayReference
            = MetadataReference.CreateFromFile(typeof(ImmutableBitArray).Assembly.Location);

        private static readonly MetadataReference EnumerationsReference
            = MetadataReference.CreateFromFile(typeof(Enumeration).Assembly.Location);

        private static readonly MetadataReference FlagsEnumerationAttributeReference
            = MetadataReference.CreateFromFile(typeof(FlagsEnumerationAttribute).Assembly.Location);

        #endregion

        private void Verifier_MetadataReferencesRequired(object sender, MetadataReferencesRequiredEventArgs e)
        {
            var netstandardAssy = Assembly.Load("netstandard, Version=2.0.0.0");
            var netstandardReference = MetadataReference.CreateFromFile(netstandardAssy.Location);

            // Will load the correct System.Runtime version.
            var systemRuntimeAssy = Assembly.Load("System.Runtime, Version=0.0.0.0");
            var systemRuntimeReference = MetadataReference.CreateFromFile(systemRuntimeAssy.Location);

            e.MetadataReferences.AddRange(
                netstandardReference
                , systemRuntimeReference
                , ImmutableBitArrayReference
                , EnumerationsReference
                , FlagsEnumerationAttributeReference
            );
        }

        protected virtual void Dispose(bool disposing)
        {
            MetadataReferencesRequired -= Verifier_MetadataReferencesRequired;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
