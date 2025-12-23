using Mzstruct.Base.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Common.Extensions
{
    public static class KernelExtender
    {
        public static Action<TOptions> ToConfigureAction<TOptions>(
            this IConfigurationSection section)
            where TOptions : class, new()
        {
            return options => section.Bind(options);
        }

        public static IActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller)
        {
            return result.Map<IActionResult>(
                Ok: data => controller.Ok(data),
                Err: error => error.ToProblem(controller)
            );
        }
    }
}
