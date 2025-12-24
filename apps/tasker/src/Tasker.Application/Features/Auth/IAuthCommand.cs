using Mzstruct.Base.Dtos;
using Mzstruct.Base.Errors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Features.Users;

namespace Tasker.Application.Features.Auth
{
    public interface IAuthCommand
    {
        Task<Result<UserModel>> SignUp(SignUpDto user);
        Task<Result<string>> SignIn(SignInDto user);
        Task<Result<string>> SignInWithGoogle(string credential);
        Task<Result<string>> RefreshToken(string userId, string refreshToken);
    }
}
