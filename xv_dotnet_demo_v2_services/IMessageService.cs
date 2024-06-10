using xv_dotnet_demo_v2_domain.Entities;

namespace xv_dotnet_demo_v2_services
{
    public interface IMessageService
    {
        Task<IEnumerable<Message>> GetMessagesAsync();

        Task<Message> GetMessageAsync(int id);

        Task AddMessageAsync(Message message);

        Task UpdateMessageAsync(Message message);

        Task DeleteMessageAsync(int id);
    }
}
