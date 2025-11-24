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
        Task<Issue?> GetById(string _id);
        Task<List<Issue>?> GetAllByField(string fieldName, string fieldValue);
        Task<long> GetAllIssueCount(List<SearchField>? searchQueries = null);
        Task<List<Issue>?> GetAllIssues(int currentPage, int pageSize, string sortField, string sortDirection, List<SearchField>? searchQueries = null);
        Task<List<Issue>?> GetAllIssues();
        Task<List<dynamic>?> GetIssueStatByUserId(string userId);
        Task<List<Issue>?> GetAllIssuesByAssigner(string assignerId);
        Task<List<Issue>?> GetAllIssuesByAssigned(string assignedId);
        Task<Issue?> GetIssuesById(string id);
        Task<Issue?> GetByTitle(string title);
        Task<Issue?> Save(IEntity entity);
        Task<bool> DeleteById(string _id);
    }
}
