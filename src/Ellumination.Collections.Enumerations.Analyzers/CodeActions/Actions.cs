using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

namespace Ellumination.Collections
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using static SyntaxFactory;
    using static SyntaxKind;

    internal static class Actions
    {
        private class MakePartialCodeAction : CodeAction
        {
            public MakePartialCodeAction(Document document, SyntaxNode root, ClassDeclarationSyntax classDecl)
            {
                Document = document;
                Root = root;
                ClassDecl = classDecl;
            }

            /// <summary>
            /// Make Flags Enumeration Partial
            /// </summary>
            /// <inheritdoc />
            public override string Title => "Make Flags Enumeration Partial";

            public override string EquivalenceKey => Title;

            private Document Document { get; }

            private SyntaxNode Root { get; }

            private ClassDeclarationSyntax ClassDecl { get; }

            protected override Task<Document> GetChangedDocumentAsync(CancellationToken cancellationToken)
            {
                return Task.Run(() =>
                {
                    var modifiedNode = ClassDecl.AddModifiers(Token(PartialKeyword));
                    return Document.WithSyntaxRoot(Root.ReplaceNode(ClassDecl, modifiedNode));
                }, cancellationToken);
            }
        }

        public static CodeAction MakePartial(Document document, SyntaxNode root, ClassDeclarationSyntax classDecl)
            => new MakePartialCodeAction(document, root, classDecl);
    }
}
