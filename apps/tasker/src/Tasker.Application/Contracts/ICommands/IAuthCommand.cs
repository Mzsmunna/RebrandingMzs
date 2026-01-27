using Mzstruct.Auth.Models.Dtos;
using Mzstruct.Base.Dtos;
using Tasker.Application.Features.Users;

namespace Tasker.Application.Contracts.ICommands
{
    public interface IAuthCommand
    {
        Task<Result<string>> SignUp(SignUpDto user);
        Task<Result<string>> SignIn(SignInDto user);
        Task<Result<string>> SignInWithGoogle(string credential);
        Task<Result<string>> SignInWithGoogle();
        Task<Result<string>> SignInWithGitHub();
        Task<Result<string>> SignInWith(string email, string option);
        Task<Result<string>> RefreshToken(string token, string refreshToken);
    }
}
