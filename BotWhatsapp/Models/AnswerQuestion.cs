namespace BotWhatsapp.Models
{
    public class AnswerQuestion
    {
        public string Id { get; set; }
        public string MainQuestion { get; set; }
        public string Answer { get; set; }
        public List<string> Questions { get; set; }
        public string Source { get; set; }


    }
}