using Microsoft.AspNetCore.Mvc;
using Mzstruct.Common.Extensions;
using Mzstruct.Common.Features.Auth;
using Tasker.Application.Contracts.ICommands;
using Tasker.Application.Features.Auth;

namespace Tasker.RestAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController(IAuthCommand authCommand) : ControllerBase
    {
        [HttpPost]
        [ActionName("Register")]
        public async Task<IActionResult> Register(SignUpDto dto)
        {
            var result = await authCommand.SignUp(dto);
            return result.ToActionResult(this);
        }

        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> Login(SignInDto dto)
        {
            var result = await authCommand.SignIn(dto);
            return result.ToActionResult(this);
        }

        [HttpPost]
        [ActionName("LoginWithGoogle")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] string credential)
        {
            var result = await authCommand.SignInWithGoogle(credential);
            return result.ToActionResult(this);
        }

        [HttpPost]
        [ActionName("RefreshToken")]
        public async Task<IActionResult> RefreshToken(string userId)
        {
            var refreshToken = Request.Cookies["refreshToken"] ?? "";
            var result = await authCommand.RefreshToken(userId, refreshToken);
            return result.ToActionResult(this);
        }
    }
}
