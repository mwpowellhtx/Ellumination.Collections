using System;
using System.Linq;
using System.Reflection;

namespace Ellumination.Collections
{
    using Xunit;
    using Xunit.Abstractions;
    using static Type;
    using static BindingFlags;

    /// <summary>
    /// 
    /// </summary>
    public static class FlagsEnumerationTestCaseExtensionMethods
    {
        /// <summary>
        /// Returns the <see cref="Keyed.Flags.Enumeration{T}"/> <typeparamref name="T"/> Value
        /// by its <paramref name="bytes"/>. <paramref name="_"/> is provided to connect the
        /// caller with the <see cref="Keyed.Flags.Enumeration{T}"/> <typeparamref name="T"/>,
        /// nothing more, nothing less.
        /// </summary>
        /// <param name="_">Given an anchor value identifying the Type <typeparamref name="T"/>.</param>
        /// <param name="bytes">The Bytes correlating to the <see cref="ImmutableBitArray"/>.</param>
        /// <returns></returns>
        /// <see cref="ImmutableBitArray"/>
        /// <see cref="Keyed.Flags.Enumeration{T}.FromBitArray"/>
        public static T GetValueByBytes<T>(this T _, byte[] bytes)
            where T : Keyed.Flags.Enumeration<T>
        {
            var key = new ImmutableBitArray(bytes);
            var value = Keyed.Flags.Enumeration<T>.FromBitArray(key).AssertNotNull();
            value.Key.AssertNotNull().AssertEqual(key);
            return value;
        }

        /// <summary>
        /// Connect the verification with the <see cref="Keyed.Flags.Enumeration{T}"/>
        /// <paramref name="_"/> only. We do not require the connection with the test class
        /// itself. In fact, we want that to be separate and distinct in order so that we may
        /// do some independent verification of good, bad, or ugly test cases. Returns whether
        /// the Type <typeparamref name="T"/> Has the Expected Flags Constructors. That is,
        /// whether there is One <see cref="NonPublic"/>, <see cref="Instance"/> Constructor
        /// accepting a single <see cref="byte"/> Array. The Constructor must also be
        /// <see cref="MethodBase.IsPrivate"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_">Given a root value in order to inform the <typeparamref name="T"/> Type.</param>
        /// <param name="reporter"></param>
        /// <see cref="ConstructorInfo"/>
        /// <see cref="MethodBase.IsPrivate"/>
        /// <see cref="NonPublic"/>
        /// <see cref="Instance"/>
        /// <see cref="T:byte[]"/>
        internal static void HasExpectedFlagsCtors<T>(this T _, IEnumerationCoverageReporter<T> reporter = null)
            where T : Keyed.Flags.Enumeration<T>
        {
            if (reporter != null)
            {
                reporter.Enabled = false;
            }

            const BindingFlags nonPublicInstance = NonPublic | Instance;
            var types = new[] {typeof(byte[])};
            var ctor = typeof(T).GetConstructor(nonPublicInstance, DefaultBinder, types, null);
            ctor.AssertNotNull().AssertTrue(x => x.IsPrivate);
        }

        /// <summary>
        /// <paramref name="value"/> is used to connect the caller with the underlying
        /// <see cref="Keyed.Flags.Enumeration{T}"/> type only. Additionally, we are making
        /// the assumption that <see cref="Enumeration.GetValues{T}"/> are unique not only
        /// in <see cref="Enumeration.Name"/>, but also in
        /// <see cref="Keyed.Enumeration{TKey}.Key"/> value. In truth, this could go either
        /// way, but for now I will expect that there must be uniqueness throughout.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="reporter"></param>
        /// <param name="outputHelper"></param>
        /// <see cref="!:http://en.wikipedia.org/wiki/Enumeration"/>
        public static void KeysShallBeUniquelyAssigned<T>(this T value
            , IEnumerationCoverageReporter<T> reporter = null
            , ITestOutputHelper outputHelper = null)
            where T : Keyed.Flags.Enumeration<T>
            => value.KeysShallBeUniquelyAssigned<ImmutableBitArray, uint, T>(
                x => x.ToInts().Single(), reporter, outputHelper
            );
    }
}
