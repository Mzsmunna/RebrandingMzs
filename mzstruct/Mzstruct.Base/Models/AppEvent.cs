using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Models
{
    public class AppEvent
    {
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString(); //Guid.NewGuid().ToString();
        public string By { get; set; } = string.Empty; // user id, system, etc.
        public DateTime At { get; set; } = DateTime.UtcNow; // DateTimeOffset.UtcNow;  
        public string? Name { get; set; }
        public string? Image { get; set; } 
        public string? Type { get; set; }
        public string? Details { get; set; } // mostly for human consumption
        public string? Context { get; set; } // json string, could be anything
    }
}
