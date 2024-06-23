namespace BotWhatsapp.Services.QnAMakerApi
{
    public interface IQnAMakerApi
    {
        Task<string> Execute(string text);
    }
}
