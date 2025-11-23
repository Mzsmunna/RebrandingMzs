using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Persistence.DAL.MongoDB.Configs
{
    public class MongoOperation
    {
        public required string Id { get; set; }
        public bool IsCompleted { get; set; }
    }
}
