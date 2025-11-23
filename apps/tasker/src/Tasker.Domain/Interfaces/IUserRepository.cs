using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Domain.Entities;
using Tasker.Domain.Models;

namespace Tasker.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> LoginUser(string email, string password);
        Task<User?> LoginUser(string email);
        Task<User?> RegisterUser(string email);
        Task<List<User>?> GetAllByField(string fieldName, string fieldValue);
        Task<User?> GetUser(string id);
        Task<long> GetAllUserCount(List<SearchField>? searchQueries = null);
        Task<List<User>?> GetAllUsers(int currentPage, int pageSize, string sortField, string sortDirection, List<SearchField>? searchQueries = null);
        Task<List<dynamic>?> GetAllUserToAssign();
        Task<User?> Save(IEntity entity);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteById(string _id);
    }
}
