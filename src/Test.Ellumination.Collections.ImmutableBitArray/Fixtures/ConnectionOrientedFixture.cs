using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Ellumination.Collections
{
    using Xunit;
    using static Guid;
    using static ConnectionState;

    // TODO: TBD: so here's a question, do we see about picking up a racked key for SQL Server?
    // TODO: TBD: or do we think about running with something more open source?
    // TODO: TBD: Interbase? Firebase? mysql?
    // https://stackoverflow.com/questions/219684/best-way-to-connect-to-interbase-7-1-using-net-c-sharp
    // https://stackoverflow.com/questions/6588779/interbase-net-entity-framework-provider
    // https://www.ibprovider.com/eng/documentation/firebird_adonet.html
    // http://www.firebirdsql.org/en/net-provider/
    // https://www.mono-project.com/docs/database-access/providers/firebird/
    // https://www.quora.com/Whats-the-best-free-alternative-to-Microsoft-SQL-Server-or-Oracle-Database
    // ? https://mariadb.org/
    // ? https://www.sqlite.org/index.html
    // ? http://www.firebirdsql.org/en/start/
    // ? https://alternativeto.net/software/microsoft-sql-server/
    // ? https://www.osalt.com/sql-server
    public abstract class ConnectionOrientedFixture : IDisposable
    {
        protected static string GetConnectionString(string databaseName = "master")
        {
            var args = new[]
            {
                new KeyValuePair<string, string>("Data Source", "localhost"),
                new KeyValuePair<string, string>("Initial Catalog", databaseName),
                new KeyValuePair<string, string>("Integrated Security", "SSPI")
            };

            const string equals = "=";
            const string semiColon = ";";

            var connectionString = string.Join(semiColon, args.Select(
                arg => string.Join(equals, arg.Key, arg.Value)));

            return connectionString;
        }

        private static void VerifySqlConnectionOpen(SqlConnection connection)
        {
            connection.Open();
            Assert.Equal(Open, connection.State);
            Assert.NotEqual(Empty, connection.ClientConnectionId);
        }

        /// <summary>
        /// Returns a new <see cref="SqlConnection"/> given <paramref name="connectionString"/>.
        /// Passes that connection through the <paramref name="verify"/> action.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="verify">Passes the connection through the action. Default is
        /// <see cref="VerifySqlConnectionOpen"/>.</param>
        /// <returns></returns>
        private static SqlConnection GetSqlConnection(string connectionString, Action<SqlConnection> verify = null)
        {
            var conn = new SqlConnection(connectionString);

            Assert.Equal(Closed, conn.State);
            Assert.Equal(Empty, conn.ClientConnectionId);

            verify = verify ?? VerifySqlConnectionOpen;

            verify(conn);

            return conn;
        }

        protected void Run(string connectionString, Action<SqlConnection> action = null)
        {
            action = action ?? (conn => { });

            using (var conn = GetSqlConnection(connectionString))
            {
                try
                {
                    action(conn);
                }
                finally
                {
                    // http://stackoverflow.com/questions/1145892/how-to-force-a-sqlconnection-to-physically-close-while-using-connection-pooling
                    SqlConnection.ClearPool(conn);
                }
            }
        }

        protected static void RunNonQuery(SqlConnection connection, string sql, params SqlParameter[] values)
        {
            using (var cmd = new SqlCommand(sql, connection))
            {
                cmd.Parameters.AddRange(values);
                cmd.ExecuteNonQuery();
            }
        }

        protected static IEnumerable<T> RunQuery<T>(SqlConnection connection, string sql, Func<SqlDataReader, T> getter, params SqlParameter[] values)
        {
            using (var cmd = new SqlCommand(sql, connection))
            {
                cmd.Parameters.AddRange(values);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return getter(reader);
                    }
                }
            }
        }

        public bool IsDisposed { get; private set; }

        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            Dispose(true);
            IsDisposed = true;
        }
    }
}
