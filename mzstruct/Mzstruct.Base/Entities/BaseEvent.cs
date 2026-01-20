using Mzstruct.Base.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Entities
{
    public class BaseEvent //<T>(string objectId) where T : class
    {
        public string Id { get; set; } =  Guid.CreateVersion7().ToString(); //ObjectId.GenerateNewId().ToString();
        public string Topic { get; set; } = string.Empty; // resource / entity / collection / table / Topic name;
        public string RefId { get; set; } = string.Empty; // record id;
        public EventType Type { get; set; } = EventType.Request;
        public DateTime At { get; set; } = DateTime.UtcNow; // DateTimeOffset.UtcNow;
        public string By { get; set; } = string.Empty; // user id, system, service, web hooks etc.
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Details { get; set; } // mostly for human consumption
        public string? Context { get; set; } // json string, could be anything
    }
}
