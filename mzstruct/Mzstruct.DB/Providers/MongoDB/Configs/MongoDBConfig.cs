using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.DB.Providers.MongoDB.Configs
{
    public class MongoDBConfig
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
    }
}
