using Mzstruct.Base.Contracts.IRepos;
using Mzstruct.Base.Dtos;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using Tasker.Application.Features.Issues;

namespace Tasker.Application.Contracts.IRepos
{
    public interface IIssueRepository : IMongoDBRepo<Issue>
    {
        Task<List<dynamic>> GetIssueStatByUserId(string userId);
        Task<List<Issue>> GetAllIssuesByAssigner(string assignerId);
        Task<List<Issue>> GetAllIssuesByAssigned(string assignedId);
        Task<Issue?> GetByTitle(string title);
    }
}
