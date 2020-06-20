using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ellumination.Collections
{
    using Xunit;
    using Xunit.Abstractions;
    using static BindingFlags;

    public class ImmutableBitArrayInterfaceTests : SubjectTestFixtureBase<ImmutableBitArray>
    {
        public ImmutableBitArrayInterfaceTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        // ReSharper disable once UnusedParameter.Local
        /// <summary>
        /// Returns the Type Of <typeparamref name="T"/>. The parameter itself is not
        /// actually used except as a feeder for Generic <see cref="Type"/> discovery.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        /// <returns></returns>
        private static Type GetTypeOf<T>(T _) => typeof(T);

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private static void VerifyMethodSpecification<TReturn>(string methodName
            , BindingFlags bindingAttr, params Action<ParameterInfo>[] havingArgs)
        {
            var actualMethod = SubjectType.GetMethod(methodName, bindingAttr);
            Assert.NotNull(actualMethod);
            Assert.Equal(typeof(TReturn), actualMethod.ReturnType);
            var args = actualMethod.GetParameters();
            Assert.Collection(args, havingArgs);
        }

        [Fact]
        public void Class_should_have_FromBytes_method()
            => VerifyMethodSpecification<ImmutableBitArray>(
                nameof(ImmutableBitArray.FromBytes)
                , Public | Static
                , pi =>
                {
                    IEnumerable<byte> bytes = null;
                    // ReSharper disable once ExpressionIsAlwaysNull
                    Assert.Equal(GetTypeOf(bytes), pi.ParameterType);
                    Assert.Equal(nameof(bytes), pi.Name);
                }
                , pi =>
                {
                    const bool msb = true;
                    Assert.Equal(GetTypeOf(msb), pi.ParameterType);
                    Assert.Equal(nameof(msb), pi.Name);
                    Assert.Equal(msb, pi.DefaultValue);
                });

        [Fact]
        public void Class_should_have_FromInts_method()
            => VerifyMethodSpecification<ImmutableBitArray>(
                nameof(ImmutableBitArray.FromInts)
                , Public | Static
                , pi =>
                {
                    IEnumerable<uint> uints = null;
                    // ReSharper disable once ExpressionIsAlwaysNull
                    Assert.Equal(GetTypeOf(uints), pi.ParameterType);
                    Assert.Equal(nameof(uints), pi.Name);
                });
    }
}
