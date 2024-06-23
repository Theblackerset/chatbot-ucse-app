using System;
using Twilio.TwiML.Voice;

namespace BotWhatsapp.Models
{
    public class dialog
    {
        public dialog()
        {
            isContextOnly = false;
            prompts = new List<object>();
        }
        public bool isContextOnly { get; set; }
        public List<object> prompts { get; set; }
    }

    public class metadata
    {
        public metadata() {
            k1 = "v1";
            k2 = "v2";
        }
        public string? k1 { get; set; }
        public string? k2 { get; set; }

    }

    public class qnaItem
    {
        public qnaItem(int actionId) {
            id = actionId;
            answer = string.Empty;
            source = string.Empty;
            questions=new List<string>();
            metadata = new metadata();
            dialog = new dialog();
        }
        public qnaItem(int actionId, string Answer, List<string> Questions)
        {
            id = actionId;
            answer = Answer;
            source = "Editorial";
            questions = Questions;
            metadata = new metadata();
            dialog = new dialog();
        }
        public qnaItem(){ }

        public int? id { get; set; }
        public string? answer { get; set; }
        public string? source { get; set; }
        public List<string>? questions { get; set; }
        public metadata? metadata { get; set; }
        public dialog? dialog { get; set; }
        public List<object>? activeLearningSuggestions { get; set; }
        public bool? isDocumentText { get; set; }
        public string? lastUpdatedDateTime { get; set; }
    }

    public class QnaResponseGet
    {
        public List<qnaItem>? value { get; set; }
    }
}
