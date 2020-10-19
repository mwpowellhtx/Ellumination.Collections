using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Generic
{
    using Xunit;
    using Xwellbehaved;
    using static Newtonsoft.Json.JsonConvert;
    using static String;

    /// <summary>
    /// Verifies that Serialization works correctly given simple <see cref="int"/>
    /// based scenarios. More complex generic subjects really depends on the type
    /// of those generic subjects.
    /// </summary>
    public class SerializationTests : TestFixtureBase
    {
        private IList<int> Collection { get; set; }

        private BidirectionalList<int> BidiCollection { get; set; }

        [Background]
        public void Background(IEnumerable<int> range)
        {
            "Initialize the range".x(() => range = Range(0, 1, 2, 3).ToList());

            "Initialize the collection".x(() => this.Collection = range.AssertIsType<List<int>>());

            "Verify the collection".x(() => this.Collection.AssertEqual(range));

            "Initialize the bidirectional scaffold".x(() => this.BidiCollection = this.Collection.ToBidirectionalList());

            "Verify the bidirectional asset".x(() => this.BidiCollection.AssertEqual(range));

            "Verify the bidirectional collection".x(() => this.BidiCollection.Collection.AssertEqual(range));

            "Verify the bidirectional collection instance".x(() => this.BidiCollection.Collection.AssertSame(this.Collection));
        }

        [Scenario]
        public void Serialization_Is_Correct(string expected, string actual)
        {
            const string comma = ",";
            const string squareBrackets = "[]";

            "Contain the expected JSON".x(() => expected = Join(Join(comma, this.Collection.Select(x => $"{x}")), squareBrackets.ToArray()));

            "Serialize the bidirectional assets".x(() => actual = SerializeObject(this.BidiCollection).AssertNotNull().AssertNotEmpty());

            "Verify that the bidirectional assets serialized correctly".x(() => actual.AssertEqual(expected));
        }

        [Scenario]
        public void Deserialization_Is_Correct(string json, BidirectionalList<int> expected, BidirectionalList<int> actual)
        {
            "Isolate the expected bidirectional assets".x(() => expected = this.BidiCollection);

            "That we can serialize the collection".x(() => json = SerializeObject(this.BidiCollection).AssertNotNull().AssertNotEmpty());

            "That we can deserialize the collection".x(() => actual = DeserializeObject<BidirectionalList<int>>(json).AssertNotNull());

            "And the collection deserialized correctly".x(() => actual.AssertNotSame(expected).AssertEqual(expected));
        }
    }
}
