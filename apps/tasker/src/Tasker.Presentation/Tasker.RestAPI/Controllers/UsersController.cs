using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Mzstruct.Base.Consts;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Errors;
using Mzstruct.Base.Models;
using Mzstruct.Common.Extensions;
using Tasker.Application.Consts;
using Tasker.Application.Contracts.ICommands;
using Tasker.Application.Contracts.IQueries;
using Tasker.Application.Features.Users;

namespace Tasker.RestAPI.Controllers
{
    [ApiController]
    //[ApiVersion("1", Deprecated = true)]
    //[ApiVersion("2")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    public class UsersController(//IConfiguration configuration,
        //IHttpContextAccessor httpContextAccessor,
        //ILogger<UsersController> logger,
        //IFeatureManager featureManager,
        IUserQuery userQuery, 
        IUserCommand userCommand) : ControllerBase
    {
        //[HttpGet, Authorize]
        ////[MapToApiVersion("1")]
        //public async Task<IActionResult> GetAllUsersV1(string sortField, string sortDirection)
        //{
        //    //if (await featureManager.IsEnabledAsync(ApiConst.UseUserApiV1))
        //    //    return Error.NotFound("FeatureDisabled", 
        //    //        "The requested api version is disabled.").ToProblem(this);
        //    var result = await userQuery.GetAllUsers(sortField, sortDirection);
        //    return result.ToActionResult(this);
        //}

        [HttpGet, Authorize]
        //[MapToApiVersion("2")]
        public async Task<IActionResult> GetAllUsers(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries)
        {
            //if (await featureManager.IsEnabledAsync(ApiConst.UseUserApiV2))
            //    return Error.NotFound("FeatureDisabled", 
            //        "The requested api version is disabled.").ToProblem(this);
            var result = await userQuery.GetAllUsers(currentPage, pageSize, sortField, sortDirection, searchQueries);
            return result.ToActionResult(this);
        }

        [HttpGet("{id}"), Authorize]
        public async Task<IActionResult> GetUser(string id)
        {
            var result = await userQuery.GetUser(id);
            return result.ToActionResult(this);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> CreateUser(UserModel user)
        {
            var result = await userCommand.CreateUser(user);
            return result.ToActionResult(this);
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> UpdateUser(UserModel user)
        {
            var result = await userCommand.UpdateUser(user);
            return result.ToActionResult(this);
        }

        [HttpDelete, Authorize]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await userCommand.DeleteUser(id);
            return result.ToActionResult(this);
        }

        [HttpGet("AvailableToAssign"), Authorize]
        public async Task<IActionResult> AvailableUsersToAssign()
        {
            var result = await userQuery.AvailableUsersToAssign();
            return result.ToActionResult(this);
        }

        [HttpGet("Count"), Authorize]
        public async Task<IActionResult> UsersCount(string searchQueries)
        {
            var result = await userQuery.UsersCount(searchQueries);
            return result.ToActionResult(this);
        }
    }
}
