using Mzstruct.Base.Contracts.IFactories;
using Npgsql;
using System.Data;

namespace Mzstruct.DB.SQL.Factory
{
    public class PostgreSqlFactory : IDbSqlFactory
    {
        private readonly string _connection;

        public PostgreSqlFactory(string connection) 
        {
            _connection = connection;
        }

        public IDbConnection Connect()
        {
            return new NpgsqlConnection(_connection);
        }

        public async Task<IDbConnection> ConnectAsync()
        {
            var conn = new NpgsqlConnection(_connection);
            await conn.OpenAsync();
            return conn;
        }
    }
}
