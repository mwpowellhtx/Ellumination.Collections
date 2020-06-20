using Microsoft.CodeAnalysis;

namespace Ellumination.Collections
{
    using static Resources;
    using static Category;
    using static DiagnosticSeverity;

    internal enum Category
    {
        Usage
    }

    internal static class Descriptors
    {
        private const string IdPrefix = "ElluminationCollectionsEnumerations";
        private const string HelpUriBase = "https://github/mwpowellhtx/Ellumination.Collections/wiki/analyzers/rules/";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString DerivedEnumerationMustBePartialTitle
            = new LocalizableResourceString(nameof(DerivedEnumerationMustBePartialTitle)
                , ResourceManager, typeof(Resources));

        private static readonly LocalizableString DerivedEnumerationMustBePartialMessageFormat
            = new LocalizableResourceString(nameof(DerivedEnumerationMustBePartialMessageFormat)
                , ResourceManager, typeof(Resources));

        private static readonly LocalizableString DerivedEnumerationMustBePartialDescription
            = new LocalizableResourceString(nameof(DerivedEnumerationMustBePartialDescription)
                , ResourceManager, typeof(Resources));

        private static DiagnosticDescriptor Rule(int id, string title, Category category
            , DiagnosticSeverity defaultSeverity, string messageFormat, string description = null)
        {
            const bool isEnabledByDefault = true;
            return new DiagnosticDescriptor($"{IdPrefix}{id}", title, messageFormat, $"{category}"
                , defaultSeverity, isEnabledByDefault, description, $"{HelpUriBase}{id}");
        }

        // ReSharper disable once InconsistentNaming
        public static DiagnosticDescriptor X1000_DerivedEnumerationMustBePartial { get; }
            = Rule(1000, DerivedEnumerationMustBePartialTitle.ToString(), Usage
                , Error, DerivedEnumerationMustBePartialMessageFormat.ToString()
                , DerivedEnumerationMustBePartialDescription.ToString());
    }
}
