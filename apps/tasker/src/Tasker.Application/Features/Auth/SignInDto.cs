using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Application.Features.Auth;

public record SignInDto(string Email, string Password);
