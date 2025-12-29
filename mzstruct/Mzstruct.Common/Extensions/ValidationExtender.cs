using FluentValidation;
using FluentValidation.Results;

namespace Mzstruct.Common.Extensions
{
    internal static class ValidationExtender
    {
        public static Task<ValidationResult> FluentInlineValidate<T>(this T obj,
            Action<InlineValidator<T>> configure)
        {
            var validator = new InlineValidator<T>();
            configure(validator);
            return validator.ValidateAsync(obj);
        }
    }
}
