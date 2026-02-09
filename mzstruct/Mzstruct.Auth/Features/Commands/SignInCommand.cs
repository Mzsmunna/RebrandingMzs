using Mzstruct.Auth.Contracts.IServices;
using Mzstruct.Auth.Services;
using Mzstruct.Base.Patterns.CQRS;
using Mzstruct.Base.Patterns.Result;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mzstruct.Auth.Features.Commands
{
    public record SignInCommand (
        string Username,
        [Required, EmailAddress] string Email, 
        [Required, MinLength(3)] string Password
    ) : ICommand<Result<string>>;

    public class SignInCommandHandler(
        //ILogger <SignInCommandHandler> logger,
        IAuthService authService)
        : ICommandHandler<SignInCommand, Result<string>>
    {
        public async Task<Result<string>> 
            HandleAsync(SignInCommand command, CancellationToken token = default)
            => await authService.SignIn(command);
    }
}
