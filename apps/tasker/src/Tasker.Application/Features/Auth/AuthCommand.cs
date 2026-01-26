using Mzstruct.Auth.Contracts.IServices;
using Mzstruct.Auth.Models.Dtos;
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
        public async Task<Result<UserModel?>> SignUp(SignUpDto signUpDto)
        {
            var result = await authService.SignUp(signUpDto);
            return result.To(user => user as UserModel);
        }

        public async Task<Result<string>> SignIn(SignInDto signInDto)
        {
            //var allUsers = await userSqlRepo.GetAllAsync();
            return await authService.SignIn(signInDto);
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

        public async Task<Result<string>> RefreshToken(string userId, string token, string refreshToken)
        {
            return await authService.RefreshToken(userId, token, refreshToken);
        }
    }
}
