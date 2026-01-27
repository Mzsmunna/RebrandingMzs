using Mzstruct.Base.Dtos;
using Mzstruct.Base.Entities;

namespace Mzstruct.DB.Providers.MongoDB.Contracts.IRepos
{
    //[EnforceResult]
    public interface IBaseUserRepository<TUser> : IMongoDBRepo<TUser> where TUser : BaseUser 
    {
        Task<TUser?> LoginUser(string email, string password);
        Task<TUser?> LoginUser(string email);
        Task<TUser?> RegisterUser(TUser user);
        Task<Result<List<TUser>>> GetUsers(string clientId, string adminId);
        Task<Result<List<dynamic>>> GetAllUserToAssign();
        Task<Result<TUser?>> UpdateUser(TUser User);
    }
}
