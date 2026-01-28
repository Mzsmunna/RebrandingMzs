using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using Mzstruct.Base.Helpers;
using Mzstruct.Notify.Configs;
using Mzstruct.Notify.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Mzstruct.Notify.Services
{
    public class EmailService(
        //IConfiguration config, 
        IOptions<EmailConfig> options
        //IHttpContextAccessor httpContextAccessor
    ) : IEmailService
    {
        private readonly EmailConfig _options = options.Value;

        public async Task<bool> SendEmailAsync(
            string to,
            string subject,
            string htmlBody,
            string? plainTextBody = null,
            CancellationToken cancellationToken = default)
        {
            var message = new MimeMessage();

            // From
            message.From.Add(new MailboxAddress(_options.FromName, _options.FromEmail));

            // To
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            // Body (plain + HTML alternative)
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlBody,
                TextBody = plainTextBody ?? BaseHelper.StripHtml(htmlBody) // simple fallback
            };
            message.Body = bodyBuilder.ToMessageBody();
            using var client = new SmtpClient();

            try
            {
                // Connect
                if (_options.EnableTLS)
                {
                    await client.ConnectAsync(
                        _options.Host,
                        _options.Port,
                        SecureSocketOptions.StartTls,
                        cancellationToken);
                }
                else
                {
                    await client.ConnectAsync(
                        _options.Host,
                        _options.Port,
                        _options.EnableSSL ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.None,
                        cancellationToken);
                }

                // Auth
                if (!string.IsNullOrEmpty(_options.UserName))
                {
                    await client.AuthenticateAsync(_options.UserName, _options.Password, cancellationToken);
                }

                // Send
                await client.SendAsync(message, cancellationToken);
            }
            finally
            {
                if (client.IsConnected)
                    await client.DisconnectAsync(true, cancellationToken);
            }
            return true;
        }
    }
}
