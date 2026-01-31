using Mzstruct.Auth.Contracts.IServices;
using Mzstruct.Auth.Features.Commands;
using Mzstruct.Base.Dtos;
using Mzstruct.Base.Extensions;
using Tasker.Application.Contracts.ICommands;
using Tasker.Application.Features.Users;

namespace Tasker.Application.Features.Auth
{
    internal class AuthCommand(IAuthService authService
        //,IEFCoreBaseRepo<User> userSqlRepo
        ) : IAuthCommand
    {
        public async Task<Result<string>> SignUp(SignUpCommand payload)
        {
            return await authService.SignUp(payload);
            //return result.To(user => user as UserModel);
        }

        public async Task<Result<string>> SignIn(SignInCommand payload)
        {
            //var allUsers = await userSqlRepo.GetAllAsync();
            return await authService.SignIn(payload);
        }

        public async Task<Result<string>> SignInWithGoogle(string credential)
        {
            return await authService.SignInWithGoogle(credential);
        }

        public async Task<Result<string>> SignInWithGoogle()
        {
            return await authService.SignInWithGoogle();
        }

        public async Task<Result<string>> SignInWithGitHub()
        {
            return await authService.SignInWithGitHub();
        }

        public async Task<Result<string>> SignInWith(string email, string option)
        {
            return await authService.SignInWith(email, option);
        }

        public async Task<Result<string>> RefreshToken(string token, string refreshToken)
        {
            return await authService.RefreshToken(token, refreshToken);
        }
    }
}
