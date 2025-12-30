using Mzstruct.Base.Dtos;
using Mzstruct.Base.Models;
using Mzstruct.Common.Features.Auth;

namespace Mzstruct.Common.Contracts.ICommands
{
    public interface IAuthCommand
    {
        Task<Result<BaseUserModel?>> SignUp(SignUpDto user);
        Task<Result<string>> SignIn(SignInDto user);
        Task<Result<string>> SignInWithGoogle(string credential);
        Task<Result<string>> RefreshToken(string userId, string refreshToken);
    }
}
