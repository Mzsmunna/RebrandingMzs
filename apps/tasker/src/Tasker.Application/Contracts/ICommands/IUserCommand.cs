using Mzstruct.Base.Patterns.Result;
using Tasker.Application.Features.Users;

namespace Tasker.Application.Contracts.ICommands
{
    public interface IUserCommand
    {
        Task<Result<UserModel?>> CreateUser(UserModel user);
        Task<Result<UserModel?>> UpdateUser(UserModel user);
        Task<Result<bool>> DeleteUser(string id);
    }
}
