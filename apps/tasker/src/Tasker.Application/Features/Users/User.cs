using Mzstruct.Base.Entities;
using Mzstruct.DB.EFCore.Entities;
using Tasker.Application.Features.Issues;

namespace Tasker.Application.Features.Users
{
    public class User : BaseUser
    {
        // relationships
        public ICollection<Issue>? AssignerIssues { get; set; }
        public ICollection<Issue>? UserIssues { get; set; }
    }

    public class TaskerUser : UserIdentity
    {
        // relationships
        public ICollection<TaskerIssue>? AssignerIssues { get; set; }
        public ICollection<TaskerIssue>? UserIssues { get; set; }
    }
}
