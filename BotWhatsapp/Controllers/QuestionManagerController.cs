using BotWhatsapp.Models;
using BotWhatsapp.Services.AuthApi;
using BotWhatsapp.Services.QnAManagerApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BotWhatsapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionManagerController : ControllerBase
    {

        private readonly IQnAManagerApiService _qnAManagerApiService;
        public QuestionManagerController(IQnAManagerApiService qnAManagerApiService)
        {
            _qnAManagerApiService = qnAManagerApiService;
        }

        [HttpGet("Questions")]
        public async Task<IActionResult> Questions()
        {
            var result = await _qnAManagerApiService.GetAll();
            return Ok(result);
        }

        [HttpPatch("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var result = await _qnAManagerApiService.Delete(id);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }

        [HttpPatch("Add")]
        public async Task<IActionResult> Add([FromBody] AnswerQuestion answerQuestion)
        {
            try
            {
                answerQuestion.Id = "1";
                var questionList = new List<string>();
                questionList.Add(answerQuestion.MainQuestion);
                questionList.AddRange(answerQuestion.Questions);
                var QnaItem = new qnaItem
                {
                    id = int.Parse(answerQuestion.Id.Trim()),
                    answer = answerQuestion.Answer,
                    questions = questionList
                };

                var result = await _qnAManagerApiService.Create(QnaItem);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch("replace")]
        public async Task<IActionResult> Replace([FromBody] AnswerQuestion answerQuestion)
        {
            try
            {
                var questionList = new List<string>();
                questionList.Add(answerQuestion.MainQuestion);
                questionList.AddRange(answerQuestion.Questions);
                var QnaItem = new qnaItem
                {
                    id = int.Parse(answerQuestion.Id.Trim()),
                    answer = answerQuestion.Answer,
                    questions = questionList
                };

                var result = await _qnAManagerApiService.Update(QnaItem);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
