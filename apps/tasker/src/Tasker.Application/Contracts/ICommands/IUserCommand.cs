using Mzstruct.Base.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Features.Users;

namespace Tasker.Application.Contracts.ICommands
{
    public interface IUserCommand
    {
        Task<Result<User?>> CreateUser(UserModel user);
        Task<Result<User?>> UpdateUser(User user);
        Task<Result<bool>> DeleteUser(string id);
    }
}
