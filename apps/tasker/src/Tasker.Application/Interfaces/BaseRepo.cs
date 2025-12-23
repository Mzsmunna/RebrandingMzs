using Mzstruct.Base.Dtos;
using Mzstruct.Base.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Errors;

namespace Tasker.Application.Interfaces
{
    public abstract class BaseRepo
    {
        private readonly ILogger _logger;

        protected BaseRepo(ILogger logger)
        {
            _logger = logger;
        }

        protected async Task<Result<T>> TryAsync<T>(Func<Task<T>> action)
        {
            try
            {
                T result = await action();
                return Result<T>.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Repository error");
                return Result<T>.Err(new(ErrorType.Network, ex.GetType().Name, ex.Message, 502, "", ex.InnerException?.Message ?? ""));
            }
        }
    }
}
