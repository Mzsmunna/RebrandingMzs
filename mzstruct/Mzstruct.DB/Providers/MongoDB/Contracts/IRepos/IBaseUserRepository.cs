using Mzstruct.Base.Dtos;
using Mzstruct.Base.Entities;

namespace Mzstruct.DB.Providers.MongoDB.Contracts.IRepos
{
    //[EnforceResult]
    public interface IBaseUserRepository : IMongoDBRepo<BaseUser>
    {
        Task<BaseUser?> LoginUser(string email, string password);
        Task<BaseUser?> LoginUser(string email);
        Task<BaseUser?> RegisterUser(BaseUser user);
        Task<Result<List<BaseUser>>> GetUsers(string clientId, string adminId);
        Task<Result<List<dynamic>>> GetAllUserToAssign();
        Task<Result<BaseUser?>> UpdateUser(BaseUser User);
    }
}
