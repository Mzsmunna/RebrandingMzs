using Mzstruct.Base.Dtos;
using Mzstruct.Base.Entities;
using CommonQueries = Mzstruct.Common.Contracts.IQueries;
using Tasker.Application.Contracts.IQueries;

namespace Tasker.Application.Features.Users
{
    internal class UserQuery(//ILogger<UserQuery> logger,
        CommonQueries.IUserQueryService userQuery) : IUserQuery
    {
        public async Task<Result<List<BaseUser>>> GetAllUsers(string sortField, string sortDirection)
        {
            return await userQuery.GetAllUsers(sortField, sortDirection);
        }

        public async Task<Result<List<BaseUser>>> GetAllUsers(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries)
        {
            return await userQuery.GetAllUsers(currentPage, pageSize, sortField, sortDirection, searchQueries);
        }

        public async Task<Result<BaseUser>> GetUser(string id)
        {
            return await userQuery.GetUser(id);
        }

        public async Task<Result<List<dynamic>>> AvailableUsersToAssign()
        {
            return await userQuery.AvailableUsersToAssign();
        }

        public async Task<Result<long>> UsersCount(string searchQueries)
        {
            return await userQuery.UsersCount(searchQueries);
        }
    }
}
