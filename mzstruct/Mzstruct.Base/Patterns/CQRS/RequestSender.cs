using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Patterns.CQRS
{
    public class RequestSender(IServiceProvider provider) : IRequestSender
    {
        public async Task<TResult> CommandAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
        {
            var handlerType  = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            var handler = provider.GetRequiredService(handlerType);
            var method = handlerType.GetMethod("HandleAsync");
            if (method is null) throw new InvalidOperationException("Command Handler does not contain a HandleAsync method.");
            var task = (Task<TResult>) method.Invoke(handler, new object[] { command, cancellationToken })!;
            return await task;
        }

        public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        {
            var handlerType  = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            var handler = provider.GetRequiredService(handlerType);
            var method = handlerType.GetMethod("HandleAsync");
            if (method is null) throw new InvalidOperationException("Query Handler does not contain a HandleAsync method.");
            var task = (Task<TResult>) method.Invoke(handler, new object[] { query, cancellationToken })!;
            return await task;
        }
    }
}
