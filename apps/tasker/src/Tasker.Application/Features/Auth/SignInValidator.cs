using FluentValidation;
using Mzstruct.Base.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Application.Features.Auth
{
    internal sealed class SignInValidator: AbstractValidator<SignInDto>
    {
        public SignInValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(3).WithMessage("Password must be at least 3 characters long.");
        }
    }
}
