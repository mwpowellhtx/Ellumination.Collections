using System;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;

namespace Ellumination.CodeAnalysis
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using static SyntaxFactory;
    using static BindingFlags;

    // TODO: TBD: these really belong in a Ellumination.CodeAnalysis.Verification / intentionally remaining decoupled from test framework dependency of consumer's choice...
    /// <summary>
    /// Provides a set of Syntax Verification Extension Methods.
    /// </summary>
    public static class SyntaxVerificationExtensionMethods
    {
        /// <summary>
        /// Returns whether the <paramref name="node"/> Is A <typeparamref name="T"/>.
        /// Additionally, matches the Node using the <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool IsA<T>(this CSharpSyntaxNode node, Func<T, bool> predicate)
            where T : MemberDeclarationSyntax
            => node is T member && predicate(member);

        // TODO: TBD: may also reconsider the async functionality in the core verifiers on account of errors here tripping up the async aspects...
        /// <summary>
        /// Returns whether the <paramref name="node"/> Has the <see cref="SyntaxList{T}"/>
        /// <see cref="MemberDeclarationSyntax"/> Members property. Returns whether this is
        /// the case, and there are <paramref name="predicates"/> and candidates in the Members
        /// list matching those predicates.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="predicates"></param>
        /// <returns></returns>
        public static bool HasAllMembers<T>(this CSharpSyntaxNode node
            , params Func<T, bool>[] predicates)
            where T : MemberDeclarationSyntax
        {
            var property = node.GetType().GetProperty("Members", Public | Instance | GetProperty);

            // The Type should be vetted, but let's double check anyway.
            var members = property?.GetValue(node);

            if (!(members is SyntaxList<MemberDeclarationSyntax> memberList))
            {
                return false;
            }

            var candidates = memberList.OfType<T>();
            return predicates.Any() && predicates.All(p => candidates.Single(p) != null);
        }

        /// <summary>
        /// Returns whether the <paramref name="baseMethodDecl"/> Has All the Parameters matching
        /// the <paramref name="predicates"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseMethodDecl"></param>
        /// <param name="predicates"></param>
        /// <returns></returns>
        public static bool HasAllParameters<T>(this T baseMethodDecl, params Func<ParameterSyntax, bool>[] predicates)
            where T : BaseMethodDeclarationSyntax
        {
            // Looking past any of the grammatical separators, etc.
            var parameters = baseMethodDecl.ParameterList.Parameters.OfType<ParameterSyntax>();
            return predicates.Any() && predicates.All(p => parameters.SingleOrDefault(p) != null);
        }

        /// <summary>
        /// Returns whether the <paramref name="baseMethodDecl"/> Has Exactly the Parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseMethodDecl"></param>
        /// <param name="predicates"></param>
        /// <returns></returns>
        public static bool HasExactParameters<T>(this T baseMethodDecl, params Func<ParameterSyntax, bool>[] predicates)
            where T : BaseMethodDeclarationSyntax
        {
            var parameters = baseMethodDecl.ParameterList.Parameters;
            return predicates.Any()
                   && predicates.Length == parameters.Count
                   && predicates.Zip(parameters, (predicate, parameter) => predicate(parameter)).All(match => match);
        }

        /// <summary>
        /// Returns whether the <paramref name="baseMethodDecl"/> matches the
        /// <paramref name="methodPredicate"/> and its Parameters All match the
        /// <paramref name="paramPredicates"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseMethodDecl"></param>
        /// <param name="methodPredicate"></param>
        /// <param name="paramPredicates"></param>
        /// <returns></returns>
        public static bool HasAllParameters<T>(this T baseMethodDecl, Func<T, bool> methodPredicate
            , params Func<ParameterSyntax, bool>[] paramPredicates)
            where T : BaseMethodDeclarationSyntax
        {
            // Looking past any of the grammatical separators, etc.
            var parameters = baseMethodDecl.ParameterList.Parameters.OfType<ParameterSyntax>();
            return methodPredicate(baseMethodDecl)
                   && paramPredicates.Any()
                   && paramPredicates.All(p => parameters.SingleOrDefault(p) != null);
        }

        // TODO: TBD: consider a "HasSyntaxTokens" paradigm; difference being that it looks for EXACTLY the given tokens, possibly also in the expected order... could even do a LINQ zip along these lines...
        /// <summary>
        /// Returns whether the <paramref name="tokenList"/> Has All of the
        /// <paramref name="expectedTokens"/>. They can be in any order just as long as All
        /// of them are found at some point in the <paramref name="tokenList"/>.
        /// </summary>
        /// <param name="tokenList"></param>
        /// <param name="expectedTokens"></param>
        /// <returns></returns>
        public static bool HasAllSyntaxTokens(this SyntaxTokenList tokenList, params SyntaxKind[] expectedTokens)
        {
            var candidates = (from t in tokenList select (SyntaxToken?)t).ToArray();
            return expectedTokens.Any()
                   && expectedTokens.All(token => candidates.Single(t => $"{t}" == $"{Token(token)}") != null);
        }

        /// <summary>
        /// Returns whether <paramref name="tokenList"/> Has the
        /// <paramref name="expectedTokens"/>. They can be in any order, but the expected tokens
        /// must All be found in the <paramref name="tokenList"/>. There also cannot be any
        /// duplicates in <paramref name="expectedTokens"/>.
        /// </summary>
        /// <param name="tokenList"></param>
        /// <param name="expectedTokens"></param>
        /// <returns></returns>
        public static bool HasSyntaxTokens(this SyntaxTokenList tokenList, params SyntaxKind[] expectedTokens)
        {
            var candidates = (from t in tokenList select (SyntaxToken?)t).ToArray();
            return expectedTokens.Length == expectedTokens.Distinct().Count()
                   && tokenList.Count == expectedTokens.Length
                   && expectedTokens.All(token => candidates.Single(t => $"{t}" == $"{Token(token)}") != null);
        }

        /// <summary>
        /// Returns whether <paramref name="tokenList"/> Has the Exact set of
        /// <paramref name="expectedTokens"/> in the same order. The expected tokens must All
        /// be found in the <paramref name="tokenList"/>. There also cannot be any duplicates
        /// in <paramref name="expectedTokens"/>.
        /// </summary>
        /// <param name="tokenList"></param>
        /// <param name="expectedTokens"></param>
        /// <returns></returns>
        public static bool HasExactSyntaxTokens(this SyntaxTokenList tokenList, params SyntaxKind[] expectedTokens)
            => expectedTokens.Length == expectedTokens.Distinct().Count()
               && tokenList.Count == expectedTokens.Length
               && tokenList.Zip(expectedTokens, (a, b) => $"{a}" == $"{Token(b)}").All(x => x);
    }
}
