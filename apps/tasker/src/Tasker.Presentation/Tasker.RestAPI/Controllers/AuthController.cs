using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mzstruct.Common.Extensions;
using Mzstruct.Common.Features.Auth;
using System.Security.Claims;
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

        [HttpPost]
        [ActionName("LoginWithGitHub")]
        public async Task<IActionResult> LoginWithGitHub()
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = "/api/auth/RequestGitHubSignIn"
            };
            return Challenge(props, "GitHub");
        }

        [HttpGet]
        [ActionName("RequestGitHubSignIn")]
        public async Task<IActionResult> GitHubCallback()
        {
            var authenticateResult =
                await HttpContext.AuthenticateAsync("GitHub");

            if (!authenticateResult.Succeeded)
                return Unauthorized();

            var claims = authenticateResult.Principal!.Claims;

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var githubId = claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            // 🔐 Create / find user in DB
            //var user = await FindOrCreateUser(githubId, email, name);

            var jwt = string.Empty;
            // 🔑 Issue JWT
            //var jwt = GenerateJwt(user);

            // redirect back to Angular
            return Redirect($"http://localhost:4200/auth/callback?token={jwt}");
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
