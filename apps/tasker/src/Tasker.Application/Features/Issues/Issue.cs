using Mzstruct.Base.Entities;
using Tasker.Application.Features.Users;

namespace Tasker.Application.Features.Issues
{
    public class Issue : Ticket
    {
        //relationships
        public User? User { get; set; }
        public User? Assigner { get; set; }
    }

    public class TaskerIssue : Ticket
    {
        //relationships
        public TaskerUser? User { get; set; }
        public TaskerUser? Assigner { get; set; }
    }

    public class Ticket : BaseEntity
    {
        public string ProjectId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserImg { get; set; } = string.Empty;
        public string AssignerId { get; set; } = string.Empty;
        public string AssignerName { get; set; } = string.Empty;
        public string AssignerImg { get; set; } = string.Empty;
        public int? LogTime { get; set; } = 0;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool? IsCompleted { get; set; } = false;
    }
}
