using Microsoft.AspNetCore.Mvc;
using Mzstruct.Base.Dtos;
using Mzstruct.Base.Errors;
using Mzstruct.Base.Helpers;
using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Features.Users;

namespace Tasker.Application.Features.Issues
{
    internal class IssueQuery(IIssueRepository issueRepository) : IIssueQuery
    {
        public async Task<Result<List<Issue>?>> GetAllIssues(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries)
        {
            List<SearchField>? queries = BaseHelper.JsonListDeserialize<SearchField>(searchQueries);
            return await issueRepository.GetAllIssues(currentPage, pageSize, sortField, sortDirection, queries);
        }

        public async Task<Result<Issue?>> GetIssue(string id)
        {
            return await issueRepository.GetById(id);
        }

        public async Task<Result<long>> GetIssuesCount(string searchQueries)
        {
            List<SearchField>? queries = BaseHelper.JsonListDeserialize<SearchField>(searchQueries);
            return await issueRepository.GetAllIssueCount(queries);
        }

        public async Task<Result<List<dynamic>?>> GetIssuesStatus(string userId)
        {
            return await issueRepository.GetIssueStatByUserId(userId);
        }

        public async Task<Result<long>> GetIssuesCountByAssigner(string searchQueries)
        {
            List<SearchField>? queries = BaseHelper.JsonListDeserialize<SearchField>(searchQueries);
            if (queries != null && queries.Count > 0 && queries.Any(x => !string.IsNullOrEmpty(x.Key) && x.Key.ToLower().Equals("assignerid")))
                return await issueRepository.GetAllIssueCount(queries);
            else return 0;
        }

        public async Task<Result<List<Issue>?>> GetIssuesByAssigner(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries)
        {
            List<SearchField>? queries = BaseHelper.JsonListDeserialize<SearchField>(searchQueries);
            if (queries != null && queries.Count > 0 && queries.Any(x => !string.IsNullOrEmpty(x.Key) && x.Key.ToLower().Equals("assignerid")))
                return await issueRepository.GetAllIssues(currentPage, pageSize, sortField, sortDirection, queries);
            else return Result<List<Issue>?>.Ok([]);
        }

        public async Task<Result<long>> GetIssuesCountByAssigned(string searchQueries)
        {
            List<SearchField>? queries = BaseHelper.JsonListDeserialize<SearchField>(searchQueries);
            if (queries != null && queries.Count > 0 && queries.Any(x => !string.IsNullOrEmpty(x.Key) && x.Key.ToLower().Equals("assignerid")))
                return await issueRepository.GetAllIssueCount(queries);
            else return 0;
        }

        public async Task<Result<List<Issue>?>> GetIssuesByAssigned(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries)
        {
            List<SearchField>? queries = BaseHelper.JsonListDeserialize<SearchField>(searchQueries);
            if (queries != null && queries.Count > 0 && queries.Any(x => !string.IsNullOrEmpty(x.Key) && x.Key.ToLower().Equals("assignerid")))
                return await issueRepository.GetAllIssues(currentPage, pageSize, sortField, sortDirection, queries);
            else return Result<List<Issue>?>.Ok([]);
        }
    }
}
