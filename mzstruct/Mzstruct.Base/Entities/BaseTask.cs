using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Entities
{
    public class BaseTask : BaseEntity
    {
        public required string Title { get; set; }
        public required string Type { get; set; }
        public required string Status { get; set; }
        public bool IsDone { get; set; } = false;
        public string? Comment { get; set; }
        public string? Summary { get; set; }
        public string? Details { get; set; }
    }
}
