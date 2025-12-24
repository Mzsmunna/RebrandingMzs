using FluentValidation;
using FluentValidation.Results;
using Mzstruct.Base.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Extensions;
using Tasker.Application.Features.Auth;

namespace Tasker.Application.Validators
{
    internal static class TaskerValidator
    {
        public static async Task<ValidationResult> ValidateSignUp(SignUpDto dto)
        {
            return await dto.ValidateInlineAsync(v =>
            {
                v.RuleFor(x => x.firstName)
                    .NotEmpty().WithMessage("First name is required.")
                    .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");
                v.RuleFor(x => x.lastName)
                    .MaximumLength(100)
                    .WithMessage("First name cannot exceed 100 characters.");
                v.RuleFor(x => x.dob)
                    .Must(x => x.HasValue && BaseHelper.IsFutureDate(x.Value))
                    .WithMessage("birth date cannot be from future");
                v.RuleFor(x => x.email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email format.");
                v.RuleFor(x => x.password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
                v.RuleFor(x => x.confirmPassword)
                    .NotEmpty().WithMessage("Confirm Password can't be empty.")
                    .Equal(x => x.password).WithMessage("Password didn't match.");
                v.RuleFor(x => x.phone)
                    .MinimumLength(10).WithMessage("Phone must be at least 10 digit long.");
                v.RuleFor(x => x)
                    .NotNull().WithMessage("Bad Request").SetValidator(v);
            });
        }
    }
}
