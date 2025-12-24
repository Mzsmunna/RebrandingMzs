using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzstruct.Base.Dtos;
using Mzstruct.Base.Helpers;
using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Contracts.IRepos;
using Tasker.Application.Features.Users;

namespace Tasker.Application.Contracts.IQueries
{
    public interface IUserQuery
    {
        Task<Result<List<User>>> GetAllUsers(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries);
        Task<Result<User>> GetUser(string id);
        Task<Result<List<dynamic>>> AvailableUsersToAssign();
        Task<Result<long>> UsersCount(string searchQueries);
    }
}
