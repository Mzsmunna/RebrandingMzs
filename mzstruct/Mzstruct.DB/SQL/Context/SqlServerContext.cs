using Microsoft.Data.SqlClient;

namespace Mzstruct.DB.SQL.Context
{
    public class SqlServerContext
    {
        private readonly string _connection;

        public SqlServerContext(string connection) 
        {
            _connection = connection;
        }

        public SqlConnection Connect()
        {
            return new SqlConnection(_connection);
        }
    }
}
