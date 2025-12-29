using Mzstruct.Base.Dtos;
using Mzstruct.Base.Entities;

namespace Mzstruct.Common.Contracts.IQueries
{
    public interface IAppUserQuery
    {
        Task<Result<List<AppUser>>> GetAllUsers(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries);
        Task<Result<AppUser>> GetUser(string id);
        Task<Result<List<dynamic>>> AvailableUsersToAssign();
        Task<Result<long>> UsersCount(string searchQueries);
    }
}
