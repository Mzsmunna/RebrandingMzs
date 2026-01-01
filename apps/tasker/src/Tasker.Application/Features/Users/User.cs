using Mzstruct.Base.Entities;
using Tasker.Application.Features.Issues;

namespace Tasker.Application.Features.Users
{
    public class User : BaseUser
    {
        // relationships
        public ICollection<Issue>? AssignerIssues { get; set; }
        public ICollection<Issue>? AssignedIssues { get; set; }
    }
}
