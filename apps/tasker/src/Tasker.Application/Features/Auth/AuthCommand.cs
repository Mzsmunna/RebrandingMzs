using Mzstruct.Base.Dtos;
using Mzstruct.Base.Models;
using Mzstruct.Common.Contracts.ICommands;
using Mzstruct.Common.Features.Auth;
using Tasker.Application.Contracts.ICommands;

namespace Tasker.Application.Features.Auth
{
    internal class AuthCommand(IAppAuthCommand authCommand) : IAuthCommand
    {
        public async Task<Result<AppUserModel>> SignUp(AppSignUpDto signUpDto)
        {
            return await authCommand.SignUp(signUpDto);
        }

        public async Task<Result<string>> SignIn(AppSignInDto signInDto)
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
