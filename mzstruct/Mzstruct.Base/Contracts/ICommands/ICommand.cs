using Mzstruct.Base.Contracts.IQueries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Contracts.ICommands
{
    public interface ICommand<out TResponse>
    {
    }

    public interface ICommandHandler<in TCommand, TResult> where TCommand : notnull, ICommand<TResult>
    {
        Task<TResult> HandleAsync(TCommand command, CancellationToken token = default);
    }
}
