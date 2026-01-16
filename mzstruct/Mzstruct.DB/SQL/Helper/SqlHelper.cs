using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mzstruct.Base.Enums;
using Mzstruct.DB.Contracts.IFactories;
using Mzstruct.DB.SQL.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.DB.SQL.Helper
{
    public static class SqlHelper
    {
        public static IServiceCollection AddDBConnFactory(IServiceCollection services, IConfiguration config, DBType dBType = DBType.SqlServer)
        {
            var conn = config.GetConnectionString("DefaultConnection");
                 //?? throw new ApplicationException("The connectiton string is null");
            if (conn is null) return services;
            
            if (dBType is DBType.SqlServer)
            {
                services.AddSingleton<IDbSqlConnFactory>(sp => new SqlServerFactory(conn));
            }
            else if (dBType is DBType.PostgreSql)
            {
                services.AddSingleton<IDbSqlConnFactory>(sp => new PostgreSqlFactory(conn));
            }
            else if (dBType is DBType.SQLite)
            {
                services.AddSingleton<IDbSqlConnFactory>(sp => new SqliteFactory(conn));
            }
            return services;
        }
    }
}
