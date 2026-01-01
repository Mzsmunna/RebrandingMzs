using FluentValidation;
using FluentValidation.Results;
using Tasker.Application.Features.Issues;
using Tasker.Application.Extensions;

namespace Tasker.Application.Validators
{
    internal static class TaskerValidator
    {
        public static async Task<ValidationResult> ValidateIssue(IssueModel model)
        {
            return await model.FluentInlineValidate(v =>
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
            return await entity.FluentInlineValidate(v =>
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
                v.RuleFor(x => x.UserId)
                    .NotEmpty().WithMessage("UserId is required.");
                v.RuleFor(x => x.AssignerId)
                    .NotEmpty().WithMessage("AssignerId is required.");
                v.RuleFor(x => x)
                    .NotNull().WithMessage("Bad Request").SetValidator(v);
            });
        }
    }
}
