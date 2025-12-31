using System.Data;
using System.Data.Common;
using System.Dynamic;
using Microsoft.Data.SqlClient;
using Mzstruct.Base.Consts;

namespace Mzstruct.DB.Providers.SqlServer.Helpers
{
    internal class SqlClientHelper
    {
        internal static T ExecuteReader<T>(Func<DbDataReader, T> dbDataRederDelegate, string procedureName, params object[] parameters)
        {
            using var conn = new SqlConnection(AppConst.SqlConnectionString);
            using var command = new SqlCommand(procedureName, conn);
            conn.Open();
            command.Parameters.AddRange(parameters);
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                using var reader = command.ExecuteReader();
                T data = dbDataRederDelegate(reader);
                return data;
            }
            finally
            {
                conn.Close();
            }
        }
        internal static T ExecuteReader<T>(Func<DbDataReader, T> dbDataRederDelegate, string procedureName)
        {
            using var conn = new SqlConnection(AppConst.SqlConnectionString);
            using var command = new SqlCommand(procedureName, conn);
            conn.Open();
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                using var reader = command.ExecuteReader();
                T data = dbDataRederDelegate(reader);
                return data;
            }
            finally
            {
                conn.Close();
            }
        }

        internal async static Task<List<IDictionary<string, Object>>> ExecuteDynamicReader(string commandText, List<SqlParameter>? sqlParameters = null)
        {
            List<IDictionary<string, Object>> results = new List<IDictionary<string, Object>>();
            using var conn = new SqlConnection(AppConst.SqlConnectionString);

            try
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = commandText;
                
                if (sqlParameters != null && sqlParameters.Count > 0)
                    command.Parameters.AddRange(sqlParameters.ToArray());
                
                var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    var item = new ExpandoObject() as IDictionary<string, Object>;

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var fieldName = reader.GetName(i);
                        var fieldValue = reader[i];

                        if (fieldValue is DBNull || fieldValue is null)
                            fieldValue = "";

                        item.Add(fieldName, fieldValue);
                    }

                    results.Add(item);
                }
            }
            finally
            {
                conn.Close();
            }

            return results;
        }
    }
}
