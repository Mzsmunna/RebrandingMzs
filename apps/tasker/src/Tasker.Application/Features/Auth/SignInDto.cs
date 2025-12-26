using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tasker.Application.Features.Auth;

public record SignInDto([Required] string Email, [Required] string Password);
