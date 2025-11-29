using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Domain.Dtos;

public record SignInDto(string email, string password);
