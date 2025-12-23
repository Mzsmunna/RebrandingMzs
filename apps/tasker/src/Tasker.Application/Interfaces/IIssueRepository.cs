using Mzstruct.Base.Dtos;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using Tasker.Domain.Entities;
using Tasker.Domain.Models;

namespace Tasker.Application.Interfaces
{
    public interface IIssueRepository //: IMongoRepository
    {
        Task<Result<Issue?>> GetById(string _id);
        Task<Result<List<Issue>?>> GetAllByField(string fieldName, string fieldValue);
        Task<Result<long>> GetAllIssueCount(List<SearchField>? searchQueries = null);
        Task<Result<List<Issue>?>> GetAllIssues(int currentPage, int pageSize, string sortField, string sortDirection, List<SearchField>? searchQueries = null);
        Task<Result<List<Issue>?>> GetAllIssues();
        Task<Result<List<dynamic>?>> GetIssueStatByUserId(string userId);
        Task<Result<List<Issue>?>> GetAllIssuesByAssigner(string assignerId);
        Task<Result<List<Issue>?>> GetAllIssuesByAssigned(string assignedId);
        Task<Result<Issue?>> GetIssuesById(string id);
        Task<Result<Issue?>> GetByTitle(string title);
        Task<Result<Issue?>> Save(IEntity entity);
        Task<Result<bool>> DeleteById(string _id);
    }
}
