using System;
using System.Runtime.Serialization;
using Microsoft.CodeAnalysis;

namespace Ellumination.Collections
{
    using CodeAnalysis.Verifiers;

    public class DerivedEnumerationClassMustBePartialCodeFixVerifier : CodeFixVerifier, IDisposable
    {
        private static void DiagnosticAnalyzerRequestedCallback(object sender, DiagnosticAnalyzerRequestedEventArgs e)
        {
            e.Analyzer = new DerivedEnumerationClassMustBePartial();
        }

        private static void CodeFixProviderRequiredCallback(object sender, CodeFixProviderRequiredEventArgs e)
        {
            e.CodeFixProvider = new DerivedEnumerationClassMustBePartialCodeFixProvider();
        }

        #region We must also reference bits from the deliverable assemblies

        private static readonly MetadataReference SystemRuntimeReference
            = MetadataReference.CreateFromFile(typeof(ISerializable).Assembly.Location);

        private static readonly MetadataReference ImmutableBitArrayReference
            = MetadataReference.CreateFromFile(typeof(ImmutableBitArray).Assembly.Location);

        private static readonly MetadataReference EnumerationsReference
            = MetadataReference.CreateFromFile(typeof(Enumeration).Assembly.Location);

        private static readonly MetadataReference FlagsEnumerationAttributeReference
            = MetadataReference.CreateFromFile(typeof(FlagsEnumerationAttribute).Assembly.Location);

        private static void MetadataReferencesRequiredCallback(object sender, MetadataReferencesRequiredEventArgs e)
        {
            e.MetadataReferences.AddRange(
                SystemRuntimeReference
                , ImmutableBitArrayReference
                , EnumerationsReference
                , FlagsEnumerationAttributeReference
            );
        }

        #endregion

        /// <summary>
        /// This constructor is utilized by Xunit IClassFixture paradigm.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public DerivedEnumerationClassMustBePartialCodeFixVerifier()
        {
            DiagnosticAnalyzerRequested += DiagnosticAnalyzerRequestedCallback;
            CodeFixProviderRequired += CodeFixProviderRequiredCallback;
            MetadataReferencesRequired += MetadataReferencesRequiredCallback;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            MetadataReferencesRequired -= MetadataReferencesRequiredCallback;
            DiagnosticAnalyzerRequested -= DiagnosticAnalyzerRequestedCallback;
            CodeFixProviderRequired -= CodeFixProviderRequiredCallback;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
