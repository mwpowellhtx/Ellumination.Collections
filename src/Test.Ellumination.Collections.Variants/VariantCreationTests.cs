using System;

namespace Ellumination.Collections.Variants
{
    using Xunit;
    using Xunit.Abstractions;
    using static String;
    using static VariantCreationTests.ExceptionDataKeys;

    // ReSharper disable RedundantTypeArgumentsOfMethod
    public class VariantCreationTests : TestFixtureBase
    {
        public VariantCreationTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        internal static class ExceptionDataKeys
        {
            // ReSharper disable once InconsistentNaming
            public const string configuration = nameof(configuration);

            // ReSharper disable once InconsistentNaming
            public const string value = nameof(value);

            public const string VariantType = nameof(VariantType);

            public static readonly string VariantTypeFullName
                = Join(".", typeof(Variant).FullName, nameof(Variant.VariantType));

            // ReSharper disable once InconsistentNaming
            public const string variantTypeArgumentName = "variantType";

            public const string ProtectedValue = nameof(ProtectedValue);
        }

        [Fact]
        public void Ctor_Default_Value_Null_Config_Throws()
        {
            const int defaultValue = default(int);
            var defaultValueType = defaultValue.GetType();

            Assert.Throws<ArgumentNullException>(() => new Variant<int>(null)
            ).VerifyException(x =>
            {
                var data = x.Data;

                var keys = new[] {configuration, value, VariantType};
                Assert.Equal(keys.Length, data.Count);
                Assert.All(keys, key => Assert.True(data.Contains(key)));

                Assert.Null(data[configuration]);

                Assert.Equal(defaultValue, Assert.IsAssignableFrom<int>(data[value]));

                Assert.NotNull(data[VariantType]);
                Assert.Equal(defaultValueType, Assert.IsAssignableFrom<Type>(data[VariantType]));
            });
        }

        private static void VerifyMisconfiguredVariant<T, TDefault>(
            Func<IVariantConfigurationCollection, Variant<T>> variantFactory
            , TDefault defaultValue, IVariantConfigurationCollection expectedConfiguration)
        {
            var defaultValueType = defaultValue.GetType();

            // ReSharper disable once ImplicitlyCapturedClosure
            Assert.Throws<ArgumentException>(() => variantFactory(expectedConfiguration)
            ).VerifyException(x =>
            {
                var data = x.Data;

                var keys = new[] {variantTypeArgumentName, configuration, ProtectedValue, VariantTypeFullName};
                Assert.Equal(keys.Length, data.Count);
                Assert.All(keys, key => Assert.True(data.Contains(key)));

                Assert.NotNull(data[configuration]);
                Assert.Same(expectedConfiguration, Assert.IsAssignableFrom<VariantConfigurationCollection>(data[configuration]));

                Assert.Equal(defaultValue, Assert.IsAssignableFrom<TDefault>(data[ProtectedValue]));

                Assert.Equal(defaultValueType, Assert.IsAssignableFrom<Type>(data[variantTypeArgumentName]));
                Assert.Equal(defaultValueType, Assert.IsAssignableFrom<Type>(data[VariantTypeFullName]));
            });
        }

        [Fact]
        public void Ctor_Default_Value_Empty_Config_Throws() => VerifyMisconfiguredVariant(
            x => new Variant<int>(x), default(int), VariantConfigurationCollection.Create()
        );

        [Fact]
        public void Ctor_Default_Value_Misconfigured_Config_Throws() => VerifyMisconfiguredVariant(
            x => new Variant<int>(x), default(int)
            , VariantConfigurationCollection.Create(VariantConfiguration.Configure<long>())
        );

        private static void VerifyInvalidVariantCombination<T, TDefault>(
            Func<Type, TDefault, IVariantConfigurationCollection, Variant<T>> variantFactory
            , TDefault defaultValue, IVariantConfigurationCollection expectedConfiguration)
        {
            var defaultValueType = typeof(TDefault);
            var derivedClassType = typeof(DerivedClass);

            // ReSharper disable once ImplicitlyCapturedClosure
            Assert.Throws<ArgumentException>(() => variantFactory(derivedClassType, defaultValue, expectedConfiguration)
            ).VerifyException(x =>
            {
                var data = x.Data;

                var keys = new[] {variantTypeArgumentName, configuration, ProtectedValue, VariantTypeFullName};
                Assert.Equal(keys.Length, data.Count);
                Assert.All(keys, key => Assert.True(data.Contains(key)));

                Assert.NotNull(data[configuration]);
                Assert.Same(expectedConfiguration, Assert.IsAssignableFrom<VariantConfigurationCollection>(data[configuration]));

                Assert.Equal(derivedClassType, Assert.IsAssignableFrom<Type>(data[variantTypeArgumentName]));

                var protectedValueData = data[ProtectedValue];
                var variantTypeFullNameData = data[VariantTypeFullName];

                if (defaultValue == null)
                {
                    Assert.All(new[] {protectedValueData, variantTypeFullNameData}, Assert.Null);
                    return;
                }

                Assert.IsAssignableFrom<TDefault>(protectedValueData);
                Assert.Equal(defaultValueType, Assert.IsAssignableFrom<Type>(variantTypeFullNameData));
            });
        }

        /// <summary>
        /// In most cases, you want to specify the variant type equal to the default value
        /// actual type. However, Variant should also accept a value whose corresponding
        /// variant type is assignable from the default value actual type. When this is
        /// not the case, it should throw.
        /// </summary>
        [Fact]
        public void Ctor_Incompatible_Variant_Types_Null_Value_Config_Value_Throws() => VerifyInvalidVariantCombination<
            DerivedClass, BaseClass>((type, x, cfg) => new Variant<DerivedClass>(type, x, cfg)
            , null, VariantConfigurationCollection.Create(VariantConfiguration.Configure<BaseClass>())
        );

        /// <summary>
        /// In most cases, you want to specify the variant type equal to the default value
        /// actual type. However, Variant should also accept a value whose corresponding
        /// variant type is assignable from the default value actual type. When this is
        /// not the case, it should throw.
        /// </summary>
        [Fact]
        public void Create_Incompatible_Variant_Types_Null_Value_Config_Value_Throws() => VerifyInvalidVariantCombination<
            DerivedClass, BaseClass>((type, x, cfg) => (Variant<DerivedClass>) Variant.Create(type, x, cfg)
            , null, VariantConfigurationCollection.Create(VariantConfiguration.Configure<BaseClass>())
        );

        /// <summary>
        /// In most cases, you want to specify the variant type equal to the default value
        /// actual type. However, Variant should also accept a value whose corresponding
        /// variant type is assignable from the default value actual type. When this is
        /// not the case, it should throw.
        /// </summary>
        [Fact]
        public void Ctor_Incompatible_Variant_Types_Instance_Throws() => VerifyInvalidVariantCombination<
            DerivedClass, BaseClass>((type, x, cfg) => new Variant<DerivedClass>(type, x, cfg)
            , new BaseClass(), VariantConfigurationCollection.Create(VariantConfiguration.Configure<BaseClass>())
        );

        /// <summary>
        /// In most cases, you want to specify the variant type equal to the default value
        /// actual type. However, Variant should also accept a value whose corresponding
        /// variant type is assignable from the default value actual type. When this is
        /// not the case, it should throw.
        /// </summary>
        [Fact]
        public void Create_Incompatible_Variant_Types_Instance_Throws() => VerifyInvalidVariantCombination<
            DerivedClass, BaseClass>((type, x, cfg) => (Variant<DerivedClass>) Variant.Create(type, x, cfg)
            , new BaseClass(), VariantConfigurationCollection.Create(VariantConfiguration.Configure<BaseClass>())
        );

        private static void VerifyDefaultConstructor<T>(VariantEquatableCallback equatableCallback
            , VariantComparableCallback comparableCallback, Action<T> verify)
        {
            var instance = Variant.Create<T>(VariantConfigurationCollection.Create(
                    VariantConfiguration.Configure<T>(equatableCallback, comparableCallback)
                )
            );

            Assert.NotNull(instance);
            verify(instance.Value);
        }

        [Fact]
        public void Create_Default_Integer_Variant_Instance_Correct()
            => VerifyDefaultConstructor(
                (x, y) => (int) x == (int) y
                , (x, y) => ((int) x).CompareTo((int) y)
                , (int x) => Assert.Equal(default(int), x)
            );

        [Fact]
        public void Create_Default_Class_Variant_Instance_Correct()
            => VerifyDefaultConstructor(
                (x, y) => ((BaseClass) x).Equals((BaseClass) y)
                , (x, y) => ((BaseClass) x).CompareTo((BaseClass) y)
                , (BaseClass x) =>
                {
                    Assert.NotNull(x);
                    Assert.NotEqual(Guid.Empty, x.Id);
                });
    }
}
