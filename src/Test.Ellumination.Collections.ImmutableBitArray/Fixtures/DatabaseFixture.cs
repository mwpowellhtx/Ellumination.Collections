using System;

namespace Ellumination.Collections
{
    public class DatabaseFixture : ConnectionOrientedFixture
    {
        public FixtureTableFixture FixtureTable { get; }

        private string DatabaseName { get; set; }

        private Guid DatabaseId
        {
            // ReSharper disable once UnusedMember.Local
            get => Guid.Parse(DatabaseName);
            set => DatabaseName = $"{value:D}";
        }

        internal DatabaseFixture(Guid? did = null)
        {
            DatabaseId = did ?? Guid.NewGuid();

            VerifyDatabaseCreated(DatabaseName);

            FixtureTable = new FixtureTableFixture(DatabaseName);
        }

        private void VerifyDatabaseCreated(string databaseName)
        {
            Run(GetConnectionString(), conn =>
            {
                RunNonQuery(conn, string.Format(
                    @"IF DB_ID('{0}') IS NULL
CREATE DATABASE [{0}]", databaseName));
            });
        }

        private void VerifyDatabaseDropped(string databaseName)
        {
            Run(GetConnectionString(), conn =>
            {
                RunNonQuery(conn, string.Format(
                    @"IF DB_ID('{0}') IS NOT NULL
DROP DATABASE [{0}]", databaseName));
            });
        }

        protected override void Dispose(bool disposing)
        {
            VerifyDatabaseDropped(DatabaseName);

            base.Dispose(disposing);
        }
    }
}
