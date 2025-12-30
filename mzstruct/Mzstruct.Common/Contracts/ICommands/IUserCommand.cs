using Mzstruct.Base.Dtos;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Models;

namespace Mzstruct.Common.Contracts.ICommands
{
    public interface IUserCommand
    {
        Task<Result<BaseUserModel?>> CreateUser(BaseUserModel user);
        Task<Result<BaseUserModel?>> UpdateUser(BaseUserModel user);
        Task<Result<bool>> DeleteUser(string id);
    }
}
