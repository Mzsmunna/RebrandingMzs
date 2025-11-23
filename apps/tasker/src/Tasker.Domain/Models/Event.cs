using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Domain.Models
{
    public class Event
    {
        public required string Id { get; set; }
        public required string By { get; set; }
        public DateTime At { get; set; } = DateTime.UtcNow; // DateTimeOffset.UtcNow;  
        public string? Name { get; set; }
        public string? Image { get; set; } 
        public string? Type { get; set; }
        public string? Details { get; set; } // mostly for human consumption
        public string? Context { get; set; } // json string, could be anything
    }
}
