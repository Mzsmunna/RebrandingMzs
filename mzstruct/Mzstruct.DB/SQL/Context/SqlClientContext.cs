using Microsoft.Data.SqlClient;
using Mzstruct.Base.Consts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mzstruct.DB.SQL.Context
{
    public static class SqlClientContext<T> where T : class
    {
        public static void CreateTable(string? tableName = null)
        {          
            try
            {
                tableName = tableName ?? $"{typeof(T).Name}";

                Console.WriteLine("Creating Table : " + tableName);
                using (SqlConnection conn = new SqlConnection(AppConst.DatabaseContext))
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        string subQuery = string.Empty;

                        PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        foreach (PropertyInfo prop in Props)
                        {
                            if (!prop.Name.ToLower().Equals("id"))
                                subQuery += prop.Name + "     nvarchar(max) NULL" + ",\n\t\t";
                        }

                        var field = Props.Where(x => x.Name.ToLower().Equals("id")).FirstOrDefault();

                        if (field != null)
                        {
                            subQuery = "Id     integer PRIMARY KEY NOT NULL,\n\t\t" + subQuery;
                        }

                        string exactQuery = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + $"{tableName}" + @"]') AND type in (N'U'))
                                            BEGIN
                                                CREATE TABLE " + $"{tableName}" +
                                                        @" ("
                                                            + subQuery +
                                                        @"); 
                                            END";

                        cmd.CommandText = exactQuery;
                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Table Creation Done if doesn't exist");
        }

        public static void QuickBulkInsert(List<T> entityList)
        {
            Console.WriteLine("Quick Bulk Called");
            try
            {
                var datatable = ToDataTable(entityList);
                using (SqlConnection conn = new SqlConnection(AppConst.DatabaseContext))
                {
                    conn.Open();
                    var transaction = conn.BeginTransaction();
                    using (var bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, transaction))
                    {
                        bulkCopy.DestinationTableName = $"{typeof(T).Name}";
                        bulkCopy.WriteToServer(datatable);
                    }
                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                throw;
            }
            Console.WriteLine("Quick Bulk Ended");
        }

        public static void QuickBulkInsert(List<T> entityList, string? tableName)
        {
            Console.WriteLine("Quick Bulk Called");
            try
            {
                var entryTableName = tableName ?? typeof(T).Name;

                using (SqlConnection conn = new SqlConnection(AppConst.DatabaseContext))
                {
                    conn.Open();
                    RowChunkAndSave(entityList, entryTableName, conn);
                }
            }
            catch (Exception)
            {
                throw;
            }
            Console.WriteLine("Quick Bulk Ended");
        }

        public static void QuickBulkUpdate(List<T> entityList, string tableName = null)
        {
            Console.WriteLine("Quick Bulk Update Called");

            try
            {
                var datatable = ToDataTable(entityList);
                var tempTableName = "TempBulkTable";
                string tempTableTxtCmd = GetTempTableCreateCmd(datatable, tempTableName);

                using (SqlConnection conn = new SqlConnection(AppConst.DatabaseContext))
                {
                    conn.Open();

                    var originalTableName = tableName ?? typeof(T).Name;

                    ExecuteCmd(tempTableTxtCmd, conn);

                    Console.WriteLine($"Created Temp Table");

                    RowChunkAndSave(entityList, tempTableName, conn);

                    Console.WriteLine("Bulk Insert Completed");

                    var updateFromTempTableCmd = GetOriginalTblToTempTableUpdateCmd(datatable, originalTableName, tempTableName);
                    ExecuteCmd(updateFromTempTableCmd, conn);

                    Console.WriteLine("Updating from Temp Table Completed");

                    var dropTempTableCmd = $"DROP TABLE {tempTableName}";
                    ExecuteCmd(dropTempTableCmd, conn);

                    Console.WriteLine("Drop that temp table");

                }
            }
            catch (Exception)
            {
                throw;
            }

            Console.WriteLine("Quick Bulk Update Ended");

            string GetTempTableCreateCmd(DataTable dataTable, string tempTable)
            {
                StringBuilder columnTxt = new StringBuilder();
                columnTxt.Append($"CREATE TABLE {tempTable}(");
                int columnCount = dataTable.Columns.Count;

                for (int i = 0; i < columnCount; i++)
                {
                    string dataType = dataTable.Columns[i].DataType == Type.GetType("System.String") ? "VARCHAR(100) " : dataTable.Columns[i].DataType.ToString();
                    string colum = $"{dataTable.Columns[i]} {dataType}";


                    columnTxt.Append($"{colum}");

                    if (i != columnCount - 1)
                        columnTxt.Append(", ");
                }

                columnTxt.Append(");");

                return columnTxt.ToString();
            }

            string GetOriginalTblToTempTableUpdateCmd(DataTable dataTable, string originalTable, string tempTable)
            {
                StringBuilder updateTblCmd = new StringBuilder();

                updateTblCmd.Append("UPDATE ORGI SET ");

                for (int i = 1; i < dataTable.Columns.Count; i++)
                {
                    updateTblCmd.Append($"ORGI.{dataTable.Columns[i]} = TEMP.{dataTable.Columns[i]}");

                    if (i != dataTable.Columns.Count - 1)
                        updateTblCmd.Append(", ");
                }

                updateTblCmd.Append($" FROM {tempTable} TEMP INNER JOIN {originalTable} ORGI ON ORGI.{dataTable.Columns[0]} = TEMP.{dataTable.Columns[0]}");

                return updateTblCmd.ToString();
            }
        }

        public static void TruncateTable(string? tableName = null)
        {
            tableName = tableName ?? $"{typeof(T).Name}";

            Console.WriteLine("Truncating Table : " + tableName);
            using (SqlConnection conn = new SqlConnection(AppConst.DatabaseContext))
            {
                using (SqlCommand command = new SqlCommand("", conn))
                {
                    try
                    {
                        conn.Open();

                        //delete all data from table on database
                        //command.CommandTimeout = 300;
                        command.CommandText = $"TRUNCATE TABLE {tableName}";
                        command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            Console.WriteLine("All data has been removed from Table : " + tableName);
        }

        public static void ExecuteStoreProcedure(string storeProcedureName)
        {
            using (SqlConnection conn = new SqlConnection(AppConst.DatabaseContext))
            {
                using (SqlCommand command = new SqlCommand(storeProcedureName, conn))
                {
                    try
                    {
                        conn.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        //command.CommandTimeout = 300;
                        //command.CommandText = $"exe ";
                        SqlDataReader rdr = command.ExecuteReader();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        public static void ExecuteStoreProcedureWithParams(string storeProcedureName)
        {
            //--- INCOMPLETE ------//

            using (SqlConnection conn = new SqlConnection(AppConst.DatabaseContext))
            {
                {
                    try
                    {
                        conn.Open();

                        // 1.  create a command object identifying the stored procedure
                        SqlCommand cmd = new SqlCommand("CustOrderHist", conn);

                        // 2. set the command object so it knows to execute a stored procedure
                        cmd.CommandType = CommandType.StoredProcedure;

                        // 3. add parameter to command, which will be passed to the stored procedure
                        string custId = "1";
                        cmd.Parameters.Add(new SqlParameter("@CustomerID", custId));

                        // execute the command
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            // iterate through results, printing each to console
                            while (rdr.Read())
                            {
                                //Console.WriteLine("Product: {0,-35} Total: {1,2}", rdr["ProductName"], rdr["Total"]);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        public static DataTable ToDataTable(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];

                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }

                dataTable.Rows.Add(values);
            }

            //put a breakpoint here and check datatable
            return dataTable;
        }

        private static void RowChunkAndSave(List<T> entityList, string entryTableName, SqlConnection conn)
        {
            int chunkSize = 10000;
            int totalCount = entityList.Count;

            int loop = totalCount / chunkSize;

            for (int i = 0; i <= loop; i++)
            {
                var datatables = ToDataTable(entityList.Skip(i * chunkSize).Take(chunkSize).ToList());
                var transaction = conn.BeginTransaction();

                using (var bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, transaction))
                {
                    bulkCopy.BulkCopyTimeout = 0;
                    bulkCopy.DestinationTableName = entryTableName;
                    bulkCopy.WriteToServer(datatables);
                }

                transaction.Commit();
            }
        }

        private static void ExecuteCmd(string cmdTxt, SqlConnection conn)
        {
            using (SqlCommand cmd = new SqlCommand(cmdTxt, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
