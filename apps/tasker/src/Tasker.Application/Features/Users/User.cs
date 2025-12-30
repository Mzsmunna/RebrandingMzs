using Mzstruct.Base.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Features.Issues;

namespace Tasker.Application.Features.Users
{
    public class User : BaseUser
    {
        // relationships
        public ICollection<Issue>? issues { get; set; }
    }
}
