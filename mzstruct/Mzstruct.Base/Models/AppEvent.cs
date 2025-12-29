using Mzstruct.Base.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Models
{
    public class AppEvent(string resource, string objectId, string type) //<T>(string objectId) where T : class
    {
        public string Id { get; set; } =  Guid.NewGuid().ToString(); //ObjectId.GenerateNewId().ToString();
        public string Res { get; set; } =  resource ?? string.Empty; // resource / entitty / table name;
        public string Ref { get; set; } = objectId ?? string.Empty;
        public string Type { get; set; } = type ?? EventType.Request.ToString();
        public DateTime At { get; set; } = DateTime.UtcNow; // DateTimeOffset.UtcNow;
        public string By { get; set; } = string.Empty; // user id, system, service etc.
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Details { get; set; } // mostly for human consumption
        public string? Context { get; set; } // json string, could be anything
    }
}
