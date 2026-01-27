using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mzstruct.Auth.Models.Dtos
{
    public record SignInDto(
        string Username,
        [Required, EmailAddress] string Email, 
        [Required, MinLength(3)] string Password
    );
}
