using Mzstruct.Auth.Features.Commands;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Models;
using Mzstruct.Base.Patterns.Result;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Auth.Contracts.IServices
{
    public interface IBasicAuthService
    {
        Task<Result<string>> SignUp(SignUpCommand request);
        Task<Result<string>> SignIn(SignInCommand request);
        Task<Result<string>> SignInWith(string email, string option = "Mail");
        Task<Result<bool>> SignOut(string token = "");
        Task<Result<string>> RefreshToken(string token = "", string refreshToken = "");
    }
}
