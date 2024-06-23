using Azure;
using Azure.AI.Language.QuestionAnswering;
using BotWhatsapp.Models;
using BotWhatsapp.Services.AuthApi;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System;
using System.Text.Json;
using System.Text;

namespace BotWhatsapp.Services.QnAManagerApi
{
    public class QnAManagerApiService : IQnAManagerApiService
    {
        private readonly AzureBotConfig _azureBotConfig;
        private readonly IAuthApiService _authApiService;
        public QnAManagerApiService(IOptions<AzureBotConfig> azureBotConfig, IAuthApiService authApiService)
        {
            _azureBotConfig = azureBotConfig.Value;
            _authApiService = authApiService;
        }

        public async Task<string> Create(qnaItem item)
        {
            var token = await _authApiService.Execute();
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Patch,
                    $"{_azureBotConfig.EndPoint}/language/query-knowledgebases/projects/{_azureBotConfig.ProjectName}/qnas?{_azureBotConfig.ApiVersion}");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Headers.Add("Ocp-Apim-Subscription-Key", _azureBotConfig.SubscriptionKey);
                var random = new Random();
                int randomId = random.Next(1, 999999);
                QnaRequestBody requestBody = new QnaRequestBody(randomId, item.answer, item.questions, "add");
                string stringBody = "[" + JsonSerializer.Serialize(requestBody) + "]";

                var content = new StringContent(stringBody, Encoding.UTF8, "application/json");
                request.Content = content;
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    await this.Deploy(token);
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
                }
            }
        }

        public async Task<string> Delete(int idToDelete)
        {
            var token = await _authApiService.Execute();
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Patch, 
                    $"{_azureBotConfig.EndPoint}/language/query-knowledgebases/projects/{_azureBotConfig.ProjectName}/qnas?{_azureBotConfig.ApiVersion}");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Headers.Add("Ocp-Apim-Subscription-Key", _azureBotConfig.SubscriptionKey);
                string stringBody = "[{\r\n    \"op\": \"delete\",\r\n    \"value\": {\r\n      \"id\":" + idToDelete.ToString() + "}}]";
                var content = new StringContent(stringBody, Encoding.UTF8, "application/json");
                request.Content = content;
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    await this.Deploy(token);
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
                }
            }
        }

        public async Task<string> GetAll()
        {
            var token = await _authApiService.Execute();
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _azureBotConfig.EndPoint+"/language/query-knowledgebases/projects/"
                                                    +_azureBotConfig.ProjectName+ "/qnas?"+_azureBotConfig.ApiVersion);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Headers.Add("Ocp-Apim-Subscription-Key", _azureBotConfig.SubscriptionKey);
                var response = await httpClient.SendAsync(request);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var qnaResponse = JsonSerializer.Deserialize<QnaResponseGet>(responseContent);

                    var simplifiedQnaItems = qnaResponse.value.Select(item => new AnswerQuestion
                    {
                        Id = item.id.ToString(),
                        MainQuestion = item.questions.FirstOrDefault(),
                        Answer = item.answer,
                        Questions = item.questions,
                        Source = item.source
                    }).ToList();

                    var simplifiedResponse = JsonSerializer.Serialize(simplifiedQnaItems);
                    return simplifiedResponse;
                }
                else
                {
                    throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
                }
            }
        }

        public async Task<string> Update(qnaItem item)
        {
            var token = await _authApiService.Execute();
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Patch,
                    $"{_azureBotConfig.EndPoint}/language/query-knowledgebases/projects/{_azureBotConfig.ProjectName}/qnas?{_azureBotConfig.ApiVersion}");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Headers.Add("Ocp-Apim-Subscription-Key", _azureBotConfig.SubscriptionKey);
                int randomId = (int)item.id;
                QnaRequestBody requestBody = new QnaRequestBody(randomId, item.answer, item.questions, "replace");
                string stringBody = "[" + JsonSerializer.Serialize(requestBody) + "]";

                var content = new StringContent(stringBody, Encoding.UTF8, "application/json");
                request.Content = content;
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    await this.Deploy(token);
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
                }
            }
        }

        public async Task<string> Deploy(string token)
        {
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Put,
                    $"{_azureBotConfig.EndPoint}/language/query-knowledgebases/projects/{_azureBotConfig.ProjectName}/deployments/{_azureBotConfig.DeploymentName}?{_azureBotConfig.ApiVersion}");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Headers.Add("Ocp-Apim-Subscription-Key", _azureBotConfig.SubscriptionKey);
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
                }
            }
        }
    }
}
