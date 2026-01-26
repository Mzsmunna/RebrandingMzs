using FluentValidation;
using FluentValidation.Results;
using Mzstruct.Auth.Models.Dtos;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Extensions;
using Mzstruct.Base.Helpers;
using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Auth.Validators
{
    internal sealed class SignUpValidator: AbstractValidator<SignUpDto>
    {
        public static async Task<ValidationResult> ValidateSignUp(SignUpDto dto)
        {
            return await dto.FluentInlineValidate(v =>
            {
                v.RuleFor(x => x.FirstName)
                    .NotEmpty().WithMessage("First name is required.")
                    .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");
                v.RuleFor(x => x.LastName)
                    .MaximumLength(100)
                    .WithMessage("First name cannot exceed 100 characters.");
                v.RuleFor(x => x.DOB)
                    .Must(x => x.HasValue && BaseHelper.IsFutureDate(x.Value))
                    .WithMessage("birth date cannot be from future");
                v.RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email format.");
                v.RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(3).WithMessage("Password must be at least 3 characters long.");
                v.RuleFor(x => x.ConfirmPassword)
                    .NotEmpty().WithMessage("Confirm Password can't be empty.")
                    .Equal(x => x.Password).WithMessage("Password didn't match.");
                v.RuleFor(x => x.Phone)
                    .MinimumLength(10).WithMessage("Phone must be at least 10 digit long.");
                v.RuleFor(x => x)
                    .NotNull().WithMessage("Bad Request").SetValidator(v);
            });
        }

        public static async Task<ValidationResult> ValidateSignIn(SignInDto dto)
        {
            return await dto.FluentInlineValidate(v =>
            {
                v.RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email format.");
                v.RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(3).WithMessage("Password must be at least 3 characters long.");
            });
        }

        public static async Task<ValidationResult> ValidateUser(BaseUserModel model)
        {
            return await model.FluentInlineValidate(v =>
            {
                v.RuleFor(x => x.FirstName)
                    .NotEmpty().WithMessage("First name is required.")
                    .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");
                v.RuleFor(x => x.LastName)
                    .MaximumLength(100)
                    .WithMessage("First name cannot exceed 100 characters.");
                v.RuleFor(x => x.BirthDate)
                    .Must(x => x.HasValue && BaseHelper.IsFutureDate(x.Value))
                    .WithMessage("birth date cannot be from future");
                v.RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email format.");
                v.RuleFor(x => x.Phone)
                    .MinimumLength(10).WithMessage("Phone must be at least 10 digit long.");
                v.RuleFor(x => x)
                    .NotNull().WithMessage("Bad Request").SetValidator(v);
            });
        }

        public static async Task<ValidationResult> ValidateUser(BaseUser entity)
        {
            return await entity.FluentInlineValidate(v =>
            {
                v.RuleFor(x => x.FirstName)
                    .NotEmpty().WithMessage("First name is required.")
                    .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");
                v.RuleFor(x => x.LastName)
                    .MaximumLength(100)
                    .WithMessage("First name cannot exceed 100 characters.");
                v.RuleFor(x => x.BirthDate)
                    .Must(x => x.HasValue && BaseHelper.IsFutureDate(x.Value))
                    .WithMessage("birth date cannot be from future");
                v.RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email format.");
                v.RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(3).WithMessage("Password must be at least 3 characters long.");
                v.RuleFor(x => x.Phone)
                    .MinimumLength(10).WithMessage("Phone must be at least 10 digit long.");
                v.RuleFor(x => x)
                    .NotNull().WithMessage("Bad Request").SetValidator(v);
            });
        }
    }
}
