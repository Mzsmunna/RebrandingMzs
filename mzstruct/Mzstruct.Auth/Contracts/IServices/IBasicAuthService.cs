using Mzstruct.Auth.Models.Dtos;
using Mzstruct.Base.Dtos;
using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Auth.Contracts.IServices
{
    public interface IBasicAuthService
    {
        Task<Result<string>> SignUp(SignUpDto user);
        Task<Result<string>> SignIn(SignInDto user);
        Task<Result<string>> SignInWith(string email, string option = "Mail");
        Task<Result<bool>> SignOut(string token = "");
        Task<Result<string>> RefreshToken(string token = "", string refreshToken = "");
    }
}
