using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tasker.Application.Features.Issues
{
    public record IssueModel : BaseModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Type { get; set; } = string.Empty;
        [Required]
        public string Summary { get; set; } = string.Empty;
        [Required]
        public string AssignerId { get; set; } = string.Empty;
        [Required]
        public string AssignedId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        //public int? LogTime { get; set; } = 0;
        //public DateTime? StartDate { get; set; }
        //public DateTime? EndDate { get; set; }
        //public DateTime? DueDate { get; set; }
    }
}
