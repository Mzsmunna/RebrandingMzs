using Mzstruct.Auth.Features.Commands;
using Mzstruct.Base.Dtos;
using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Auth.Contracts.IServices
{
    public interface IBasicAuthService
    {
        Task<Result<string>> SignUp(SignUpCommand payload);
        Task<Result<string>> SignIn(SignInCommand payload);
        Task<Result<string>> SignInWith(string email, string option = "Mail");
        Task<Result<bool>> SignOut(string token = "");
        Task<Result<string>> RefreshToken(string token = "", string refreshToken = "");
    }
}
