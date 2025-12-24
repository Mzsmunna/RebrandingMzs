using Mzstruct.Base.Dtos;
using Mzstruct.Base.Errors;
using Mzstruct.Base.Models;
using Mzstruct.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Features.Users;

namespace Tasker.Application.Features.Issues
{
    public interface IIssueCommand
    {
        Task<Result<Issue>> CreateIssue(IssueModel issue);
        Task<Result<Issue>> UpdateIssue(Issue issue);
        Task<Result<bool>> DeleteIssue(string id);
    }
}
