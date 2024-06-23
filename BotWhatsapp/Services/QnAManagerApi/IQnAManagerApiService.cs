using BotWhatsapp.Models;

namespace BotWhatsapp.Services.QnAManagerApi
{
    public interface IQnAManagerApiService
    {
        Task<string> GetAll();

        public Task<string> Deploy(string token);

        Task<string> Create(qnaItem item);

        Task<string> Update(qnaItem item);

        Task<string> Delete(int idToDelete);
    }
}
