using Microsoft.Data.SqlClient;
using Mzstruct.Base.Contracts.IFactories;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Data;

namespace Mzstruct.DB.SQL.Factory
{
    public class SqlServerFactory : IDbSqlConnFactory
    {
        private readonly string _conn;

        public SqlServerFactory(string connection) 
        {
            _conn = connection;
        }

        public IDbConnection Connect()
        {
            return new SqlConnection(_conn);
        }

        public async Task<IDbConnection> ConnectAsync(CancellationToken token = default)
        {
            var conn = new SqlConnection(_conn);
            await conn.OpenAsync(token);
            return conn;
        }
    }
}
