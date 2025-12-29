using Mzstruct.Base.Dtos;
using Mzstruct.Base.Entities;

namespace Mzstruct.DB.Providers.MongoDB.Contracts.IRepos
{
    //[EnforceResult]
    public interface IAppUserRepository : IMongoDBRepo<AppUser>
    {
        Task<AppUser?> LoginUser(string email, string password);
        Task<AppUser?> LoginUser(string email);
        Task<AppUser?> RegisterUser(AppUser user);
        Task<Result<List<AppUser>>> GetUsers(string clientId, string adminId);
        Task<Result<List<dynamic>>> GetAllUserToAssign();
        Task<Result<AppUser?>> UpdateUser(AppUser User);
    }
}
