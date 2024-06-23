using BotWhatsapp.Services.QnAMakerApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twilio.AspNet.Common;
using Twilio.AspNet.Core;
using Twilio.TwiML;

namespace BotWhatsapp.Controllers
{
    [Route("api/whatsapp")]
    public class WhatsappController : TwilioController
    {
        private IQnAMakerApi _qnAMakerApiService;

        public WhatsappController(IQnAMakerApi qnAMakerApiService)
        {
            _qnAMakerApiService = qnAMakerApiService;
        }

        [HttpPost("message")]
        public async Task<TwiMLResult> MessageAsync(SmsRequest input) 
        { 
            var response = new MessagingResponse();
            string textUser = input.Body; //Texto del usuario
            string textBot = await _qnAMakerApiService.Execute(textUser);

            response.Message(textBot);
            return TwiML(response);
        }
    }
}
