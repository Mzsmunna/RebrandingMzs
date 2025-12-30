using Mzstruct.Base.Dtos;
using Mzstruct.Base.Models;
using Mzstruct.Common.Features.Auth;
using Tasker.Application.Contracts.ICommands;
using CommonCommands = Mzstruct.Common.Contracts.ICommands;

namespace Tasker.Application.Features.Auth
{
    internal class AuthCommand(CommonCommands.IAuthCommand authCommand) : IAuthCommand
    {
        public async Task<Result<BaseUserModel>> SignUp(SignUpDto signUpDto)
        {
            return await authCommand.SignUp(signUpDto);
        }

        public async Task<Result<string>> SignIn(SignInDto signInDto)
        {        
            return await authCommand.SignIn(signInDto);
        }

        public async Task<Result<string>> SignInWithGoogle(string credential)
        {
            return await SignInWithGoogle(credential);
        }

        public async Task<Result<string>> RefreshToken(string userId, string refreshToken)
        {
            return await RefreshToken(userId, refreshToken);
        }
    }
}
