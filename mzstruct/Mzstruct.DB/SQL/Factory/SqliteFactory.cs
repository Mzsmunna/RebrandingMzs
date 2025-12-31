using Microsoft.Data.Sqlite;
using Mzstruct.Base.Contracts.IFactories;
using Npgsql;
using System.Data;

namespace Mzstruct.DB.SQL.Factory
{
    public class SqliteFactory : IDbSqlFactory
    {
        private readonly string _connection;

        public SqliteFactory(string connection) 
        {
            _connection = connection;
        }

        public IDbConnection Connect()
        {
            return new SqliteConnection(_connection);
        }

        public async Task<IDbConnection> ConnectAsync()
        {
            var conn = new SqliteConnection(_connection);
            await conn.OpenAsync();
            return conn;
        }
    }
}
