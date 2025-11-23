using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Domain.Entities;
using Tasker.Domain.Models;

namespace Tasker.Domain.Interfaces
{
    public interface IIssueRepository
    {
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
