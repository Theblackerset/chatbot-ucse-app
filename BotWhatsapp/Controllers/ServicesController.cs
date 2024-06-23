using BotWhatsapp.Services.QnAMakerApi;
using Microsoft.AspNetCore.Mvc;

namespace BotWhatsapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IQnAMakerApi _qnAMakerApiService;
        public ServicesController(IQnAMakerApi qnAMakerApiService) 
        { 
            _qnAMakerApiService = qnAMakerApiService;
        }

        [HttpGet("qnamaker")]
        public async Task <IActionResult> GetQnAMaker(string message)
        {
            var result = await _qnAMakerApiService.Execute(message);
            return Ok(result);
        }
    }
}
