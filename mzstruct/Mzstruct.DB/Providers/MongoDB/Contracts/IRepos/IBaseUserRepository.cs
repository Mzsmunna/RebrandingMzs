using Mzstruct.Base.Entities;
using Mzstruct.Base.Patterns.Result;

namespace Mzstruct.DB.Providers.MongoDB.Contracts.IRepos
{
    //[EnforceResult]
    public interface IBaseUserRepository<TUser> : IMongoDBRepo<TUser> where TUser : BaseUser 
    {
        Task<Result<List<TUser>>> GetUsers(string clientId, string adminId);
        Task<Result<List<dynamic>>> GetAllUserToAssign();
        Task<Result<TUser?>> UpdateUser(TUser User);
    }
}
