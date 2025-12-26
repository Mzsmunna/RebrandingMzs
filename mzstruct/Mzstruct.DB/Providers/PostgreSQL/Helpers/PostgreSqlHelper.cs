using Mzstruct.Base.Dtos;
using Mzstruct.DB.Providers.PostgreSQL.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.DB.Providers.PostgreSQL.Helpers
{
    public static class PostgreSqlHelper
    {
        public static NpgSqlParams GenerateNpgsqlParams(ParamDto param)
        {
            var npgsqlParams = new NpgSqlParams
            {
                StartDateParameter = new NpgsqlParameter("StartDate", param.StartDate),
                EndDateParameter = new NpgsqlParameter("EndDate", param.EndDate),
                PageNumberParameter = new NpgsqlParameter("PageNumber", param.PageNumber.HasValue ? param.PageNumber : 1),
                PageSizeParameter = new NpgsqlParameter("PageSize", param.PageSize.HasValue ? param.PageSize : 5),
                SortFieldParameter = new NpgsqlParameter("SortField", string.IsNullOrEmpty(param.SortField) ? DBNull.Value : param.SortField),
                SortOrderParameter = new NpgsqlParameter("SortOrder", string.IsNullOrEmpty(param.SortOrder) ? DBNull.Value : param.SortOrder),
                SearchTextParameter = new NpgsqlParameter("SearchText", string.IsNullOrEmpty(param.SearchText) ? DBNull.Value : param.SearchText)
            };

            return npgsqlParams;
        }
    }
}
