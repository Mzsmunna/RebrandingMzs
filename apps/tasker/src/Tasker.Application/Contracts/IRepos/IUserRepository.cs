using MongoDB.Bson;
using MongoDB.Driver;
using Mzstruct.Base.Contracts.IRepos;
using Mzstruct.Base.Dtos;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Attributes;
using Tasker.Application.Features.Users;

namespace Tasker.Application.Contracts.IRepos
{
    //[EnforceResult]
    public interface IUserRepository : IMongoDBRepo<User>
    {
        Task<User?> LoginUser(string email, string password);
        Task<User?> LoginUser(string email);
        Task<User?> RegisterUser(User user);
        Task<Result<List<User>>> GetUsers(string clientId, string adminId);
        Task<Result<List<dynamic>>> GetAllUserToAssign();
        Task<Result<User?>> UpdateUser(User User);
    }
}
