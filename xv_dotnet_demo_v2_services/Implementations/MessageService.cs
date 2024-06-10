using xv_dotnet_demo_v2_domain.Entities;
using xv_dotnet_demo_v2_infrastructure;

namespace xv_dotnet_demo_v2_services.Implementations
{
    public class MessageService : IMessageService
    {
        private readonly IGenericRepository<Message> _messageRepository;

        public MessageService(IGenericRepository<Message> messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync()
        {
            return await _messageRepository.GetAllAsync();
        }

        public async Task<Message> GetMessageAsync(int id)
        {
            return await _messageRepository.GetByIdAsync(id);
        }

        public async Task AddMessageAsync(Message message)
        {
            await _messageRepository.AddAsync(message);
        }

        public async Task UpdateMessageAsync(Message message)
        {
            await _messageRepository.UpdateAsync(message);
        }

        public async Task DeleteMessageAsync(int id)
        {
            await _messageRepository.DeleteAsync(id);
        }
    }
}
