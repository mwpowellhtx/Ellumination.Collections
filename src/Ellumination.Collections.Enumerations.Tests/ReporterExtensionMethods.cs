using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using Xunit;
    using static String;

    /// <summary>
    /// Reporter extension methods provided for shorthand.
    /// </summary>
    public static class ReporterExtensionMethods
    {
        internal static void VerifyValuesCoverage(this IEnumerable<Enumeration> values, IDictionary<string, int> coverage)
        {
            /* This is a hard exception. If this occurs, we have other problems to contend with.
             * Think it through, there need to be at least One item in the Values array for this
             * to be useful. */

            // ReSharper disable once InconsistentNaming
            var __values = values.AssertNotNull().AssertNotEmpty().ToArray();

            try
            {
                // Then, we expect each of the Values to be Evaluated.
                __values.AssertEqual(coverage.Count, x => x.Length);

                // Each of the Values shall be Evaluated at least Once.
                coverage.Values.AssertTrue(x => x.All(count => count > 0));
            }
            catch (Exception ex)
            {
                // TODO: TBD: Assert inconclusive how? i.e. NUnit provides Assert.Inconclusive(...).
                var incomplete = __values.Select(x => x.Name).Except(coverage.Keys)
                    .Aggregate(Empty, (g, x) => IsNullOrEmpty(g) ? $"'{x}'" : $"{g}, '{x}'");

                // TODO: TBD: for lack of a better way of signaling, just throw the IOEX here...
                throw new InvalidOperationException($"Incomplete test coverage: [ {incomplete} ]", ex);
            }
        }
    }
}
