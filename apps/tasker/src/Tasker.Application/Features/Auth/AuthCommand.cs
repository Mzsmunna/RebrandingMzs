using FluentValidation;
using Mzstruct.Base.Dtos;
using Mzstruct.Base.Errors;
using Mzstruct.Base.Interfaces.Auth;
using Mzstruct.Auth;
using Mzstruct.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Extensions;
using Tasker.Application.Interfaces;
using Tasker.Application.Validators;
using Tasker.Domain.Dtos;
using Tasker.Domain.Entities;
using Tasker.Domain.Models;

namespace Tasker.Application.Features.Auth
{
    internal class AuthCommand(ILogger<AuthCommand> logger,
        //IValidator<SignUpDto> signUpValidator,
        IUserRepository userRepository, 
        IJwtTokenManager jwtTokenManager, 
        IGoogleAuthManager googleAuthManager) : IAuthCommand
    {
        public async Task<Result<UserModel>> SignUp(SignUpDto signUpDto)
        {
            //var validation = signUpValidator.Validate(signUpDto);
            var validation = await TaskerValidator.ValidateSignUp(signUpDto);
            if (!validation.IsValid)
                return Error.Validation("AuthCommand.SignUp.InvalidInput", "User input invalid");
            var user = signUpDto.ToEntity<User, SignUpDto>();
            
            if (user is null)
            {
                logger.LogWarning("SignUp: Bad Request");
                return Error.Bad("AuthCommand.SignUp.BadRequest", "Requested body payload seems invalid");
            }
            
            var result = await userRepository.RegisterUser(user);
            
            return result.Map(
                Ok: data => data.ToModel<UserModel, User>(),
                Err: error => default! //error //Error.None
            );
        }

        public async Task<Result<string>> SignIn(SignInDto user)
        {
            if (user == null)
            {
                logger.LogWarning("SignIn: Bad Request");
                return ClientError.BadRequest;
            }
            
            var result = await userRepository.LoginUser(user.email, user.password);

            if (!result.IsSuccess || result.Data is null)
                return Error.NotFound("Login.Credential.NotFound", "User doesn't exist."); //StatusCode(StatusCodes.Status204NoContent, "User doesn't exist.");
            
            var signInUser = result.Data;
            jwtTokenManager.CreatePasswordHash(user.password, out byte[] passwordHash, out byte[] passwordSalt);
            signInUser.PasswordHash = passwordHash;
            signInUser.PasswordSalt = passwordSalt;
            
            if (!jwtTokenManager.VerifyPasswordHash(signInUser.Password, signInUser.PasswordHash, signInUser.PasswordSalt))
                return Error.Validation("Login.Credential.Wrong", "Wrong credential.");
            
            string token = jwtTokenManager.CreateToken(signInUser);
            var refreshToken = jwtTokenManager.GenerateRefreshToken();
            jwtTokenManager.SetRefreshToken(refreshToken, signInUser);
            
            return token;
        }

        public async Task<Result<string>> SignInWithGoogle(string credential)
        {
            var payload = await googleAuthManager.ValidateToken(credential);

            if (payload == null)
            {
                logger.LogWarning("SignInWithGoogle: Bad Request");
                return ClientError.BadRequest;
            }

            var result = await userRepository.LoginUser(payload.Email);

            if (!result.IsSuccess || result.Data is null)
                Error.NotFound("Login.Google.NotLinkned", "User doesn't exist."); //StatusCode(StatusCodes.Status204NoContent, "User doesn't exist.");

            var signInUser = result.Data!;
            jwtTokenManager.CreatePasswordHash(signInUser.Password, out byte[] passwordHash, out byte[] passwordSalt);
            signInUser.PasswordHash = passwordHash;
            signInUser.PasswordSalt = passwordSalt;

            if (!jwtTokenManager.VerifyPasswordHash(signInUser.Password, signInUser.PasswordHash, signInUser.PasswordSalt))
                return Error.Validation("Login.Google.Error", "Wrong credential."); //StatusCode(StatusCodes.Status403Forbidden, "Wrong credential.");

            string token = jwtTokenManager.CreateToken(signInUser);
            var refreshToken = jwtTokenManager.GenerateRefreshToken();
            jwtTokenManager.SetRefreshToken(refreshToken, signInUser);
            return token;
        }

        public async Task<Result<string>> RefreshToken(string userId, string refreshToken)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(refreshToken))
            {
                logger.LogWarning("RefreshToken: Bad Request");
                return ClientError.BadRequest;
            }

            var result = await userRepository.GetUser(userId);
            
            if (!result.IsSuccess || result.Data is null)
                return Error.NotFound("Token.Refresh.NotFound", "User token unavailable");
            
            var signInUser = result.Data;
            if (!signInUser.RefreshToken.Equals(refreshToken))
                return Error.Unauthorized("Token.Refresh.Invalid", "Invalid Refresh Token."); //Unauthorized("Invalid Refresh Token.");
            else if (signInUser.TokenExpires < DateTime.UtcNow)
                return Error.Unauthorized("Token.Refresh.Expired", "Token expired."); //Unauthorized("Token expired.");

            string token = jwtTokenManager.CreateToken(signInUser);
            var newRefreshToken = jwtTokenManager.GenerateRefreshToken();
            jwtTokenManager.SetRefreshToken(newRefreshToken, signInUser);
            return token;
        }
    }
}
