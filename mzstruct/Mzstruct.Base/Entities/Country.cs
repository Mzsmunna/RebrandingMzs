using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Entities
{
    public class Country
    {
        public string Id { get; set; } = Guid.CreateVersion7().ToString();
        public required string Name { get; set; }
        public string Suffix { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string DialCode { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
    }
}
