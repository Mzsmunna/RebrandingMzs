using Mzstruct.Base.Dtos;
using Mzstruct.Base.Extensions;
using Tasker.Application.Contracts.ICommands;
using Mzstruct.Common.Contracts.ICommands;

namespace Tasker.Application.Features.Users
{
    internal class UserCommand(IUserCommandService userCommand) : IUserCommand
    {
        public async Task<Result<UserModel?>> CreateUser(UserModel user)
        {
            var result = await userCommand.CreateUser(user);
            return result.To(user => user as UserModel);
        }

        public async Task<Result<UserModel?>> UpdateUser(UserModel user)
        {
            var result = await userCommand.UpdateUser(user);
            return result.To(user => user as UserModel);
        }

        public async Task<Result<bool>> DeleteUser(string id)
        {
            return await userCommand.DeleteUser(id);
        }
    }
}
