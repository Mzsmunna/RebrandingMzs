using Microsoft.AspNetCore.Mvc;
using Mzstruct.Base.Dtos;
using Mzstruct.Base.Errors;
using Mzstruct.Base.Helpers;
using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Features.Users;
using Tasker.Application.Contracts.IQueries;
using Tasker.Application.Contracts.IRepos;

namespace Tasker.Application.Features.Issues
{
    internal class IssueQuery(IIssueRepository issueRepository) : IIssueQuery
    {
        public async Task<Result<List<Issue>>> GetAllIssues(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries)
        {
            List<SearchField>? queries = BaseHelper.JsonListDeserialize<SearchField>(searchQueries);
            return await issueRepository.GetAll(currentPage, pageSize, sortField, sortDirection, queries);
        }

        public async Task<Result<Issue?>> GetIssue(string id)
        {
            if (string.IsNullOrEmpty(id))
                return ClientError.BadRequest;
            return await issueRepository.GetById(id);
        }

        public async Task<Result<long>> GetIssuesCount(string searchQueries)
        {
            List<SearchField>? queries = BaseHelper.JsonListDeserialize<SearchField>(searchQueries);
            return await issueRepository.GetCount(queries);
        }

        public async Task<Result<List<dynamic>>> GetIssuesStatus(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return ClientError.BadRequest;
            return await issueRepository.GetIssueStatByUserId(userId);
        }

        public async Task<Result<long>> GetIssuesCountByAssigner(string searchQueries)
        {
            List<SearchField>? queries = BaseHelper.JsonListDeserialize<SearchField>(searchQueries);
            if (queries != null && queries.Count > 0 && queries.Any(x => !string.IsNullOrEmpty(x.Key) && x.Key.ToLower().Equals("assignerid")))
                return await issueRepository.GetCount(queries);
            else return 0;
        }

        public async Task<Result<List<Issue>>> GetIssuesByAssigner(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries)
        {
            List<SearchField>? queries = BaseHelper.JsonListDeserialize<SearchField>(searchQueries);
            if (queries != null && queries.Count > 0 && queries.Any(x => !string.IsNullOrEmpty(x.Key) && x.Key.ToLower().Equals("assignerid")))
                return await issueRepository.GetAll(currentPage, pageSize, sortField, sortDirection, queries);
            else return Result<List<Issue>>.Ok([]);
        }

        public async Task<Result<long>> GetIssuesCountByAssigned(string searchQueries)
        {
            List<SearchField>? queries = BaseHelper.JsonListDeserialize<SearchField>(searchQueries);
            if (queries != null && queries.Count > 0 && queries.Any(x => !string.IsNullOrEmpty(x.Key) && x.Key.ToLower().Equals("assignerid")))
                return await issueRepository.GetCount(queries);
            else return 0;
        }

        public async Task<Result<List<Issue>>> GetIssuesByAssigned(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries)
        {
            List<SearchField>? queries = BaseHelper.JsonListDeserialize<SearchField>(searchQueries);
            if (queries != null && queries.Count > 0 && queries.Any(x => !string.IsNullOrEmpty(x.Key) && x.Key.ToLower().Equals("assignerid")))
                return await issueRepository.GetAll(currentPage, pageSize, sortField, sortDirection, queries);
            else return Result<List<Issue>>.Ok([]);
        }
    }
}
