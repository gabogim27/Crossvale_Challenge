using Microsoft.EntityFrameworkCore;
using xv_dotnet_demo_v2_domain.Entities;
using xv_dotnet_demo_v2_infrastructure.DbContext;

namespace xv_dotnet_demo_v2_infrastructure.Repository
{
    public class MessageRepository : IGenericRepository<Message>
    {
        protected readonly ApplicationDBContext _dbContext;

        public MessageRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Message>> GetAllAsync()
        {
            return await _dbContext.Message.ToListAsync();
        }

        public async Task<Message> GetByIdAsync(int id)
        {
            return await _dbContext.Message.FindAsync(id);
        }

        public async Task AddAsync(Message entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Message entity)
        {
            var existingMessage = _dbContext.Message.FirstOrDefault(m => m.id == entity.id);
            if (existingMessage != null)
            {
                existingMessage.message = entity.message;
                await _dbContext.SaveChangesAsync();
            }

            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var messageToRemove = _dbContext.Message.FirstOrDefault(m => m.id == id);
            if (messageToRemove != null)
            {
                _dbContext.Message.Remove(messageToRemove);
                await _dbContext.SaveChangesAsync();
            }

            await Task.CompletedTask;
        }
    }
}
