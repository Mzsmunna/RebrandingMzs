using Mzstruct.Auth.Contracts.IServices;
using Mzstruct.Base.Contracts.ICommands;
using Mzstruct.Base.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mzstruct.Auth.Features.Commands
{
    public record SignUpCommand(
        [Required] string Name,
        string FirstName,
        string LastName,
        string Gender,
        DateTime? DOB,
        //string Role,
        string Username,
        [Required] string Email,
        [Required] string Password,
        [Required] string ConfirmPassword,
        string Phone,
        string Address,
        string Department,
        string Designation,
        string Position,
        string Img
    ) : ICommand<Result<string>>;

    public class SignUpCommandHandler(
        //ILogger <SignUpCommandHandler> logger,
        IAuthService authService)
        : ICommandHandler<SignUpCommand, Result<string>>
    {
        public async Task<Result<string>> 
            HandleAsync(SignUpCommand command, CancellationToken token = default) 
            => await authService.SignUp(command);
    }
}
