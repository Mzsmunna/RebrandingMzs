using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzstruct.Notify.Models;
using Twilio.AspNet.Core;
using Twilio.TwiML;

namespace Tasker.RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwilioSmsController : TwilioController
    {
        [HttpPost("Receive")] // webhook endpoint to receive incoming SMS
        public IActionResult ReceiveSms([FromForm] TwilioReplySms reply)
        {
            var messagingResponse = new MessagingResponse();
            messagingResponse.Message("Thanks for your message! We will get back to you shortly.");
            return TwiML(messagingResponse);
        }
    }
}
