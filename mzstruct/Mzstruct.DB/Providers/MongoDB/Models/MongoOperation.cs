using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.DB.Providers.MongoDB.Models
{
    public class MongoOperation
    {
        public required string Id { get; set; }
        public bool IsCompleted { get; set; }
    }
}
