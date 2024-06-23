using Azure;
using Azure.AI.Language.QuestionAnswering;
using BotWhatsapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BotWhatsapp.Services.AuthApi
{

    public class AuthApiService : IAuthApiService
    {
        private readonly AuthConfig _authConfig;
        public AuthApiService(IOptions<AuthConfig> authConfig)
        {
            _authConfig = authConfig.Value;
        }

        public async Task<string> Execute()
        {
            using (var httpClient = new HttpClient())
            {
                var parameters = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", _authConfig.ClientId },
                { "client_secret", _authConfig.ClientSecret },
                { "scope", _authConfig.Scope }
            };

                var content = new FormUrlEncodedContent(parameters);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                var response = await httpClient.PostAsync(_authConfig.Url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);
                    if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.access_token))
                    {
                        return tokenResponse.access_token;
                    }
                    else
                    {
                        throw new Exception("Access token not found in the response.");
                    }
                }
                else
                {
                    throw new HttpRequestException($"Request to {_authConfig.Url} failed with status code {response.StatusCode}: {responseContent}");
                }
            }
        }
    }
}
