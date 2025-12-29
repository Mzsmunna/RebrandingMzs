using Mzstruct.Base.Dtos;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Models;
using Mzstruct.Common.Contracts.ICommands;
using Tasker.Application.Contracts.ICommands;

namespace Tasker.Application.Features.Users
{
    internal class UserCommand(IAppUserCommand userCommand) : IUserCommand
    {
        public async Task<Result<AppUser?>> CreateUser(AppUserModel user)
        {
            return await userCommand.CreateUser(user);
        }

        public async Task<Result<AppUser?>> UpdateUser(AppUser user)
        {
            return await userCommand.UpdateUser(user);
        }

        public async Task<Result<bool>> DeleteUser(string id)
        {
            return await userCommand.DeleteUser(id);
        }
    }
}
