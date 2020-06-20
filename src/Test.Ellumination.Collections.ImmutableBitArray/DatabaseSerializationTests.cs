using System;
using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    using Xunit;

    public class DatabaseSerializationTests : TestFixtureBase
    {
        private DatabaseFixture Db { get; set; }

        public DatabaseSerializationTests()
        {
            Db = new DatabaseFixture();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !IsDisposed)
            {
                Db?.Dispose();
                Db = null;
            }

            base.Dispose(disposing);
        }

        public static IEnumerable<object[]> SerializedMaskData
        {
            get
            {
                yield return new object[] { (uint)0x12121212 };
                yield return new object[] { (uint)0x34343434 };
                yield return new object[] { (uint)0x12341234 };
                yield return new object[] { (uint)0x56565656 };
                yield return new object[] { (uint)0x78787878 };
                yield return new object[] { (uint)0x56785678 };
                yield return new object[] { (uint)0x12345678 };
                yield return new object[] { (uint)0x15263748 };
            }
        }

        /// <summary>
        /// Verifies that bit array serialization is correct.
        /// </summary>
        /// <param name="mask"></param>
        [Theory(Skip = "Disabled for the time being, considering alternate physical and/or logical DB strategies...")
            , MemberData(nameof(SerializedMaskData), DisableDiscoveryEnumeration = true)
            ]
        public void Verify_Bit_Array_serializes_correctly(uint mask)
        {
            var subject = CreateBitArray(mask);

            // Make sure that we report them in the correct, expected LSB order.
            var record = Db.FixtureTable.InsertBytes(subject.ToBytes(false));

            CreateBitArray(record.Bytes,
                s =>
                {
                    Assert.Equal(record.Bytes.Length * 8, s.Length);
                    Assert.True(s.Equals(subject));
                });
        }

        private static ImmutableBitArrayFixture VerifyBitArray(ImmutableBitArrayFixture subject,
            Action<ImmutableBitArrayFixture> verify = null)
        {
            verify = verify ?? (x => { });
            Assert.NotNull(subject);
            verify(subject);
            return subject;
        }

        // ReSharper disable once UnusedMethodReturnValue.Local
        private static ImmutableBitArrayFixture CreateBitArray(byte[] bytes,
            Action<ImmutableBitArrayFixture> verify)
        {
            return VerifyBitArray(new ImmutableBitArrayFixture(bytes), verify);
        }

        /// <summary>
        /// Returns a created fully vetted array based on <paramref name="mask"/>.
        /// </summary>
        /// <param name="mask"></param>
        /// <returns></returns>
        private static ImmutableBitArrayFixture CreateBitArray(uint mask)
        {
            var result = VerifyBitArray(new ImmutableBitArrayFixture(new[] {mask}),
                s =>
                {
                    Assert.Equal(sizeof(uint) * 8, s.Length);
                    // Reverse these bytes because they are in reverse order.
                    var maskBytes = BitConverter.GetBytes(mask).ToArray();
                    var subjectBytes = s.ToBytes(false).ToArray();
                    Assert.Equal(maskBytes, subjectBytes);
                });

            return result;
        }
    }
}
