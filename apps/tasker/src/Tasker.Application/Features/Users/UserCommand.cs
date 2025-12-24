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

namespace Tasker.Application.Features.Users
{
    internal class UserCommand(//ILogger<UserQuery> logger,
        //IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository) : IUserCommand
    {
        public async Task<Result<User>> CreateUser(UserModel user)
        {
            var userEntity = user.ToEntity<User, UserModel>();
            return await SaveUser(userEntity);
        }

        public async Task<Result<User>> UpdateUser(User user)
        {
            return await SaveUser(user);
        }

        public async Task<Result<bool>> DeleteUser(string id)
        {
            return await userRepository.DeleteById(id);
        }

        private async Task<Result<User>> SaveUser(User user)
        {
            var usersByMail = await userRepository.GetAllByField("Email", user.Email.ToLower());
            var existingUser = usersByMail.Map(
                Ok: users => users.Where(x => !x.Id.Equals(user.Id)).FirstOrDefault(),
                Err: _ => null
            );
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
            else
            {
                return await userRepository.Save(user);
            }  
        }
    }
}
