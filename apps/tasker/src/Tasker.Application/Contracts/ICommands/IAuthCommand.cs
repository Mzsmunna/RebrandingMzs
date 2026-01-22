using Mzstruct.Base.Dtos;
using Mzstruct.Common.Features.Auth;
using Tasker.Application.Features.Users;

namespace Tasker.Application.Contracts.ICommands
{
    public interface IAuthCommand
    {
        Task<Result<UserModel?>> SignUp(SignUpDto user);
        Task<Result<string>> SignIn(SignInDto user);
        Task<Result<string>> SignInWithGoogle(string credential);
        Task<Result<string>> SignInWithGitHub();
        Task<Result<string>> SignInWith(string email, string option);
        Task<Result<string>> RefreshToken(string userId, string token, string refreshToken);
    }
}
