using Microsoft.Data.SqlClient;
using Mzstruct.Base.Contracts.IFactories;
using Npgsql;
using System.Data;

namespace Mzstruct.DB.SQL.Factory
{
    public class SqlServerFactory : IDbSqlFactory
    {
        private readonly string _connection;

        public SqlServerFactory(string connection) 
        {
            _connection = connection;
        }

        public IDbConnection Connect()
        {
            return new SqlConnection(_connection);
        }

        public async Task<IDbConnection> ConnectAsync()
        {
            var conn = new SqlConnection(_connection);
            await conn.OpenAsync();
            return conn;
        }
    }
}
