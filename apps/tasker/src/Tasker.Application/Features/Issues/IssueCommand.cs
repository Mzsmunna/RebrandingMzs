using Kernel.Drivers.Dtos;
using Kernel.Drivers.Errors;
using Kernel.Drivers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Interfaces;
using Tasker.Domain.Entities;

namespace Tasker.Application.Features.Issues
{
    internal class IssueCommand(IIssueRepository issueRepository,
        IUserRepository userRepository) : IIssueCommand
    {
        public async Task<Result<Issue>> CreateIssue(Issue issue)
        {
            return await SaveIssue(issue);
        }

        public async Task<Result<Issue>> UpdateIssue(Issue issue)
        {
            return await SaveIssue(issue);
        }

        private async Task<Result<Issue>> SaveIssue(Issue issue)
        {
            if (issue != null)
            {
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
            else
            {
                return ClientError.BadRequest;
            }
        }

        public async Task<Result<bool>> DeleteIssue(string id)
        {
            var result = await issueRepository.DeleteById(id);
            return result; //Ok(users);
        }
    }
}
