using Mzstruct.Base.Dtos;
using Mzstruct.Base.Models;
using Mzstruct.Common.Features.Auth;

namespace Mzstruct.Common.Contracts.ICommands
{
    public interface IAppAuthCommand
    {
        Task<Result<AppUserModel>> SignUp(AppSignUpDto user);
        Task<Result<string>> SignIn(AppSignInDto user);
        Task<Result<string>> SignInWithGoogle(string credential);
        Task<Result<string>> RefreshToken(string userId, string refreshToken);
    }
}
