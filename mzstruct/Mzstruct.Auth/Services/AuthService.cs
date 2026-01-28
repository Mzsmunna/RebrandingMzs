using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Mzstruct.Auth.Contracts.IManagers;
using Mzstruct.Auth.Contracts.IServices;
using Mzstruct.Auth.Models.Dtos;
using Mzstruct.Base.Dtos;
using Mzstruct.Base.Errors;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mzstruct.Auth.Services
{
    public class AuthService(
        IBasicAuthService basicAuthService,
        IOAuthService oAuthService) : IAuthService
    {
        public async Task<Result<string>> SignUp(SignUpDto signUpDto) => await basicAuthService.SignUp(signUpDto);
        public async Task<Result<string>> SignIn(SignInDto signInDto) => await basicAuthService.SignIn(signInDto);
        public async Task<Result<string>> SignInWith(string email, string option = "Mail") => await basicAuthService.SignInWith(email, option);
        public async Task<Result<bool>> SignOut(string token = "") => await basicAuthService.SignOut(token);
        public async Task<Result<string>> RefreshToken(string token = "", string refreshToken = "") => await basicAuthService.RefreshToken(token, refreshToken);
        public async Task<Result<string>> SignInWithGoogle(string credential) => await oAuthService.SignInWithGoogle(credential);
        public async Task<Result<string>> SignInWithGoogle() => await oAuthService.SignInWithGoogle();
        public async Task<Result<string>> SignInWithGitHub() => await oAuthService.SignInWithGitHub();
    }
}
