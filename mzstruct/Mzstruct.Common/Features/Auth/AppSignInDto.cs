using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mzstruct.Common.Features.Auth;

public record AppSignInDto([Required, EmailAddress] string Email, [Required, MinLength(3)] string Password);
