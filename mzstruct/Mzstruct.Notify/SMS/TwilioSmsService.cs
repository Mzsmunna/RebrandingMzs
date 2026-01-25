
using Microsoft.Extensions.Options;
using Mzstruct.Notify.Contracts;
using Mzstruct.Notify.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Verify.V2.Service;
using Twilio.Types;

namespace Mzstruct.Notify.SMS
{
    public class TwilioSmsService(IOptions<TwilioOption> option) : ISmsService
    {
        public async Task<string> SendSMS(string from, string to, string content)
        {
            var accountSid = option.Value.Sid;
            var authToken = option.Value.AuthToken;
            TwilioClient.Init(accountSid, authToken);

            var verification = VerificationResource.Create(
                to: "+8801744692979",
                channel: "sms",
                pathServiceSid: option.Value.ServiceSid
            );

            //Console.WriteLine(verification.Sid);

            var messageOptions = new CreateMessageOptions(
              new PhoneNumber("to"));
            var message = MessageResource.Create(messageOptions);
            Console.WriteLine(message.Body);
            return message.Sid;
        }
    }
}
