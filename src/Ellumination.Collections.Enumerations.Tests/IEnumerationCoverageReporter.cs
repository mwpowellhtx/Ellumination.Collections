using System;

namespace Ellumination.Collections
{
    using Xunit;

    // ReSharper disable once UnusedTypeParameter
    /// <summary>
    /// <see cref="Enumeration"/> coverage reporter. Disposal occurs transparently
    /// when we inject it via <see cref="IClassFixture{TFixture}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <inheritdoc />
    public interface IEnumerationCoverageReporter<T> : IDisposable
        where T : Enumeration
    {
        /// <summary>
        /// Gets or sets whether the Reporter is considered Enabled for purposes
        /// of the current testing context.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Reports the <paramref name="names"/> that were tested.
        /// </summary>
        /// <param name="names"></param>
        /// <see cref="Report(Enumeration[])"/>
        void Report(params string[] names);

        /// <summary>
        /// Reports the <paramref name="values"/> that were tested.
        /// </summary>
        /// <param name="values"></param>
        /// <see cref="Report(string[])"/>
        void Report(params Enumeration[] values);
    }
}
