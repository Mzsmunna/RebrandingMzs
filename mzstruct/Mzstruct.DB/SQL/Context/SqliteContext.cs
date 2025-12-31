using Microsoft.Data.Sqlite;

namespace Mzstruct.DB.SQL.Context
{
    public class SqliteContext
    {
        private readonly string _connection;

        public SqliteContext(string connection) 
        {
            _connection = connection;
        }

        public SqliteConnection CreateDbContext()
        {
            return new SqliteConnection(_connection);
        }
    }
}
