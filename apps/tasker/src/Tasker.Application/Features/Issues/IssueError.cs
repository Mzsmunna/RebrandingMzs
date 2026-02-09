using Mzstruct.Base.Enums;
using Mzstruct.Base.Patterns.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Application.Features.Issues
{
    public static class IssueError
    {
        public static readonly Error EmptyTitle = new(ErrorType.Validation, "Issue.Title.Empty", "Issue Title can't be empty");
    }
}
