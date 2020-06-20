using System.Diagnostics;

namespace Ellumination.Collections
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplayName) + "}")]
    public abstract partial class Enumeration
    {
        /// <summary>
        /// Override in order to provide a Debugger Display Name. Defaults either
        /// <see cref="DisplayName"/> or the name of the <see cref="Enumeration"/> type.
        /// </summary>
        protected internal virtual string DebuggerDisplayName => DisplayName ?? nameof(Enumeration);

        /// <summary>
        /// DisplayName backing field.
        /// </summary>
        private string _displayName;

        /// <summary>
        /// Gets the DisplayName.
        /// </summary>
        public virtual string DisplayName
        {
            get => _displayName ?? (_displayName = ResolveHumanReadableCamelCase(Name));
            protected internal set => _displayName = value;
        }

        /// <summary>
        /// Resolves the <paramref name="s"/> Source as a Human Readable Camel Case string.
        /// Tries to resolve a DisplayName.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        protected static string ResolveHumanReadableCamelCase(string s) => s.GetHumanReadableCamelCase();
    }
}
