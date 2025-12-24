using Mzstruct.Base.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Domain.Entities
{
    public class Issue : BaseEntity
    {
        public string ProjectId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AssignedName { get; set; } = string.Empty;
        public string AssignedImg { get; set; } = string.Empty;
        public string AssignedId { get; set; } = string.Empty;
        public int? LogTime { get; set; } = 0;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public bool? IsActive { get; set; } = false;
        public bool? IsDeleted { get; set; } = false;
        public bool? IsCompleted { get; set; } = false;
    }
}
