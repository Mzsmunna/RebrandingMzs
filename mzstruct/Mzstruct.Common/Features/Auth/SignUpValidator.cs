using FluentValidation;
using Mzstruct.Base.Helpers;

namespace Mzstruct.Common.Features.Auth
{
    internal sealed class SignUpValidator: AbstractValidator<SignUpDto>
    {
        public SignUpValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");
            RuleFor(x => x.LastName)
                .MaximumLength(100)
                .WithMessage("First name cannot exceed 100 characters.");
            RuleFor(x => x.DOB)
                .Must(x => x.HasValue && BaseHelper.IsFutureDate(x.Value))
                .WithMessage("birth date cannot be from future");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(3).WithMessage("Password must be at least 3 characters long.");
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password can't be empty.")
                .Equal(x => x.Password).WithMessage("Password didn't match.");
            RuleFor(x => x.Phone)
                .MinimumLength(10).WithMessage("Phone must be at least 10 digit long.");
            RuleFor(x => x)
                .NotNull().WithMessage("Bad Request").SetValidator(this);
        }
    }
}
