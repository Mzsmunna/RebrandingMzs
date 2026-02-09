using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Patterns.CQRS
{
    public interface IRequestSender
    {
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
        Task<TResult> CommandAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
    }
}
