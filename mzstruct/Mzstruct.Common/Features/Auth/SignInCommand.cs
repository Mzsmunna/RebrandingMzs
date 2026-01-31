using Microsoft.Extensions.Logging;
using Mzstruct.Auth.Contracts.IServices;
using Mzstruct.Auth.Models.Dtos;
using Mzstruct.Base.Contracts.ICommands;
using Mzstruct.Base.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mzstruct.Common.Features.Auth
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
        public async Task<Result<string>> HandleAsync(SignInCommand command, CancellationToken token = default)
        {
            var dto = new SignInDto
            (
                command.Username,
                command.Email,
                command.Password
            );
            return await authService.SignIn(dto);
        }
    }
}
