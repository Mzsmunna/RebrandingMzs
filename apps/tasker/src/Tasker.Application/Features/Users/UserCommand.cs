using Mzstruct.Base.Dtos;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Models;
using CommonCommands = Mzstruct.Common.Contracts.ICommands;
using Tasker.Application.Contracts.ICommands;

namespace Tasker.Application.Features.Users
{
    internal class UserCommand(CommonCommands.IUserCommand userCommand) : IUserCommand
    {
        public async Task<Result<BaseUser?>> CreateUser(BaseUserModel user)
        {
            return await userCommand.CreateUser(user);
        }

        public async Task<Result<BaseUser?>> UpdateUser(BaseUser user)
        {
            return await userCommand.UpdateUser(user);
        }

        public async Task<Result<bool>> DeleteUser(string id)
        {
            return await userCommand.DeleteUser(id);
        }
    }
}
