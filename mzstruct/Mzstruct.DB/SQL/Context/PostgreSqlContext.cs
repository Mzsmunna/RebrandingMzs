using Npgsql;

namespace Mzstruct.DB.SQL.Context
{
    public class PostgreSqlContext
    {
        private readonly string _connection;

        public PostgreSqlContext(string connection) 
        {
            _connection = connection;
        }

        public NpgsqlConnection Connect()
        {
            return new NpgsqlConnection(_connection);
            
        }
    }
}
