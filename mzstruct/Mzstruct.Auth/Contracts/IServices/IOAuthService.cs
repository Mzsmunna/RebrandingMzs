using Mzstruct.Base.Dtos;
using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Auth.Contracts.IServices
{
    public interface IOAuthService
    {
        Task<Result<string>> SignInWithGoogle(string credential);
        Task<Result<string>> SignInWithGoogle();
        Task<Result<string>> SignInWithGitHub();
    }
}
