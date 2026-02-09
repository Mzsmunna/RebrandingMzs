using Mzstruct.Base.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mzstruct.Base.Patterns.Result;

namespace Mzstruct.Base.Repos
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

                // Build a dictionary for inner error details (matches expected parameter type)
                Dictionary<string, string[]>? innerException = ex.InnerException?.Message != null
                    ? new Dictionary<string, string[]> { { "inner", new[] { ex.InnerException.Message } } }
                    : null;

                return Result<T>.Err(new(ErrorType.Network, ex.GetType().Name, ex.Message, 502, "", innerException));
            }
        }
    }
}
