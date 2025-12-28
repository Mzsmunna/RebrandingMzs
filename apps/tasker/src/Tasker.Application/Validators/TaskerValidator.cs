using FluentValidation;
using FluentValidation.Results;
using Mzstruct.Base.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Features.Auth;
using Tasker.Application.Extensions;
using Tasker.Application.Features.Users;
using Tasker.Application.Features.Issues;

namespace Tasker.Application.Validators
{
    internal static class TaskerValidator
    {
        public static async Task<ValidationResult> ValidateSignUp(SignUpDto dto)
        {
            return await dto.ValidateInlineAsync(v =>
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
            return await dto.ValidateInlineAsync(v =>
            {
                v.RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email format.");
                v.RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(3).WithMessage("Password must be at least 3 characters long.");
            });
        }

        public static async Task<ValidationResult> ValidateUser(UserModel model)
        {
            return await model.ValidateInlineAsync(v =>
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

        public static async Task<ValidationResult> ValidateUser(User entity)
        {
            return await entity.ValidateInlineAsync(v =>
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

        public static async Task<ValidationResult> ValidateIssue(IssueModel model)
        {
            return await model.ValidateInlineAsync(v =>
            {
                v.RuleFor(x => x.Title)
                    .NotEmpty().WithMessage("Title is required.")
                    .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");
                v.RuleFor(x => x.Type)
                    .MaximumLength(100)
                    .WithMessage("First name cannot exceed 100 characters.");
                v.RuleFor(x => x.Summary)
                    .NotEmpty().WithMessage("Summary is required.")
                    .MaximumLength(300).WithMessage("Summary cannot exceed 300 characters.");
                v.RuleFor(x => x.Description)
                    //.NotEmpty().WithMessage("Description is required.")
                    .MaximumLength(4000).WithMessage("Title cannot exceed 4000 characters.");
                v.RuleFor(x => x.AssignedId)
                    .NotEmpty().WithMessage("AssignedId is required.");
                v.RuleFor(x => x)
                    .NotNull().WithMessage("Bad Request").SetValidator(v);
            });
        }

        public static async Task<ValidationResult> ValidateIssue(Issue entity)
        {
            return await entity.ValidateInlineAsync(v =>
            {
                v.RuleFor(x => x.Title)
                    .NotEmpty().WithMessage("Title is required.")
                    .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");
                v.RuleFor(x => x.Type)
                    .MaximumLength(100)
                    .WithMessage("First name cannot exceed 100 characters.");
                v.RuleFor(x => x.Summary)
                    .NotEmpty().WithMessage("Summary is required.")
                    .MaximumLength(300).WithMessage("Summary cannot exceed 300 characters.");
                v.RuleFor(x => x.Description)
                    //.NotEmpty().WithMessage("Description is required.")
                    .MaximumLength(4000).WithMessage("Title cannot exceed 4000 characters.");
                v.RuleFor(x => x.AssignedId)
                    .NotEmpty().WithMessage("AssignedId is required.");
                v.RuleFor(x => x.AssignerId)
                    .NotEmpty().WithMessage("AssignerId is required.");
                v.RuleFor(x => x)
                    .NotNull().WithMessage("Bad Request").SetValidator(v);
            });
        }
    }
}
