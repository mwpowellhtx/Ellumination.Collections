using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Ellumination.CodeAnalysis.Verifiers
{
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Text;
    using static LanguageNames;
    using static OutputKind;

    // TODO: TBD: Ostensibly this could potentially even be separated to a Verifiers.Documents assembly all its own...
    // TODO: TBD: Will pursue that avenue if the situation calls for it...
    /// <summary>
    /// 
    /// </summary>
    public abstract partial class SourceDocumentVerifier
    {
        /// <summary>
        /// Test
        /// </summary>
        public const string DefaultFilePathPrefix = "Test";

        /// <summary>
        /// cs
        /// </summary>
        public const string CSharpDefaultFileExt = "cs";

        ///// <summary>
        ///// vb
        ///// </summary>
        //public const string VisualBasicDefaultExt = "vb";

        /// <summary>
        /// TestProject
        /// </summary>
        public const string TestProjectName = "TestProject";

        /// <summary>
        /// Returns the set of <see cref="Document"/> instances corresponding with the
        /// <paramref name="sources"/> in the given <paramref name="language"/>.
        /// </summary>
        /// <param name="sources">The source code that will land in each <see cref="Document"/>.</param>
        /// <param name="language">The programming language in which the <paramref name="sources"/> is given.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        protected IEnumerable<Document> GetDocuments(IEnumerable<string> sources, string language)
        {
            var languages = new[] {CSharp, VisualBasic};

            if (languages.All(l => l != language))
            {
                throw new ArgumentException($"Unsupported language \"{language}\"", nameof(language));
            }

            // ReSharper disable once PossibleMultipleEnumeration
            var project = CreateProject(sources, language);
            var documents = project.Documents;

            // ReSharper disable once PossibleMultipleEnumeration
            if (sources.Count()
                // ReSharper disable once PossibleMultipleEnumeration
                != documents.Count())
            {
                // ReSharper disable once PossibleMultipleEnumeration
                throw new ArgumentException(
                    $"Number of Sources ({sources.Count()})"
                    // ReSharper disable once PossibleMultipleEnumeration
                    + $" did not match number of Documents ({documents.Count()}) created", nameof(sources));
            }

            return project.Documents;
        }

        // TODO: TBD: this one may be unnecessary, strictly speaking...
        /// <summary>
        /// Create a <see cref="Document"/> from a string through creating a <see cref="Project"/>
        /// that contains it.
        /// </summary>
        /// <param name="source">Source in the form of a string.</param>
        /// <param name="language">The <paramref name="language"/> the <paramref name="source"/>
        /// are in.</param>
        /// <returns>A <see cref="Document"/> created from the <paramref name="source"/>.</returns>
        protected Document CreateDocument(string source, string language = CSharp)
            => CreateProject(new[] {source}, language).Documents.First();

        /// <summary>
        /// 
        /// </summary>
        protected event EventHandler<DocumentFileExtensionRequiredEventArgs> DocumentFileExtensionRequired;

        private void OnDocumentFileExtensionRequired(string language, out DocumentFileExtensionRequiredEventArgs e)
        {
            e = new DocumentFileExtensionRequiredEventArgs(language);
            DocumentFileExtensionRequired?.Invoke(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        protected event EventHandler<MetadataReferencesRequiredEventArgs> MetadataReferencesRequired;

        private void OnMetadataReferencesRequired(out MetadataReferencesRequiredEventArgs e)
        {
            e = new MetadataReferencesRequiredEventArgs();
            MetadataReferencesRequired?.Invoke(this, e);
        }

        /// <summary>
        /// Create a <see cref="Project"/> using the input strings <paramref name="sources"/>.
        /// </summary>
        /// <param name="sources">Sources in the form of strings.</param>
        /// <param name="language">The <paramref name="language"/> the <paramref name="sources"/>
        /// are in.</param>
        /// <returns>A <see cref="Project"/> created out of the <see cref="Document"/> created from
        /// the <paramref name="sources"/>.</returns>
        private Project CreateProject(IEnumerable<string> sources, string language = CSharp)
        {
            var projectId = ProjectId.CreateNewId(TestProjectName);

            OnMetadataReferencesRequired(out var metadataReferenceArgs);

            var sln = new AdhocWorkspace().CurrentSolution
                    .AddProject(projectId, TestProjectName, TestProjectName, language)
                    .AddMetadataReferences(projectId, metadataReferenceArgs.MetadataReferences)
                ;

            var count = 0;

            OnDocumentFileExtensionRequired(language, out var fileExtensionArgs);

            foreach (var source in sources)
            {
                var newFileName = $"{DefaultFilePathPrefix}{count++}.{fileExtensionArgs.FileExtension}";
                var documentId = DocumentId.CreateNewId(projectId, newFileName);
                sln = sln.AddDocument(documentId, newFileName, SourceText.From(source));
            }

            // TODO: TBD: there is no way to set the target framework creating a Project in this manner?
            // TODO: TBD: 
            var project = sln.GetProject(projectId).WithCompilationOptions(
                new CSharpCompilationOptions(DynamicallyLinkedLibrary)
            );

            return project;
        }
    }
}
