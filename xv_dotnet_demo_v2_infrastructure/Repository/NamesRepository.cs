using Microsoft.EntityFrameworkCore;
using xv_dotnet_demo_v2_domain.Entities;
using xv_dotnet_demo_v2_infrastructure.DbContext;

namespace xv_dotnet_demo_v2_infrastructure.Repository
{
    public class NamesRepository : IGenericRepository<Names>
    {
        protected readonly ApplicationDBContext _dbContext;

        public NamesRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Names>> GetAllAsync()
        {
            return await _dbContext.Names.ToListAsync();
        }

        public async Task<Names> GetByIdAsync(int id)
        {
            return await _dbContext.Names.FindAsync(id);
        }

        public async Task AddAsync(Names entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Names entity)
        {
            var existingName = _dbContext.Names.FirstOrDefault(m => m.Id == entity.Id);
            if (existingName != null)
            {
                existingName.Name = entity.Name;
                await _dbContext.SaveChangesAsync();
            }

            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var nameToRemove = _dbContext.Names.FirstOrDefault(m => m.Id == id);
            if (nameToRemove != null)
            {
                _dbContext.Names.Remove(nameToRemove);
                await _dbContext.SaveChangesAsync();
            }

            await Task.CompletedTask;
        }
    }
}
