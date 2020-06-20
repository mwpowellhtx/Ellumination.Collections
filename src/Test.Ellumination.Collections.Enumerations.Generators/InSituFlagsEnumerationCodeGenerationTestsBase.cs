using System;
using System.Linq;
using System.Reflection;

namespace Ellumination.Collections
{
    using Keyed.Flags;
    using Xunit;
    using static BindingFlags;

    /// <summary>
    /// This provides a handful of in-situ <see cref="FlagsEnumerationAttribute"/> Code Generation
    /// unit tests. This means that we expect actual code generation to have taken place when
    /// building this assembly. Therefore, in simple terms, we should have a handful of known
    /// artifacts leading up to that point, and as a result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class InSituFlagsEnumerationCodeGenerationTestsBase<T> : EnumerationTestsBase<T>
        where T : Enumeration<T>
    {
        private static readonly Type EnumerationType = typeof(T);

        /// <summary>
        /// The user-specified definition provides for a Static, non-Instance, Constructor.
        /// There is also a Private, Default Constructor. Whereas, Code Generation enabled
        /// by the <see cref="FlagsEnumerationAttribute"/> affords a Private Constructor
        /// accepting a <see cref="byte"/> array.
        /// </summary>
        [Fact]
        public void Expected_Ctors_are_all_valid()
        {
            // Verify Public and Non-Public, Instance or Static, just to be clear.
            var ctors = EnumerationType.GetConstructors(Instance | Static | Public | NonPublic)
                .OrderByDescending(ctor => ctor.IsStatic)
                .ThenBy(ctor => ctor.IsPublic)
                .ThenBy(ctor => ctor.GetParameters().Length).ToArray();

            Assert.Collection(ctors
                , staticCtor => { Assert.True(staticCtor.IsStatic); }
                , defaultCtor =>
                {
                    Assert.False(defaultCtor.IsStatic);
                    Assert.True(defaultCtor.IsPrivate);
                    Assert.Empty(defaultCtor.GetParameters());
                }
                , bitwiseOperatorsCtor =>
                {
                    Assert.True(bitwiseOperatorsCtor.IsPrivate);
                    Assert.False(bitwiseOperatorsCtor.IsStatic);
                    Assert.Collection(bitwiseOperatorsCtor.GetParameters()
                        , bytes =>
                        {
                            Assert.Equal(nameof(bytes), bytes.Name);
                            Assert.Equal(typeof(byte[]), bytes.ParameterType);
                        }
                    );
                }
            );
        }

        /// <summary>
        /// Verifies whether the <see cref="EnumerationType"/> Has the Method corresponding
        /// with the <paramref name="expectedMethodName"/>. We also know the methods in question
        /// to be <see cref="MethodInfo.IsStatic"/>, <see cref="MethodBase.IsPublic"/>, and to
        /// have parameters aligned with <see cref="paramPredicates"/>, for starters.
        /// </summary>
        /// <param name="expectedMethodName"></param>
        /// <param name="expectedReturnType"></param>
        /// <param name="allParamsPredicate"></param>
        /// <param name="paramPredicates"></param>
        private static void VerifyHasMethod(string expectedMethodName, Type expectedReturnType
            , Action<ParameterInfo> allParamsPredicate, params Action<ParameterInfo>[] paramPredicates)
        {
            // Do a bit of vetting of the parameters themselves.
            Assert.NotNull(expectedReturnType);
            Assert.NotNull(allParamsPredicate);
            Assert.NotEmpty(paramPredicates);
            Assert.All(paramPredicates, Assert.NotNull);

            var methodInfo = EnumerationType.GetMethods().SingleOrDefault(
                m => m.IsStatic && m.IsPublic && m.Name == expectedMethodName
                     && m.GetParameters().Length == paramPredicates.Length);
            Assert.NotNull(methodInfo);

            var parameters = methodInfo.GetParameters();
            Assert.All(parameters, allParamsPredicate);
            Assert.Collection(parameters, paramPredicates);
            Assert.Equal(expectedReturnType, methodInfo.ReturnType);
        }

        /// <summary>
        /// Verifies whether the <see cref="EnumerationType"/> defines the Ones Complement
        /// operator overload. Although this is a Unary Operator Overload, this is pretty
        /// much the one isolated case, so we can vet the issue as a <see cref="FactAttribute"/>.
        /// </summary>
        [Fact]
        public void Has_Ones_Complement_operator_overload()
            => VerifyHasMethod("op_OnesComplement", EnumerationType
                , p => Assert.Equal(EnumerationType, p.ParameterType)
                , other => Assert.Equal(nameof(other), other.Name)
            );

        /// <summary>
        /// Verifies whether the <see cref="EnumerationType"/> defines the Binary Bitwise
        /// operator overloads.
        /// </summary>
        /// <param name="methodName"></param>
        [Theory
         , InlineData("op_BitwiseAnd")
         , InlineData("op_BitwiseOr")
         , InlineData("op_ExclusiveOr")]
        public void Has_Binary_operator_overloads(string methodName)
            => VerifyHasMethod(methodName, EnumerationType
                , p => Assert.Equal(EnumerationType, p.ParameterType)
                , a => Assert.Equal(nameof(a), a.Name)
                , b => Assert.Equal(nameof(b), b.Name)
            );
    }
}
