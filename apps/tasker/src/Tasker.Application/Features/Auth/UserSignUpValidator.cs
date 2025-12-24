using FluentValidation;
using Mzstruct.Base.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Application.Features.Auth
{
    internal sealed class UserSignUpValidator: AbstractValidator<SignUpDto>
    {
        public UserSignUpValidator()
        {
            RuleFor(x => x.firstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");
            RuleFor(x => x.lastName)
                .MaximumLength(100)
                .WithMessage("First name cannot exceed 100 characters.");
            RuleFor(x => x.dob)
                .Must(x => x.HasValue && BaseHelper.IsFutureDate(x.Value))
                .WithMessage("birth date cannot be from future");
            RuleFor(x => x.email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
            RuleFor(x => x.confirmPassword)
                .NotEmpty().WithMessage("Confirm Password can't be empty.")
                .Equal(x => x.password).WithMessage("Password didn't match.");
            RuleFor(x => x.phone)
                .MinimumLength(10).WithMessage("Phone must be at least 10 digit long.");
            RuleFor(x => x)
                .NotNull().WithMessage("Bad Request").SetValidator(this);
        }
    }
}
