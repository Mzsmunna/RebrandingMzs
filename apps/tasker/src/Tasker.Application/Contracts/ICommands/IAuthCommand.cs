using Mzstruct.Auth.Features.Commands;
using Mzstruct.Base.Patterns.Result;
using Tasker.Application.Features.Users;

namespace Tasker.Application.Contracts.ICommands
{
    public interface IAuthCommand
    {
        Task<Result<string>> SignUp(SignUpCommand request);
        Task<Result<string>> SignIn(SignInCommand request);
        Task<Result<string>> SignInWithGoogle(string credential);
        Task<Result<string>> SignInWithGoogle();
        Task<Result<string>> SignInWithGitHub();
        Task<Result<string>> SignInWith(string email, string option);
        Task<Result<string>> RefreshToken(string token, string refreshToken);
    }
}
