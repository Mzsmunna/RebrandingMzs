using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mzstruct.Common.Extensions;
using Mzstruct.Common.Features.Auth;
using Tasker.Application.Contracts.ICommands;

namespace Tasker.RestAPI.Controllers
{
    [AllowAnonymous]
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

        [HttpGet]
        [ActionName("LoginWithGitHub")]
        public async Task<IActionResult> LoginWithGitHub()
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = "/api/auth/ConfirmGitHubSignIn"
            };
            return Challenge(props, "GitHub");
        }

        [HttpGet]
        [ActionName("ConfirmGitHubSignIn")] // Callback
        public async Task<IActionResult> ConfirmGitHubSignIn()
        {
            var result = await authCommand.SignInWithGitHub();
            //if (string.IsNullOrEmpty(result.Data)) return Unauthorized();
            //return result.ToActionResult(this);
            return Redirect($"http://localhost:4200/auth/login?token={result.Data}");
        }

        [HttpPost]
        [ActionName("RefreshToken")]
        public async Task<IActionResult> RefreshToken(string userId)
        {
            var token = Request.Headers.Authorization
                .FirstOrDefault()?
                .Replace("Bearer ", "") ?? "";
            var refreshToken = Request.Cookies["refreshToken"] ?? "";
            var result = await authCommand.RefreshToken(userId, token, refreshToken);
            return result.ToActionResult(this);
        }
    }
}
