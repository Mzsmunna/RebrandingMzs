using Mzstruct.Base.Entities;
using Tasker.Application.Features.Issues;

namespace Tasker.Application.Features.Users
{
    public class User : BaseUser
    {
        // relationships
        public ICollection<Issue>? Issues { get; set; }
    }
}
