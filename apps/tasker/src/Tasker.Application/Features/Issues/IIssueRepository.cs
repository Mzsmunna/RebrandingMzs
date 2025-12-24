using Mzstruct.Base.Dtos;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Tasker.Application.Features.Issues
{
    public interface IIssueRepository //: IMongoRepository
    {
        Task<Result<Issue?>> GetById(string id);
        Task<Result<List<Issue>?>> GetAllByField(string fieldName, string fieldValue);
        Task<Result<long>> GetAllIssueCount(List<SearchField>? searchQueries = null);
        Task<Result<List<Issue>?>> GetAllIssues(int currentPage, int pageSize, string sortField, string sortDirection, List<SearchField>? searchQueries = null);
        Task<Result<List<Issue>?>> GetAllIssues();
        Task<Result<List<dynamic>?>> GetIssueStatByUserId(string userId);
        Task<Result<List<Issue>?>> GetAllIssuesByAssigner(string assignerId);
        Task<Result<List<Issue>?>> GetAllIssuesByAssigned(string assignedId);
        Task<Result<Issue?>> GetIssuesById(string id);
        Task<Result<Issue?>> GetByTitle(string title);
        Task<Result<Issue?>> Save(BaseEntity entity);
        Task<Result<bool>> DeleteById(string _id);
    }
}
