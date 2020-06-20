using System;

namespace Ellumination.Collections.Variants
{
    using Xunit;
    using Xunit.Abstractions;
    using static String;
    using static VariantBaseClassValueTestFixtureBase.ExceptionDataKeys;

    public abstract class VariantBaseClassValueTestFixtureBase : TestFixtureBase
    {
        protected abstract Variant WeaklyTypedSubject { get; }

        protected VariantBaseClassValueTestFixtureBase(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        internal static class ExceptionDataKeys
        {
            // ReSharper disable once InconsistentNaming
            private const string dot = ".";

            // ReSharper disable once InconsistentNaming
            public const string value = nameof(value);

            // ReSharper disable once InconsistentNaming
            public const string valueType = nameof(valueType);

            public static readonly string VariantTypeFullName = Join(dot, typeof(Variant).FullName, nameof(Variant.VariantType));

            public static readonly string ProtectedValueFullName = Join(dot, typeof(Variant).FullName, nameof(Variant.ProtectedValue));
        }
    }

    public abstract class VariantBaseClassValueTestFixtureBase<T> : VariantBaseClassValueTestFixtureBase
    {
        protected T DefaultValue { get; }

        /// <summary>
        /// Base class view of the <see cref="StronglyTypedSubject"/>.
        /// </summary>
        protected override Variant WeaklyTypedSubject => StronglyTypedSubject;

        private Variant<T> StronglyTypedSubject { get; }

        private static Variant<T> CreateSubject(T defaultValue) => Variant.Create(defaultValue
            , VariantConfigurationCollection.Create(VariantConfiguration.Configure<T>())
        );

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="outputHelper"></param>
        /// <param name="defaultValue">Null would not work for reference types.</param>
        protected VariantBaseClassValueTestFixtureBase(ITestOutputHelper outputHelper, T defaultValue = default(T))
            : base(outputHelper)
            => VerifyCtor(StronglyTypedSubject = CreateSubject(defaultValue), DefaultValue = defaultValue);

        private void VerifyCtor(Variant<T> subject, T defaultValue)
        {
            Assert.Equal(defaultValue, DefaultValue);
            Assert.Same(subject, WeaklyTypedSubject);
            Assert.Equal(typeof(T), WeaklyTypedSubject.VariantType);
            Assert.Equal(defaultValue, WeaklyTypedSubject.Value);
        }

#pragma warning disable xUnit1003 // Theory methods must have test data
        [Theory]
        public virtual void Can_Replace_Like_Types(T value)
        {
            Assert.NotEqual(WeaklyTypedSubject.Value, value);
            WeaklyTypedSubject.Value = value;
            Assert.Equal(value, WeaklyTypedSubject.Value);
        }
#pragma warning restore xUnit1003 // Theory methods must have test data

#pragma warning disable xUnit1003 // Theory methods must have test data
        /// <summary>
        /// This is where the comparisons between C Plus Plus Variants and a CSharp modeling
        /// of the same breaks down a little bit. We are unable to replace values in-place,
        /// especially considering the strongly typed <see cref="IVariant{T}"/> variation.
        /// Rather, if replacement is desired, we suggest tracking the plain
        /// <see cref="IVariant"/> interface or concrete <see cref="Variant"/> class,
        /// and replacing Variant instances themselves.
        /// </summary>
        /// <param name="replacementValue"></param>
        [Theory]
        public virtual void Cannot_Replace_Disparate_Types(object replacementValue)
        {
            Assert.NotNull(replacementValue);
            Assert.IsNotType(WeaklyTypedSubject.VariantType, replacementValue);
            // ReSharper disable once IdentifierTypo
            Assert.Throws<InvalidOperationException>(() => WeaklyTypedSubject.Value = replacementValue).VerifyException(ioex =>
            {
                var data = ioex.Data;

                var keys = new[] {value, valueType, VariantTypeFullName, ProtectedValueFullName};
                Assert.Equal(keys.Length, data.Count);

                Assert.Equal(DefaultValue, Assert.IsAssignableFrom<T>(data[ProtectedValueFullName]));
                Assert.Equal(WeaklyTypedSubject.VariantType, Assert.IsAssignableFrom<Type>(data[VariantTypeFullName]));

                Assert.Equal(replacementValue, data[value]);
                Assert.Equal(replacementValue.GetType(), data[valueType]);
            });
        }
#pragma warning restore xUnit1003 // Theory methods must have test data

    }
}
