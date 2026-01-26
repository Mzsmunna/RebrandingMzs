using Mzstruct.Auth.Models.Dtos;
using Mzstruct.Base.Dtos;
using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Auth.Contracts.IServices
{
    public interface IAuthService
    {
        Task<Result<BaseUserModel?>> SignUp(SignUpDto user);
        Task<Result<string>> SignIn(SignInDto user);
        Task<Result<string>> SignInWithGoogle(string credential);
        Task<Result<string>> SignInWithGoogle();
        Task<Result<string>> SignInWithGitHub();
        Task<Result<string>> SignInWith(string email, string option = "Mail");
        Task<Result<string>> RefreshToken(string userId, string token, string refreshToken);
    }
}
