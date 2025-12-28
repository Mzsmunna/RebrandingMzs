using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzstruct.Base.Dtos;
using Mzstruct.Base.Errors;
using Mzstruct.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Contracts.ICommands;
using Tasker.Application.Contracts.IRepos;
using Tasker.Application.Features.Issues;
using Tasker.Application.Validators;

namespace Tasker.Application.Features.Users
{
    internal class UserCommand(//ILogger<UserQuery> logger,
        //IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository) : IUserCommand
    {
        public async Task<Result<User?>> CreateUser(UserModel user)
        {
            var validation = await TaskerValidator.ValidateUser(user);
            if (validation.IsValid is false)
                return Error.Validation("IssueCommand.CreateUser.InvalidInput", "User input invalid");
            var userEntity = user.ToEntity<User, UserModel>();
            return await SaveUser(userEntity);
        }

        public async Task<Result<User?>> UpdateUser(User user)
        {
            return await SaveUser(user);
        }

        public async Task<Result<bool>> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return ClientError.BadRequest;
            var result = await userRepository.DeleteById(id);
            return result != null ? true : false;
        }

        private async Task<Result<User?>> SaveUser(User user)
        {
            var validation = await TaskerValidator.ValidateUser(user);
            if (validation.IsValid is false)
                return Error.Validation("UserCommand.UpdateUser.InvalidState", "Updated User info seems in invalid state");

            var usersWithSameEmail = await userRepository.GetByFieldValue("Email", user.Email.ToLower());
            var existingUser = usersWithSameEmail?.Where(x => !x.Id.Equals(user.Id)).FirstOrDefault();
            if (string.IsNullOrEmpty(user.Id) && existingUser != null)
            {
                return Error.Conflict("UserCommand.Create.EmailExist", "This email already exist");
                //return StatusCode(StatusCodes.Status409Conflict, "This email already exist");
                
                //var problemDetails = Results.Problem(
                //    instance: httpContextAccessor.HttpContext?.Request.Path,
                //    type: "https://www.rfc-editor.org/rfc/rfc9110#name-409-conflict",
                //    title: "Conflict",
                //    detail:  "This email already exist",
                //    statusCode: StatusCodes.Status409Conflict,
                //    extensions: new Dictionary<string, object?>
                //    {
                //        { "errors", response.Error }
                //    }
                //);
                
                
                //var problemDetails = new ProblemDetails
                //{
                //    Instance = _httpContextAccessor.HttpContext?.Request.Path,
                //    Type = "https://www.rfc-editor.org/rfc/rfc9110#name-409-conflict",
                //    Status = StatusCodes.Status409Conflict,
                //    Title = "Conflict",
                //    Detail = "This email already exist",
                //    Extensions = new Dictionary<string, object?>
                //    {
                //        { "errors", response.Error }
                //    }
                //};

                //return problemDetails;
            }
            return await userRepository.Save(user); 
        }
    }
}
