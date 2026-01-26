using Mzstruct.Base.Dtos;
using Mzstruct.Base.Entities;

namespace Mzstruct.Common.Contracts.IQueries
{
    public interface IUserQueryService
    {
        Task<Result<List<BaseUser>>> GetAllUsers(string sortField = "", string sortDirection = "");
        Task<Result<List<BaseUser>>> GetAllUsers(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries);
        Task<Result<BaseUser>> GetUser(string id);
        Task<Result<List<dynamic>>> AvailableUsersToAssign();
        Task<Result<long>> UsersCount(string searchQueries);
    }
}
