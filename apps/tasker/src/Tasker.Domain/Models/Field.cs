using System;
using System.Collections.Generic;
using System.Text;

namespace TaskerDomain.Models
{
    public class Field
    {
        public required string Key { get; set; }
        public string? Value { get; set; }
    }

    public class IconField : Field
    {
        public string KeyIcon { get; set; } = string.Empty;
        public string KeyLink { get; set; } = string.Empty;
        public string ValueIcon { get; set; } = string.Empty;
        public string ValueLink { get; set; } = string.Empty;
    }
}
