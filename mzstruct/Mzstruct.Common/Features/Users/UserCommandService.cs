using Mzstruct.Base.Dtos;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Errors;
using Mzstruct.Base.Extensions;
using Mzstruct.Base.Mappings;
using Mzstruct.Base.Models;
using Mzstruct.Common.Contracts.ICommands;
using Mzstruct.Common.Validators;
using Mzstruct.DB.Providers.MongoDB.Contracts.IRepos;

namespace Mzstruct.Common.Features.Users
{
    internal class UserCommandService(//ILogger<UserQuery> logger,
        //IHttpContextAccessor httpContextAccessor,
        IBaseUserRepository userRepository) : IUserCommandService
    {
        public async Task<Result<BaseUserModel?>> CreateUser(BaseUserModel user)
        {
            var validation = await UserValidator.Validate(user);
            if (validation.IsValid is false)
                return Error.Validation("IssueCommand.CreateUser.InvalidInputs",
                    "One or more User input invalid", validation.ToErrorDictionary());
            var userEntity = user.ToEntity<BaseUser, BaseUserModel>();
            return await SaveUser(userEntity);
        }

        public async Task<Result<BaseUserModel?>> UpdateUser(BaseUserModel user)
        {
            var userEntity = user.ToEntity<BaseUser, BaseUserModel>();
            return await SaveUser(userEntity);
        }

        public async Task<Result<bool>> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return ClientError.BadRequest;
            var result = await userRepository.DeleteById(id);
            return result != null ? true : false;
        }

        private async Task<Result<BaseUserModel?>> SaveUser(BaseUser user)
        {
            if (user is null) return ClientError.BadRequest;
            var validation = await UserValidator.Validate(user);
            if (validation.IsValid is false)
                return Error.Validation("UserCommand.UpdateUser.InvalidState", 
                    "Updated User info seems in invalid state", validation.ToErrorDictionary());

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
            var result = await userRepository.Save(user);
            return result?.ToModel<BaseUserModel, BaseUser>() ;
        }
    }
}
