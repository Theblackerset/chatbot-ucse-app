using Azure;
using Azure.AI.Language.QuestionAnswering;
using BotWhatsapp.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BotWhatsapp.Services.QnAMakerApi
{

    public class QnAMakerApi : IQnAMakerApi
    {
        private readonly AzureBotConfig _azureBotConfig;
        public QnAMakerApi(IOptions<AzureBotConfig> azureBotConfig)
        {
            _azureBotConfig = azureBotConfig.Value;
        }

        public async Task<string> Execute(string text)
        {
            //Parámetros obtenidos de Azure Language Studio
            AzureKeyCredential credential = new AzureKeyCredential(_azureBotConfig.SubscriptionKey);
            Uri endpointUri = new Uri(_azureBotConfig.EndPoint);
            QuestionAnsweringClient client = new QuestionAnsweringClient(endpointUri, credential);
            QuestionAnsweringProject project = new QuestionAnsweringProject(_azureBotConfig.ProjectName, _azureBotConfig.DeploymentName);

            //Se setea un Size por defecto para que el chatbot me devuelva 3 respuestas como máximo.
            var response = await client.GetAnswersAsync(text, project, new AnswersOptions { Size = 3 });

            if (response != null && response.Value.Answers.Count > 0)
            { 
                var answerBot = response.Value.Answers[0].Answer;
                var score = response.Value.Answers[0].Confidence;

                //50> valido
                if (score < 0.5)
                    return "Lo siento, pero no cuento con la respuesta a tu pregunta, intenta escribirla de otra forma";
                else
                    return answerBot;
            }

            return "Lo siento, algo salió mal. intentalo más tarde.";
        }
    }
}



