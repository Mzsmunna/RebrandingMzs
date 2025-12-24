using Mzstruct.Base.Dtos;
using Mzstruct.Base.Helpers;
using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Application.Features.Issues
{
    public interface IIssueQuery
    {
        Task<Result<List<Issue>?>> GetAllIssues(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries);
        Task<Result<Issue?>> GetIssue(string id);
        Task<Result<long>> GetIssuesCount(string searchQueries);
        Task<Result<List<dynamic>?>> GetIssuesStatus(string userId);
        Task<Result<long>> GetIssuesCountByAssigner(string searchQueries);
        Task<Result<List<Issue>?>> GetIssuesByAssigner(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries);
        Task<Result<long>> GetIssuesCountByAssigned(string searchQueries);
        Task<Result<List<Issue>?>> GetIssuesByAssigned(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries);
    }
}
