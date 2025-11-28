using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Resources.DAL.MongoDB.Models
{
    public class MongoOperation
    {
        public required string Id { get; set; }
        public bool IsCompleted { get; set; }
    }
}
