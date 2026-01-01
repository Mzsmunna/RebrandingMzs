using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Entities
{
    public class BaseActivity //: BaseEntity
    {
        public bool IsActive { get; set; } = true;
        public DateTime ActivatedAt { get; set; } = DateTime.UtcNow; // DateTimeOffset.UtcNow;
        public DateTime? DeactivatedAt { get; set; }
        //public IDictionary<string, DateTime>? ActivityLogs { get; set; }
    }
}
