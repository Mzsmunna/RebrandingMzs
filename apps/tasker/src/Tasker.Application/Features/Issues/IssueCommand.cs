using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mzstruct.Base.Dtos;
using Mzstruct.Base.Errors;
using Mzstruct.Base.Models;
using Mzstruct.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Contracts.ICommands;
using Tasker.Application.Contracts.IRepos;
using Tasker.Application.Features.Auth;
using Tasker.Application.Validators;

namespace Tasker.Application.Features.Issues
{
    internal class IssueCommand(IIssueRepository issueRepository,
        IUserRepository userRepository) : IIssueCommand
    {
        public async Task<Result<Issue>> CreateIssue(IssueModel issue)
        {
            var validation = await TaskerValidator.ValidateIssue(issue);
            if (validation.IsValid is false)
                return Error.Validation("IssueCommand.CreateIssue.InvalidInput", "User input invalid");
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
                return Error.Validation("IssueCommand.UpdateIssue.InvalidState", "Issue state is invalid");

            if (issue is null)
                return ClientError.BadRequest;

            if (issue.Created == null)
                issue.Created = new AppEvent();

            if (string.IsNullOrEmpty(issue.Id))
                issue.Created.At = DateTime.UtcNow;
            else if (issue.Modified != null)
                issue.Modified.At = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(issue.AssignedId))
            {
                var response = userRepository.GetUser(issue.AssignedId).Result;
                issue = response.Map(
                    Ok: user =>
                    {
                        issue.AssignedName = user.FirstName + " " + user.LastName;
                        issue.AssignedImg = user.Img ?? "";
                        return issue;
                    },
                    Err: _ => issue
                );
            }

            if (!string.IsNullOrEmpty(issue.Created.By))
            {
                var response = await userRepository.GetUser(issue.Created.By);
                issue = response.Map(
                    Ok: user =>
                    {
                        issue.Created.Name = user.FirstName + " " + user.LastName;
                        issue.Created.Image = user.Img;
                        return issue;
                    },
                    Err: _ => issue
                );
            }

            var result = await issueRepository.Save(issue);
            return result!;
        }

        public async Task<Result<bool>> DeleteIssue(string id)
        {
            if (string.IsNullOrEmpty(id))
                return ClientError.BadRequest;
            var result = await issueRepository.DeleteById(id);
            return result;
        }
    }
}
