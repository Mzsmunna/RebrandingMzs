using Mzstruct.Base.Dtos;
using Mzstruct.Common.Extensions;
using Mzstruct.Common.Features.Auth;
using Tasker.Application.Contracts.ICommands;
using Tasker.Application.Features.Users;
using CommonCommands = Mzstruct.Common.Contracts.ICommands;

namespace Tasker.Application.Features.Auth
{
    internal class AuthCommand(CommonCommands.IAuthCommand authCommand
        //,IEFCoreBaseRepo<User> userSqlRepo
        ) : IAuthCommand
    {
        public async Task<Result<UserModel?>> SignUp(SignUpDto signUpDto)
        {
            var result = await authCommand.SignUp(signUpDto);
            return result.To(user => user as UserModel);
        }

        public async Task<Result<string>> SignIn(SignInDto signInDto)
        {
            //var allUsers = await userSqlRepo.GetAllAsync();
            return await authCommand.SignIn(signInDto);
        }

        public async Task<Result<string>> SignInWithGoogle(string credential)
        {
            return await authCommand.SignInWithGoogle(credential);
        }

        public async Task<Result<string>> SignInWithGoogle()
        {
            return await authCommand.SignInWithGoogle();
        }

        public async Task<Result<string>> SignInWithGitHub()
        {
            return await authCommand.SignInWithGitHub();
        }

        public async Task<Result<string>> SignInWith(string email, string option)
        {
            return await authCommand.SignInWith(email, option);
        }

        public async Task<Result<string>> RefreshToken(string userId, string token, string refreshToken)
        {
            return await authCommand.RefreshToken(userId, token, refreshToken);
        }
    }
}
