using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Extensions
{
    public static class ValidationExtender
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
