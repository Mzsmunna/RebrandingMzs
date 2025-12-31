using Mzstruct.DB.Contracts.IFactories;
using Npgsql;
using System.Data;

namespace Mzstruct.DB.SQL.Factory
{
    public class PostgreSqlFactory : IDbSqlConnFactory
    {
        private readonly string _conn;

        public PostgreSqlFactory(string connection) 
        {
            _conn = connection;
        }

        public IDbConnection Connect()
        {
            var conn = new NpgsqlConnection(_conn);
            conn.Open();
            return conn;
        }

        public async Task<IDbConnection> ConnectAsync(CancellationToken token = default)
        {
            var conn = new NpgsqlConnection(_conn);
            await conn.OpenAsync(token);
            return conn;
        }
    }
}
