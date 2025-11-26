using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Attributes;
using Tasker.Application.Models;
using Tasker.Domain.Entities;
using Tasker.Domain.Models;

namespace Tasker.Application.Interfaces
{
    [EnforceResult]
    public interface IUserRepository
    {
        Task<Result<User>> LoginUser(string email, string password);
        Task<Result<User>> LoginUser(string email);
        Task<Result<User>> RegisterUser(string email);
        Task<Result<List<User>>> GetAllByField(string fieldName, string fieldValue);
        Task<Result<User>> GetUser(string id);
        Task<Result<long>> GetAllUserCount(List<SearchField>? searchQueries = null);
        Task<Result<List<User>>> GetAllUsers(int currentPage, int pageSize, string sortField, string sortDirection, List<SearchField>? searchQueries = null);
        Task<Result<List<dynamic>>> GetAllUserToAssign();
        Task<Result<User>> Save(IEntity entity);
        Task<Result<bool>> UpdateUser(User user);
        Task<Result<bool>> DeleteById(string _id);
    }
}
