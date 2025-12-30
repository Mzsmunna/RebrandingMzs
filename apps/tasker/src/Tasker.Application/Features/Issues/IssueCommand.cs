using Mzstruct.Base.Dtos;
using Mzstruct.Base.Errors;
using Mzstruct.Common.Extensions;
using Mzstruct.Common.Mappings;
using Mzstruct.DB.Providers.MongoDB.Contracts.IRepos;
using Tasker.Application.Contracts.ICommands;
using Tasker.Application.Contracts.IRepos;
using Tasker.Application.Validators;

namespace Tasker.Application.Features.Issues
{
    internal class IssueCommand(IIssueRepository issueRepository,
        IBaseUserRepository userRepository) : IIssueCommand
    {
        public async Task<Result<Issue>> CreateIssue(IssueModel issue)
        {
            var validation = await TaskerValidator.ValidateIssue(issue);
            if (validation.IsValid is false)
                return Error.Validation("IssueCommand.CreateIssue.InvalidForm", 
                    "One or more Issue form invalid", validation.ToErrorDictionary());
            var issueEntity = issue.ToEntity<Issue, IssueModel>();
            return await SaveIssue(issueEntity);
        }

        public async Task<Result<Issue>> UpdateIssue(Issue issue)
        {
            return await SaveIssue(issue);
        }

        private async Task<Result<Issue>> SaveIssue(Issue issue)
        {
            var validation = await TaskerValidator.ValidateIssue(issue);
            if (validation.IsValid is false)
                return Error.Validation("IssueCommand.UpdateIssue.InvalidState", 
                    "Updated Issue info seems in invalid state", validation.ToErrorDictionary());

            if (issue is null)
                return ClientError.BadRequest;

            if (!string.IsNullOrEmpty(issue.AssignedId))
            {
                var user = userRepository.GetById(issue.AssignedId).Result;
                if (user != null)
                {
                    issue.AssignedName = user.FirstName + " " + user.LastName;
                    issue.AssignedImg = user.Img ?? "";
                }
            }

            if (issue.Created is not null && !string.IsNullOrEmpty(issue.Created.By))
            {
                var user = await userRepository.GetById(issue.Created.By);
                if (user != null)
                {
                    issue.Created.Name = user.FirstName + " " + user.LastName;
                    issue.Created.Image = user.Img;
                }
            }

            var result = await issueRepository.Save(issue);
            return result ?? issue;
        }

        public async Task<Result<bool>> DeleteIssue(string id)
        {
            if (string.IsNullOrEmpty(id))
                return ClientError.BadRequest;
            var result = await issueRepository.DeleteById(id);
            return result != null ? true : false;
        }
    }
}
