using System;
using System.Linq;
using System.Reflection;

namespace Ellumination.Collections
{
    using Xunit;
    using static BindingFlags;

    /// <summary>
    /// Set of Internal extension methods used in support of the
    /// <see cref="EnumerationTestFixtureBase{T}"/>. These methods are necessary in order to
    /// isolate specific use cases for purposes of vetting the good, bad, and ugly test case
    /// paths in the tests tests.
    /// </summary>
    public static class EnumerationTestCaseExtensionMethods
    {
        /// <summary>
        /// Returns the <see cref="Enumeration"/> <typeparamref name="T"/> Value by its
        /// <paramref name="name"/>. <paramref name="_"/> is provided to connect the caller
        /// with the <see cref="Enumeration"/> <typeparamref name="T"/>. Nothing more,
        /// nothing less.
        /// </summary>
        /// <param name="_"></param>
        /// <param name="name"></param>
        /// <param name="comparisonType"></param>
        /// <param name="verify"></param>
        /// <returns></returns>
        /// <see cref="Enumeration.FromName{T}(string)"/>
        /// <see cref="Enumeration.FromName{T}(string,StringComparison)"/>
        public static T GetValueByName<T>(this T _, string name
            , StringComparison? comparisonType = null, Action<T> verify = null)
            where T : Enumeration
        {
            var result = comparisonType == null
                ? Enumeration.FromName<T>(name).AssertNotNull()
                : Enumeration.FromName<T>(name, comparisonType.Value);
            var value = result.AssertNotNull().AssertEqual(name, x => x.Name);
            verify?.Invoke(value);
            return value;
        }

        /// <summary>
        /// Returns the <see cref="Enumeration"/> <typeparamref name="T"/> Value by its
        /// <paramref name="displayName"/>. <paramref name="_"/> is provided to connect the
        /// caller with the <see cref="Enumeration"/> <typeparamref name="T"/>. Nothing more,
        /// nothing less.
        /// </summary>
        /// <param name="_"></param>
        /// <param name="displayName"></param>
        /// <param name="comparisonType"></param>
        /// <param name="verify"></param>
        /// <returns></returns>
        /// <see cref="Enumeration.FromDisplayName{T}(string)"/>
        /// <see cref="Enumeration.FromDisplayName{T}(string,StringComparison)"/>
        public static T GetValueByDisplayName<T>(this T _, string displayName
            , StringComparison? comparisonType = null, Action<T> verify = null)
            where T : Enumeration
        {
            var result = comparisonType == null
                ? Enumeration.FromDisplayName<T>(displayName).AssertNotNull()
                : Enumeration.FromDisplayName<T>(displayName, comparisonType.Value);
            var value = result.AssertNotNull().AssertEqual(displayName, x => x.DisplayName);
            verify?.Invoke(value);
            return value;
        }

        /// <summary>
        /// Connect the verification with the <see cref="Enumeration"/> <paramref name="_"/> only.
        /// We do not require the connection with the test class itself. In fact, we want it to be
        /// separate, so we can do some independent verification of different good, bad, or ugly
        /// use cases. We expect there to be no <see cref="Public"/>, <see cref="Instance"/>
        /// Constructors.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_">An anchor value rooting the Type <typeparamref name="T"/> only.</param>
        /// <param name="reporter">Disables the Reported following this invocation.</param>
        /// <see cref="Public"/>
        /// <see cref="Instance"/>
        /// <see cref="Type.GetConstructors(BindingFlags)"/>
        internal static void ShallNotHaveAnyPublicCtors<T>(this T _, IEnumerationCoverageReporter<T> reporter = null)
            where T : Enumeration
        {
            if (reporter != null)
            {
                reporter.Enabled = false;
            }

            // We do not care about the Value apart from identifying the underlying Enumeration Type.
            typeof(T).GetConstructors(Public | Instance).AssertEmpty();
        }

        /// <summary>
        /// Connect the verification with the <see cref="Enumeration"/> <paramref name="value"/>
        /// only. We do not require the connection with the test class itself. In fact, we want
        /// it to be separate, so we can do some independent verification of different good, bad,
        /// or ugly use cases. Returns whether the type <typeparamref name="T"/> Has the Expected
        /// Bitwise Constructors. That is, there is One <see cref="NonPublic"/>,
        /// <see cref="Instance"/> Constructor accepting a single <see cref="int"/> parameter.
        /// The Constructor must also be  <see cref="MethodBase.IsPrivate"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="ctorInspectors"></param>
        /// <see cref="ConstructorInfo"/>
        /// <see cref="MethodBase.IsPrivate"/>
        /// <see cref="NonPublic"/>
        /// <see cref="Instance"/>
        /// <see cref="T:byte[]"/>
        internal static void HasExpectedCtors<T>(this T value, params Action<ConstructorInfo>[] ctorInspectors)
            where T : Enumeration
        {
            var type = typeof(T);
            const BindingFlags nonPublicInstance = NonPublic | Instance;
            var ctors = type.GetConstructors(nonPublicInstance).AssertNotNull().AssertNotEmpty();
            ctors.OrderBy(ctorInfo => ctorInfo.GetParameters().Length).AssertCollection(ctorInspectors);
        }

        /// <summary>
        /// Verifies that the type <typeparamref name="T"/> Shall Have At Least One Value. We do
        /// not care about the <paramref name="_"/> itself apart from its connection with the
        /// generic type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_">Given a pass through anchor value rooting the Type <typeparamref name="T"/>.</param>
        /// <param name="reporter">Reporter is disabled following this invocation.</param>
        /// <returns>The pass through <paramref name="_"/> value.</returns>
        /// <see cref="Enumeration.GetValues{T}"/>
        internal static T ShallHaveAtLeastOneValue<T>(this T _, IEnumerationCoverageReporter<T> reporter = null)
            where T : Enumeration
        {
            if (reporter != null)
            {
                reporter.Enabled = false;
            }

            /* Do not Report any Names at this level; this is intentional. Rather, do leave that
             * level of reporting for the actual unit test coverage. This ensures that we enforce
             * appropriate Enumeration Test Coverage at appropriate levels. */
            Enumeration.GetValues<T>().AssertNotNull().AssertNotEmpty();

            return _;
        }
    }
}
