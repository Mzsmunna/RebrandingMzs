using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mzstruct.Auth.Models.Dtos;
using Mzstruct.Base.Extensions;
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
        [ActionName("SignInWithGoogle")]
        public async Task<IActionResult> SignInWithGoogle([FromBody] string credential)
        {
            var result = await authCommand.SignInWithGoogle(credential);
            return result.ToActionResult(this);
        }

        [HttpGet]
        [ActionName("ChallengeGoogleSignIn")]
        public async Task<IActionResult> ChallengeGoogleSignIn()
        {
            var redirectUrl = Url.Action(nameof(ConfirmGoogleSignIn), "Auth")!;
            var props = new AuthenticationProperties
            {
                RedirectUri = redirectUrl // OAuth.CallbackPath != RedirectUri
            };
            // Tell ASP.NET Core to start Google OAuth flow
            return Challenge(props, "Google");
        }

        [HttpGet]
        [ActionName("ConfirmGoogleSignIn")] // Callback
        public async Task<IActionResult> ConfirmGoogleSignIn()
        {
            var result = await authCommand.SignInWithGoogle();
            //return result.ToActionResult(this);
            return Redirect($"http://localhost:4200/auth/login?token={result.Data}");
        }

        [HttpGet]
        [ActionName("ChallengeGitHubSignIn")]
        public async Task<IActionResult> ChallengeGitHubSignIn()
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = "/api/auth/ConfirmGitHubSignIn" // OAuth.CallbackPath != RedirectUri
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
        public async Task<IActionResult> RefreshToken()
        {
            var token = Request.Headers.Authorization
                .FirstOrDefault()?
                .Replace("Bearer ", "") ?? "";
            var refreshToken = Request.Cookies["refreshToken"] ?? "";
            var result = await authCommand.RefreshToken(token, refreshToken);
            return result.ToActionResult(this);
        }
    }
}
