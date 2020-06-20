using System;

namespace Ellumination.Collections
{
    using Xunit;
    using Xunit.Abstractions;

    public abstract partial class SubjectTestFixtureBase<T> : TestFixtureBase
        where T : class
    {
        protected ITestOutputHelper OutputHelper { get; }

        protected SubjectTestFixtureBase(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        protected T Subject { get; set; }

        protected static Type SubjectType { get; } = typeof(T);

        protected virtual T GetSubject(Func<T> create)
        {
            Assert.NotNull(create);
            var s = Subject = create();
            Assert.NotNull(s);
            return s;
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                if (Subject is IDisposable subject)
                {
                    subject.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}
