
using Microsoft.Extensions.Options;
using Mzstruct.Notify.Configs;
using Mzstruct.Notify.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Verify.V2.Service;
using Twilio.TwiML;
using Twilio.Types;

namespace Mzstruct.Notify.SMS
{
    public class TwilioSmsService(IOptions<TwilioConfig> option) : ISmsService
    {
        public async Task<string> Send(string from, string to, string content)
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

            //var messageOptions = new CreateMessageOptions(
            //  new PhoneNumber(to));
            //var message = MessageResource.Create(messageOptions);
            var message = MessageResource.Create(
              body: content,
              from: new PhoneNumber(from),
              to: new PhoneNumber(to)
            );

            Console.WriteLine(message.Body);
            return message.Sid;
        }

        public async Task<string> Receive(string from, string to, string content)  // webhook: when someone replies
        {
            var reply = new MessagingResponse();
            var response = reply.Message(content, to, from);
            return response.ToString();
        }
    }
}
