using FluentValidation;
using FluentValidation.Results;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Extensions;
using Mzstruct.Base.Helpers;
using Mzstruct.Base.Models;
using Mzstruct.Common.Extensions;

namespace Mzstruct.Common.Validators
{
    internal static class UserValidator
    {
        public static async Task<ValidationResult> Validate(BaseUserModel model)
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

        public static async Task<ValidationResult> Validate(BaseUser entity)
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
