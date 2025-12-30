using Mzstruct.Base.Dtos;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Errors;
using Mzstruct.Base.Helpers;
using Mzstruct.Base.Models;
using Mzstruct.Common.Contracts.IQueries;
using Mzstruct.DB.Providers.MongoDB.Contracts.IRepos;

namespace Mzstruct.Common.Features.Users
{
    internal class UserQuery(//ILogger<UserQuery> logger,
        IBaseUserRepository userRepository) : IUserQuery
    {
        public async Task<Result<List<BaseUser>>> GetAllUsers(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries)
        {
            List<SearchField>? queries = BaseHelper.JsonListDeserialize<SearchField>(searchQueries);
            return await userRepository.GetAll(currentPage, pageSize, sortField, sortDirection, queries);
        }

        public async Task<Result<BaseUser>> GetUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return ClientError.BadRequest;
            var result = await userRepository.GetById(id);
            return result != null ? result : Error.NotFound("UserQuery.GetUser", "We didn't find any user with id: " + id);
        }

        public async Task<Result<List<dynamic>>> AvailableUsersToAssign()
        {
            return await userRepository.GetAllUserToAssign();
        }

        public async Task<Result<long>> UsersCount(string searchQueries)
        {
            List<SearchField>? queries = BaseHelper.JsonListDeserialize<SearchField>(searchQueries);
            return await userRepository.GetCount(queries);
        }
    }
}
