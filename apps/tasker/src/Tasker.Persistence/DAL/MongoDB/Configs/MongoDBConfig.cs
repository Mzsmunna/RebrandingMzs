using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Persistence.DAL.MongoDB.Configs
{
    public class MongoDBConfig
    {
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }
    }
}
