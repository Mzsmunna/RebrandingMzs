using Microsoft.AspNetCore.Mvc;
using Mzstruct.Base.Dtos;
using Mzstruct.Base.Errors;
using Mzstruct.Common.Contracts.ICommands;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Features.Auth;
using Tasker.Application.Features.Users;

namespace Tasker.Application.Contracts.ICommands
{
    public interface IAuthCommand : IAppAuthCommand { }
}
