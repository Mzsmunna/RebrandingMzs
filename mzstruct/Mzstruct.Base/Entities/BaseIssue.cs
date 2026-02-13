using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Entities
{
    public class BaseIssue : BaseTask
    {
        public string ReportId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string AssignerId { get; set; } = string.Empty;
        public int Proogress { get; set; } = 0; // 0-100%
        public int LogTime { get; set; } = 0;
        public DateTime? DueAt { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
    }
}
