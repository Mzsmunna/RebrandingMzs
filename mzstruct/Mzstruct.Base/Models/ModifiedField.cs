using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace Mzstruct.Base.Models
{
    public class ModifiedField
    {
        //[BsonIgnore]
        public required PropertyInfo PropertyInfo { get; set; }
        public required string PropertyType { get; set; }
        public required string EntityName { get; set; }
        public required string PropertyName { get; set; }
        public required string CustomPropertyName { get; set; }
        public required string CurrentValue { get; set; }
        public required string PreviousValue { get; set; }
        public string? CustomCurrentValue { get; set; }
        public string? CustomPreviousValue { get; set; }
    }
}
