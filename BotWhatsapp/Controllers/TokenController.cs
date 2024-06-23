using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using BotWhatsapp.Services.AuthApi;


namespace TokenService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IAuthApiService _authApiService;
        public TokenController(IAuthApiService authApiService)
        {
            _authApiService = authApiService;
        }

        [HttpGet("GetAccessToken")]
        public async Task<IActionResult> GetAccessToken()
        {
            var result = await _authApiService.Execute();
            return Ok(result.ToString());
        }
    }


}
