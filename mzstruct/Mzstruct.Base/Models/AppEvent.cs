using Mzstruct.Base.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Models
{
    public class AppEvent(string entity, string objectId, EventType type = EventType.Request) //<T>(string objectId) where T : class
    {
        public string Id { get; set; } =  Guid.NewGuid().ToString(); //ObjectId.GenerateNewId().ToString();
        public string Ref { get; set; } = (!string.IsNullOrEmpty(entity) && !string.IsNullOrEmpty(objectId)) ? 
            $"{entity}:{objectId}" : string.Empty; //$"{typeof(T).Name}:{objectId}"; // entitty / table name + ':' + object id
        public EventType Type { get; set; } = type;
        public DateTime At { get; set; } = DateTime.UtcNow; // DateTimeOffset.UtcNow;
        public string By { get; set; } = string.Empty; // user id, system, service etc.
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Details { get; set; } // mostly for human consumption
        public string? Context { get; set; } // json string, could be anything
    }
}
