using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.CodeAnalysis.Verifiers
{
    using Microsoft.CodeAnalysis;
    using static String;

    /// <summary>
    /// Struct that stores information about a Diagnostic appearing in a source
    /// </summary>
    public struct DiagnosticResult
    {
        private IEnumerable<DiagnosticResultLocation> _locations;

        /// <summary>
        /// Gets or sets the Locations.
        /// </summary>
        public IEnumerable<DiagnosticResultLocation> Locations
        {
            get => _locations ?? (_locations = new DiagnosticResultLocation[0]);
            set => _locations = value;
        }

        /// <summary>
        /// Gets or sets the Severity.
        /// </summary>
        public DiagnosticSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the Message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="messageArgs"></param>
        public DiagnosticResult(DiagnosticDescriptor descriptor, params object[] messageArgs)
        {
            _locations = null;
            Id = descriptor.Id;
            Message = Format(descriptor.MessageFormat.ToString(), messageArgs);
            Severity = descriptor.DefaultSeverity;
        }

        /// <summary>
        /// Returns the Created <see cref="DiagnosticResult"/> given <paramref name="descriptor"/>
        /// and <paramref name="locations"/>.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="locations"></param>
        /// <returns></returns>
        public static DiagnosticResult Create(DiagnosticDescriptor descriptor
            , params DiagnosticResultLocation[] locations)
            => new DiagnosticResult(descriptor) {Locations = locations};
        // TODO: TBD: may round out the Create methods...

        internal string Path => Locations.Any() ? Locations.First().Path : Empty;

        internal int Line => Locations.Any() ? Locations.First().Line : -1;

        internal int Column => Locations.Any() ? Locations.First().Column : -1;
    }
}
