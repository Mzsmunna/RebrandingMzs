using Kernel.Drivers.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Application.Interfaces
{
    public interface ICommandHandler<TCommand, TResult> where TCommand : notnull
    {
        Task<Result<TResult>> Execute(TCommand command); 
    }
}
