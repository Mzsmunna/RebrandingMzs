using Mzstruct.Base.Entities;
using Mzstruct.Base.Models;
using Mzstruct.Base.Patterns.Result;

namespace Mzstruct.Common.Contracts.ICommands
{
    public interface IUserCommandService
    {
        Task<Result<BaseUserModel?>> CreateUser(BaseUserModel user);
        Task<Result<BaseUserModel?>> UpdateUser(BaseUserModel user);
        Task<Result<bool>> DeleteUser(string id);
    }
}
