using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Ellumination.CodeAnalysis.Verifiers
{
    using static LanguageNames;

    /// <summary>
    /// 
    /// </summary>
    /// <inheritdoc />
    public sealed class DocumentFileExtensionRequiredEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public string Language { get; }

        private static IEnumerable<Tuple<string, string>> GetDefaultExtensions()
        {
            yield return Tuple.Create(CSharp, "cs");
            yield return Tuple.Create(VisualBasic, "vb");
            yield return Tuple.Create(FSharp, "fs");
        }

        /// <summary>
        /// Gets the Default File Extensions.
        /// </summary>
        /// <see cref="CSharp"/>
        /// <see cref="FSharp"/>
        /// <see cref="VisualBasic"/>
        public IReadOnlyDictionary<string, string> DefaultExtensions { get; }
            = new ReadOnlyDictionary<string, string>(GetDefaultExtensions()
                .ToDictionary(x => x.Item1, x => x.Item2));

        /// <summary>
        /// Gets or sets the File Extension for the <see cref="Language"/>.
        /// </summary>
        /// <see cref="DefaultExtensions"/>
        public string FileExtension { get; set; }

        internal DocumentFileExtensionRequiredEventArgs()
            : this(CSharp)
        {
        }

        internal DocumentFileExtensionRequiredEventArgs(string language)
        {
            Language = DefaultExtensions.ContainsKey(language) ? language : CSharp;
            FileExtension = DefaultExtensions[Language];
        }
    }
}
