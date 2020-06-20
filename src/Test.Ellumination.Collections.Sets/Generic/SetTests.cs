namespace Ellumination.Collections.Generic
{
    using Xunit;
    using Xunit.Abstractions;

    public class SetTests : SetTestFixtureBase<int>
    {
        public SetTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public void New_Instance_Okay()
        {
            var instance = Instance.AssertEqual(default(int), x => x.Count);
            instance.Keys.AssertNotNull().AssertEmpty();
            instance.Values.AssertNotNull().AssertEmpty();
        }

        [Theory, InlineData(1), InlineData(2), InlineData(3)]
        public void Can_Add_One_Value(int value)
        {
            var instance = Instance.AssertTrue(x => x.Add(value));
            instance.AssertNotEmpty().Count.AssertEqual(1);
            instance.AssertTrue(x => x.Contains(value));
            instance.AssertTrue(x => x.ContainsKey(value));
            instance.Keys.AssertContains(x => x == value);
            instance.Values.AssertContains(x => x == value);
            instance[value].AssertEqual(value);
        }

        [Theory, InlineData(1, 2, 3), InlineData(4, 5, 6)]
        public void Can_Add_Several_Values(int a, int b, int c)
        {
            a.AssertNotEqual(b);
            b.AssertNotEqual(c);

            var instance = Instance.AssertTrue(x => x.Add(a));
            instance.AssertTrue(x => x.Add(b));
            instance.AssertTrue(x => x.Add(c));

            instance.AssertNotEmpty().Count.AssertEqual(3);
            instance.AssertTrue(x => x.Contains(a));
            instance.AssertTrue(x => x.ContainsKey(a));
            instance.AssertTrue(x => x.Contains(b));
            instance.AssertTrue(x => x.ContainsKey(b));
            instance.AssertTrue(x => x.Contains(c));
            instance.AssertTrue(x => x.ContainsKey(c));

            instance.Keys.AssertNotNull().AssertNotEmpty()
                .AssertTrue(x => x.Contains(a))
                .AssertTrue(x => x.Contains(b))
                .AssertTrue(x => x.Contains(c))
                ;

            instance.Values.AssertNotNull().AssertNotEmpty()
                .AssertTrue(x => x.Contains(a))
                .AssertTrue(x => x.Contains(b))
                .AssertTrue(x => x.Contains(c))
                ;

            instance[a].AssertEqual(a);
            instance[b].AssertEqual(b);
            instance[c].AssertEqual(c);
        }

        [Theory, InlineData(1), InlineData(2), InlineData(3)]
        public void Cannot_Add_Duplicate_Items(int value)
        {
            var instance = Instance.AssertTrue(x => x.Add(value));
            instance.AssertFalse(x => x.Add(value));
            instance.AssertEqual(1, x => x.Count);
        }
    }
}
