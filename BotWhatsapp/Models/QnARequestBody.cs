using Twilio.TwiML.Voice;

namespace BotWhatsapp.Models
{
    public class QnaRequestBody
    {
        public QnaRequestBody(int actionId, string action)
        {
            op = action;
            value = new qnaItem(actionId);
        }

        public QnaRequestBody(int actionId, string Answer, List<string> Questions, string action)
        {
            op = action;
            value = new qnaItem(actionId, Answer, Questions);
        }
        public QnaRequestBody(){ }
        public string? op { get; set; }
        public qnaItem? value { get; set; }
    }
}
