using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.CodeAnalysis.Verifiers
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using static String;
    using static StringComparison;
    using static VerificationResult;

    /// <summary>
    /// Class for turning strings into documents and getting the diagnostics on them. Parent
    /// class of all Unit Tests for <see cref="DiagnosticAnalyzer"/>.
    /// </summary>
    /// <inheritdoc />
    public abstract partial class DiagnosticVerifier
    {
        #region To be implemented by Test classes

        /// <summary>
        /// Implement the event handler when a <see cref="DiagnosticAnalyzer"/> may be required.
        /// </summary>
        protected event EventHandler<DiagnosticAnalyzerRequestedEventArgs> DiagnosticAnalyzerRequested;

        /// <summary>
        /// Raised on <see cref="DiagnosticAnalyzerRequested"/>.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="e"></param>
        protected virtual void OnDiagnosticAnalyzerRequested(string language, out DiagnosticAnalyzerRequestedEventArgs e)
        {
            e = new DiagnosticAnalyzerRequestedEventArgs(language);
            DiagnosticAnalyzerRequested?.Invoke(this, e);
        }

        #endregion

        #region Verifier wrappers

        /// <summary>
        /// General method that gets a collection of actual <see cref="DiagnosticResult"/> found
        /// in the <paramref name="source"/> after the <see cref="DiagnosticAnalyzer"/> is run,
        /// then verifies each of them.
        /// </summary>
        /// <param name="language">The <see cref="LanguageNames"/> represented by the
        /// <paramref name="source"/>.</param>
        /// <param name="source">A string from which to create <see cref="Document"/> on which to
        /// run the <see cref="DiagnosticAnalyzer"/>.</param>
        /// <param name="expectedResults"><see cref="DiagnosticResult"/> that should appear after
        /// the <see cref="DiagnosticAnalyzer"/> is run on the sources.</param>
        public void VerifyDiagnostics(string language, string source, params DiagnosticResult[] expectedResults)
            => VerifyDiagnostics(new[] {source}, language, expectedResults);

        /// <summary>
        /// General method that gets a collection of actual <see cref="DiagnosticResult"/> found
        /// in the <paramref name="sources"/> after the <see cref="DiagnosticAnalyzer"/> is run,
        /// then verifies each of them.
        /// </summary>
        /// <param name="sources">A set strings from which to create <see cref="Document"/> to run
        /// the <see cref="DiagnosticAnalyzer"/> on.</param>
        /// <param name="language">The language of the classes represented by the
        /// <paramref name="sources"/>.</param>
        /// <param name="expectedResults"><see cref="DiagnosticResult"/> that should appear after
        /// the <see cref="DiagnosticAnalyzer"/> is run on the sources.</param>
        public void VerifyDiagnostics(IEnumerable<string> sources, string language
            , params DiagnosticResult[] expectedResults)
        {
            OnDiagnosticAnalyzerRequested(language, out var e);
            var actualDiagnostics = GetSortedDiagnostics(sources, language, e.Analyzer);
            VerifyDiagnosticResults(language, e.Analyzer, expectedResults, actualDiagnostics.ToArray());
        }

        #endregion

        #region Actual comparisons and verifications

        /// <summary>
        /// Checks each of the actual Diagnostics found and compares them with the corresponding
        /// <see cref="DiagnosticResult"/> in the array of expected results. Diagnostics are
        /// considered equal only if the <see cref="DiagnosticResultLocation"/>, Id, Severity,
        /// and Message of the <see cref="DiagnosticResult"/> match the actual diagnostic.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="analyzer">The analyzer that was being run on the sources.</param>
        /// <param name="expectedResults"><see cref="DiagnosticResult"/> that should have
        /// appeared in the code.</param>
        /// <param name="actualDiagnostics">The <see cref="Diagnostic"/> found by the compiler
        /// after running the analyzer on the source code.</param>
        /// <exception cref="BadDiagnosticResultsException">Throws exceptions when something is
        /// amiss, which may subsequently be vetted by the caller's unit test framework of choice.</exception>
        protected virtual void VerifyDiagnosticResults(string language, DiagnosticAnalyzer analyzer
            , IEnumerable<DiagnosticResult> expectedResults, params Diagnostic[] actualDiagnostics)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            var expectedCount = expectedResults.Count();
            var actualLength = actualDiagnostics.Length;

            if (expectedCount != actualLength)
            {
                // ReSharper disable once PossibleMultipleEnumeration
                throw new BadDiagnosticResultsException(
                    $"Mismatch between number of expected results ({expectedCount})"
                    + $" and the actual diagnostics ({actualLength})"
                    , language, analyzer, LengthMismatch, expectedResults, actualDiagnostics);
            }

            for (var i = 0; i < expectedCount; i++)
            {
                var actualDiagnostic = actualDiagnostics.ElementAt(i);
                // ReSharper disable once PossibleMultipleEnumeration
                var expectedResult = expectedResults.ElementAt(i);

                if (expectedResult.Line == -1 && expectedResult.Column == -1)
                {
                    if (actualDiagnostic.Location == Location.None)
                    {
                        throw new BadDiagnosticResultsException(
                            $"Expected a {typeof(Project).FullName} with No location"
                            + $" but was \"{actualDiagnostic.Location}\""
                            , language, analyzer, DiagnosticMismatch, expectedResult, actualDiagnostic);
                    }
                }
                else
                {
                    VerifyDiagnosticLocation(language, analyzer, expectedResult.Locations.First()
                        , actualDiagnostic, actualDiagnostic.Location);
                    var additionalLocations = actualDiagnostic.AdditionalLocations.ToArray();

                    if (expectedResult.Locations.Count() - 1 != additionalLocations.Length)
                    {
                        throw new BadDiagnosticResultsException(
                            $"Expected {expectedResult.Locations.Count()}"
                            + $" additional locations but found {additionalLocations.Length}"
                            , language, analyzer, LocationMismatch, expectedResult, actualDiagnostic);
                    }

                    for (var j = 0; j < additionalLocations.Length; ++j)
                    {
                        VerifyDiagnosticLocation(language, analyzer, expectedResult.Locations.ElementAt(j + 1)
                            , actualDiagnostic, additionalLocations[j]);
                    }
                }

                // TODO: TBD: repeating the same theme of "verify some property, etc" ...
                // TODO: TBD: may look into simplifying that to leverage a common "verification" for such bits...
                // TODO: TBD: along these lines, we're more or less halfway there already just in terms of the Exception parameters ...
                if (actualDiagnostic.Id != expectedResult.Id)
                {
                    throw new BadDiagnosticResultsException(
                        $"Expected diagnostic id to be \"{expectedResult.Id}\""
                        + $" but was \"{actualDiagnostic.Id}\""
                        , language, analyzer, IdMismatch, expectedResult, actualDiagnostic);
                }

                if (actualDiagnostic.Severity != expectedResult.Severity)
                {
                    throw new BadDiagnosticResultsException(
                        $"Expected diagnostic severity \"{expectedResult.Severity}\""
                        + $" but was \"{actualDiagnostic.Severity}\""
                        , language, analyzer, SeverityMismatch, expectedResult, actualDiagnostic);
                }

                if (!string.Equals(expectedResult.Message, actualDiagnostic.GetMessage()
                    , CurrentCultureIgnoreCase))
                {
                    throw new BadDiagnosticResultsException(
                        $"Expected diagnostic message to be \"{expectedResult.Message}\""
                        + $" but was \"{actualDiagnostic.GetMessage()}\""
                        , language, analyzer, MessageMismatch, expectedResult, actualDiagnostic);
                }
            }
        }

        /// <summary>
        /// Helper method to <see cref="VerifyDiagnosticResults"/> that checks the location of
        /// a <see cref="Diagnostic"/> and compares it with the <see cref="Location"/> in the
        /// expected <see cref="DiagnosticResult"/>.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="analyzer">The <see cref="DiagnosticAnalyzer"/> that was being run on the sources.</param>
        /// <param name="actualDiagnostic">The <see cref="Diagnostic"/> that was found in the code.</param>
        /// <param name="actualLocation">The <see cref="Location"/> of the <paramref name="actualDiagnostic"/> found in the code.</param>
        /// <param name="expectedResult">The <see cref="DiagnosticResultLocation"/> that should have been found.</param>
        /// <exception cref="BadDiagnosticLocationException"></exception>
        protected virtual void VerifyDiagnosticLocation(string language, DiagnosticAnalyzer analyzer
            , DiagnosticResultLocation expectedResult, Diagnostic actualDiagnostic, Location actualLocation)
        {
            var actualSpan = actualLocation.GetLineSpan();

            if (actualSpan.Path != expectedResult.Path
                && (IsNullOrEmpty(actualSpan.Path)
                    || !actualSpan.Path.Contains($"{DefaultFilePathPrefix}0.")
                    || !expectedResult.Path.Contains($"{DefaultFilePathPrefix}.")))
            {
                throw new BadDiagnosticLocationException(
                    $"Expected diagnostic to be in file \"${expectedResult.Path}\""
                    + $" but was in file \"{actualSpan.Path}\""
                    , language, analyzer, PathMismatch, expectedResult, actualDiagnostic, actualLocation);
            }

            var actualLinePosition = actualSpan.StartLinePosition;

            if (actualLinePosition.Line > 0 && actualLinePosition.Line + 1 != expectedResult.Line)
            {
                throw new BadDiagnosticLocationException(
                    $"Expected diagnostic to be on line \"{expectedResult.Line}\""
                    + $" but was on line \"{actualLinePosition.Line + 1}\""
                    , language, analyzer, LineMismatch, expectedResult, actualDiagnostic, actualLocation);
            }

            if (actualLinePosition.Character > 0 && actualLinePosition.Character + 1 != expectedResult.Column)
            {
                throw new BadDiagnosticLocationException(
                    $"Expected diagnostic to start at column \"{expectedResult.Column}\""
                    + $" but was at column \"{actualLinePosition.Character + 1}\""
                    , language, analyzer, ColumnMismatch, expectedResult, actualDiagnostic, actualLocation);
            }
        }

        #endregion
    }
}
