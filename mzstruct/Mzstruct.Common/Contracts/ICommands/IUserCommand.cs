using Mzstruct.Base.Dtos;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Models;

namespace Mzstruct.Common.Contracts.ICommands
{
    public interface IUserCommand
    {
        Task<Result<BaseUser?>> CreateUser(BaseUserModel user);
        Task<Result<BaseUser?>> UpdateUser(BaseUser user);
        Task<Result<bool>> DeleteUser(string id);
    }
}
