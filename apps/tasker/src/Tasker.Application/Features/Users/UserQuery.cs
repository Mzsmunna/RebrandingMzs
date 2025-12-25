using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mzstruct.Base.Dtos;
using Mzstruct.Base.Helpers;
using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Contracts.IQueries;
using Tasker.Application.Contracts.IRepos;

namespace Tasker.Application.Features.Users
{
    internal class UserQuery(//ILogger<UserQuery> logger,
        IUserRepository userRepository) : IUserQuery
    {
        public async Task<Result<List<User>>> GetAllUsers(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries)
        {
            List<SearchField>? queries = BaseHelper.JsonListDeserialize<SearchField>(searchQueries);
            return await userRepository.GetAllUsers(currentPage, pageSize, sortField, sortDirection, queries);
        }

        public async Task<Result<User>> GetUser(string id)
        {
            return await userRepository.GetUser(id);
        }

        public async Task<Result<List<dynamic>>> AvailableUsersToAssign()
        {
            return await userRepository.GetAllUserToAssign();
        }

        public async Task<Result<long>> UsersCount(string searchQueries)
        {
            List<SearchField>? queries = BaseHelper.JsonListDeserialize<SearchField>(searchQueries);
            return await userRepository.GetAllUserCount(queries);
        }
    }
}
