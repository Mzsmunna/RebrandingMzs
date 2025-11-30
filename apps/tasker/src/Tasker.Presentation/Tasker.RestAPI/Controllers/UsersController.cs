using Google.Apis.Auth;
using Kernel.Drivers.Models;
using Kernel.Processes.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using Tasker.Application.Interfaces;
using Tasker.Domain.Entities;
using Tasker.Domain.Models;

namespace Tasker.RestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Route("api/[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UsersController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersController(IConfiguration configuration, ILogger<UsersController> logger, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _logger = logger;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet, Authorize]
        //[ActionName("GetAllUsers")]
        public IActionResult AllUsers(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries)
        {
            List<SearchField>? queries = ProcessHelper.JsonListDeserialize<SearchField>(searchQueries);
            var users = _userRepository.GetAllUsers(currentPage, pageSize, sortField, sortDirection, queries).Result;
            return Ok(users);
        }

        [HttpGet("{id}"), Authorize]
        //[ActionName("GetUser")]
        public IActionResult GetUser(string id)
        {
            var user = _userRepository.GetUser(id).Result;
            //users.Password = "?";
            return Ok(user);
        }

        [HttpPost, Authorize]
        //[ActionName("SaveUser")]
        public IActionResult CreateUser(User user)
        {
            return SaveUser(user);
        }

        [HttpPut, Authorize]
        //[ActionName("UpdateUser")]
        public IActionResult UpdateUser(User user)
        {
            return SaveUser(user);
        }

        [HttpDelete, Authorize]
        //[ActionName("DeleteUser")]
        public IActionResult DeleteUser(string id)
        {
            var isDeleted = _userRepository.DeleteById(id);
            return Ok(isDeleted);
        }

        [HttpGet("AvailableToAssign"), Authorize]
        //[ActionName("AvailableToAssign")]
        public IActionResult AvailableUsersToAssign()
        {
            var users = _userRepository.GetAllUserToAssign();
            return Ok(users);
        }

        [HttpGet("Count"), Authorize]
        //[ActionName("Count")]
        public IActionResult UsersCount(string searchQueries)
        {
            List<SearchField>? queries = ProcessHelper.JsonListDeserialize<SearchField>(searchQueries);
            var users = _userRepository.GetAllUserCount(queries).Result;
            return Ok(users);
        }

        // ---
        private IActionResult SaveUser(User user)
        {
            var response = _userRepository.GetAllByField("Email", user.Email.ToLower()).Result;
            var existingUser = response.Map(
                Ok: users => users.Where(x => !x.Id.Equals(user.Id)).FirstOrDefault(),
                Err: _ => null
            );
            if (existingUser != null)
            {
                //return StatusCode(StatusCodes.Status409Conflict, "This email already exist");               
                var problemDetails = Results.Problem(
                    instance: _httpContextAccessor.HttpContext?.Request.Path,
                    type: "https://www.rfc-editor.org/rfc/rfc9110#name-409-conflict",
                    title: "Conflict",
                    detail:  "This email already exist",
                    statusCode: StatusCodes.Status409Conflict,
                    extensions: new Dictionary<string, object?>
                    {
                        { "errors", response.Error }
                    }
                );
                return Ok(problemDetails);
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
            }
            else
            {
                var result = _userRepository.Save(user).Result;
                return Ok(result);
            }  
        }
    }
}
