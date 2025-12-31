using Microsoft.Data.Sqlite;
using Mzstruct.DB.Contracts.IFactories;
using System.Data;

namespace Mzstruct.DB.SQL.Factory
{
    public class SqliteFactory : IDbSqlConnFactory
    {
        private readonly string _conn;

        public SqliteFactory(string connection) 
        {
            _conn = connection;
        }

        public IDbConnection Connect()
        {
            return new SqliteConnection(_conn);
        }

        public async Task<IDbConnection> ConnectAsync(CancellationToken token = default)
        {
            var conn = new SqliteConnection(_conn);
            await conn.OpenAsync(token);
            return conn;
        }
    }
}
