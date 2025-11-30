using Kernel.Drivers.Dtos;
using Kernel.Drivers.Errors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Domain.Dtos;
using Tasker.Domain.Models;

namespace Tasker.Application.Interfaces
{
    public interface IAuthCommand
    {
        Task<Result<UserModel>> SignUp(SignUpDto user);
        Task<Result<string>> SignIn(SignInDto user);
        Task<Result<string>> SignInWithGoogle(string credential);
        Task<Result<string>> RefreshToken(string userId, string refreshToken);
    }
}
