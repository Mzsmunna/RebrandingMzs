using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Application.Errors
{
    public static class IssueError
    {
        public static readonly Error EmptyTitle = new(ErrorType.Validation, "Issue.Empty_Title", "Issue Title can't be empty");
    }
}
