using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Domain.Entities;
using Tasker.Domain.Models;

namespace Tasker.Application.Interfaces
{
    internal interface IMongoRepository
    {
        #region Common_Methods
        Task<Issue> GetById(string _id);
        Task<List<Issue>> GetAllByField(string fieldName, string fieldValue);
        Task<long> GetAllIssueCount(List<SearchField>? searchQueries = null);
        Task<List<Issue>> GetAllIssues(int currentPage, int pageSize, string sortField, string sortDirection, List<SearchField>? searchQueries = null);
        Task<int> GetAllCount();
        Task<string> SaveMany(IEnumerable<IEntity> records);
        #endregion
    }
}
