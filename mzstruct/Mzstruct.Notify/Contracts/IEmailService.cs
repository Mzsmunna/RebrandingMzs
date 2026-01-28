using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Notify.Contracts
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(
            string to,
            string subject,
            string htmlBody,
            string? plainTextBody = null,
            CancellationToken cancellationToken = default);
    }
}
