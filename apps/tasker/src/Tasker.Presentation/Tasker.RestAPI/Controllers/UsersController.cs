using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Models;
using Mzstruct.Common.Extensions;
using Tasker.Application.Contracts.ICommands;
using Tasker.Application.Contracts.IQueries;

namespace Tasker.RestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Route("api/[controller]/[action]")]
    public class UsersController(//IConfiguration configuration,
        //IHttpContextAccessor httpContextAccessor,
        //ILogger<UsersController> logger,
        IUserQuery userQuery, 
        IUserCommand userCommand) : ControllerBase
    {
        [HttpGet, Authorize]
        //[ActionName("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries)
        {
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
        public async Task<IActionResult> CreateUser(BaseUserModel user)
        {
            var result = await userCommand.CreateUser(user);
            return result.ToActionResult(this);
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> UpdateUser(BaseUser user)
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
