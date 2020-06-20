using System.Collections.Generic;

namespace Ellumination.Collections.Keyed.Flags
{
    /// <summary>
    /// Provides an <see cref="ImmutableBitArray"/> based <see cref="Enumeration{TKey,T}"/>
    /// base class. We call it Base so as not to be confused with the FlagsEnumerationAttribute
    /// we plan to offer later on.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract partial class Enumeration<T> : Enumeration<ImmutableBitArray, T>
        where T : Enumeration<T>
    {
        /// <summary>
        /// Gets the DefaultKeyBytes, a One Byte Array bearing a single Default Byte value.
        /// </summary>
        private static IEnumerable<byte> DefaultKeyBytes => new[] {default(byte)};

        /// <summary>
        /// Returns the <see cref="ImmutableBitArray"/> correlating to <paramref name="bytes"/>.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        /// <see cref="DefaultKeyBytes"/>
        private static ImmutableBitArray GetDefaultKey(IEnumerable<byte> bytes = null)
            => ImmutableBitArray.FromBytes(bytes ?? DefaultKeyBytes);

        /// <summary>
        /// Gets the DefaultKey. Basically a one <see cref="byte"/> Zero Key.
        /// </summary>
        /// <see cref="GetDefaultKey"/>
        private static ImmutableBitArray DefaultKey => GetDefaultKey();

        /// <summary>
        /// Protected Default Constructor.
        /// </summary>
        protected Enumeration()
            : this(DefaultKey, null, null)
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="name"></param>
        protected Enumeration(string name)
            : base(DefaultKey, name, null)
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        protected Enumeration(string name, string displayName)
            : base(DefaultKey, name, displayName)
        {
        }

        /// <summary>
        /// Protected Default Constructor.
        /// </summary>
        /// <param name="bytes"></param>
        protected Enumeration(IEnumerable<byte> bytes)
            : base(GetDefaultKey(bytes), null, null)
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="name"></param>
        protected Enumeration(IEnumerable<byte> bytes, string name)
            : base(GetDefaultKey(bytes), name, null)
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        protected Enumeration(IEnumerable<byte> bytes, string name, string displayName)
            : base(GetDefaultKey(bytes), name, displayName)
        {
        }

        /// <summary>
        /// Protected Default Constructor.
        /// </summary>
        /// <param name="bits"></param>
        protected Enumeration(ImmutableBitArray bits)
            : base(bits ?? DefaultKey, null, null)
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="bits"></param>
        /// <param name="name"></param>
        protected Enumeration(ImmutableBitArray bits, string name)
            : base(bits ?? DefaultKey, name, null)
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="bits"></param>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        protected Enumeration(ImmutableBitArray bits, string name, string displayName)
            : base(bits ?? DefaultKey, name, displayName)
        {
        }
    }
}
