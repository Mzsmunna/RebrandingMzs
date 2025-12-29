using FluentValidation;
using Mzstruct.Base.Dtos;
using Mzstruct.Base.Errors;
using Mzstruct.Auth;
using Mzstruct.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Validators;
using Tasker.Application.Features.Users;
using Mzstruct.Common.Mappings;
using Tasker.Application.Contracts.IRepos;
using Tasker.Application.Contracts.ICommands;
using Mzstruct.Auth.Contracts.IManagers;

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
            if (validation.IsValid is false)
                return Error.Validation("AuthCommand.SignUp.InvalidInput",
                    "SignUp form is invalid", validation.ToErrorDictionary());
            
            var user = signUpDto.ToEntity<User, SignUpDto>();
            if (user is null)
            {
                logger.LogWarning("SignUp: Bad Request");
                return Error.Bad("AuthCommand.SignUp.BadRequest", "Requested body payload seems invalid");
            }
            
            var registered = await userRepository.RegisterUser(user);
            if (registered is null)
                return Error.Conflict("User.Exists", "This email already exists.");

            return registered.ToModel<UserModel, User>();
        }

        public async Task<Result<string>> SignIn(SignInDto signInDto)
        {
            if (signInDto is null)
            {
                logger.LogWarning("SignIn: Bad Request");
                return ClientError.BadRequest;
            }

            var validation = await TaskerValidator.ValidateSignIn(signInDto);
            if (validation.IsValid is false)
                return Error.Validation("AuthCommand.SignIn.InvalidForm",
                    "SignIn form is invalid", validation.ToErrorDictionary());
            
            var signInUser = await userRepository.LoginUser(signInDto.Email, signInDto.Password);
            if (signInUser is null)
                return Error.NotFound("SignIn.Credential.NotFound", "User credential didn't match"); //StatusCode(StatusCodes.Status204NoContent, "User doesn't exist.");
            
            jwtTokenManager.CreatePasswordHash(signInDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            signInUser.PasswordHash = passwordHash;
            signInUser.PasswordSalt = passwordSalt;
            
            if (!jwtTokenManager.VerifyPasswordHash(signInUser.Password, signInUser.PasswordHash, signInUser.PasswordSalt))
                return Error.Validation("SignIn.Credential.Wrong", "Wrong credential.");
            
            string token = jwtTokenManager.CreateToken(signInUser);
            var refreshToken = jwtTokenManager.GenerateRefreshToken();
            jwtTokenManager.SetRefreshToken(refreshToken, signInUser);
            
            return token;
        }

        public async Task<Result<string>> SignInWithGoogle(string credential)
        {
            var payload = await googleAuthManager.ValidateToken(credential);
            if (payload is null)
            {
                logger.LogWarning("SignInWithGoogle: Bad Request");
                return ClientError.BadRequest;
            }

            var signInUser = await userRepository.LoginUser(payload.Email);
            if (signInUser is null)
                return Error.NotFound("SignIn.Google.NotLinkned", "User doesn't exist."); //StatusCode(StatusCodes.Status204NoContent, "User doesn't exist.");

            jwtTokenManager.CreatePasswordHash(signInUser.Password, out byte[] passwordHash, out byte[] passwordSalt);
            signInUser.PasswordHash = passwordHash;
            signInUser.PasswordSalt = passwordSalt;

            if (!jwtTokenManager.VerifyPasswordHash(signInUser.Password, signInUser.PasswordHash, signInUser.PasswordSalt))
                return Error.Validation("SignIn.Google.Error", "Wrong credential."); //StatusCode(StatusCodes.Status403Forbidden, "Wrong credential.");

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

            var signInUser = await userRepository.GetById(userId);           
            if (signInUser is null)
                return Error.NotFound("Token.Refresh.NotFound", "User token unavailable");
            
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
