using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.DB.Providers.PostgreSQL.Models
{
    public class NpgSqlParams
    {
        public required NpgsqlParameter StartDateParameter { get; set; }
        public required NpgsqlParameter EndDateParameter { get; set; }
        public required NpgsqlParameter PageNumberParameter { get; set; }
        public required NpgsqlParameter PageSizeParameter { get; set; }
        public required NpgsqlParameter SortFieldParameter { get; set; }
        public required NpgsqlParameter SortOrderParameter { get; set; }
        public required NpgsqlParameter SearchTextParameter { get; set; }
    }
}
