using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using Xunit;

    /// <inheritdoc />
    public class EnumerationCoverageReporter<T> : IEnumerationCoverageReporter<T>
        where T : Enumeration
    {
        // TODO: TBD: I get why we did this with the Reporter...
        // TODO: TBD: however maybe we assume false until we want to do a coverage test and then Enable it...
        /// <inheritdoc />
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Tallies the Tested Names.
        /// </summary>
        protected IDictionary<string, int> TestedNames { get; } = new ConcurrentDictionary<string, int>();

        /// <inheritdoc />
        public void Report(params string[] names)
        {
            names.AssertNotEmpty();

            foreach (var name in names)
            {
                if (TestedNames.ContainsKey(name))
                {
                    TestedNames[name]++;
                    continue;
                }

                TestedNames.Add(name, 1);
            }
        }

        /// <inheritdoc />
        public void Report(params Enumeration[] values) => Report(values.Select(x => x.Name).ToArray());

        /// <summary>
        /// Gets whether the object IsDisposed.
        /// </summary>
        protected bool IsDisposed { get; private set; }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            // ReSharper disable once InvertIf
            if (disposing && !IsDisposed)
            {
                if (Enabled)
                {
                    Enumeration.GetValues<T>().VerifyValuesCoverage(TestedNames);
                }
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            IsDisposed = true;
        }
    }
}
