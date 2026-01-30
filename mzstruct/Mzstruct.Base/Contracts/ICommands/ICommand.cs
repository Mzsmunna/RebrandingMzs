using Mzstruct.Base.Contracts.IQueries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Contracts.ICommands
{
    public interface ICommand<out TResponse> where TResponse : class
    {
    }

    public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult> where TResult : class
    {
        Task<TResult> HandleAsync(TCommand command, CancellationToken token = default);
    }
}
