using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Drivers.Models
{
    public class SearchField
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public string DataSeparator { get; set; } = string.Empty;
        public bool IsId { get; set; } = false;
        public bool IsString { get; set; } = true;
        public bool IsCaseSensitive { get; set; } = false;
        public bool IsPartialMatch { get; set; } = false;
        public bool IsBoolean { get; set; } = false;
        public bool IsDateTime { get; set; } = false;
        public bool IsAndQuery { get; set; } = true;
        public bool IsEncrypted { get; set; } = false;
        public int QueryOrder { get; set; } = 0;
    }
}
