using xv_dotnet_demo_v2_domain.Entities;
using xv_dotnet_demo_v2_infrastructure;

namespace xv_dotnet_demo_v2_services.Implementations
{
    public class NamesService : INamesService
    {
        private readonly IGenericRepository<Names> _namesRepository;

        public NamesService(IGenericRepository<Names> namesRepository)
        {
            _namesRepository = namesRepository;
        }

        public async Task<IEnumerable<Names>> AllAsync()
        {
            return await _namesRepository.GetAllAsync();
        }

        public async Task<Names> GetNameAsync(int id)
        {
            return await _namesRepository.GetByIdAsync(id);
        }

        public async Task AddNameAsync(Names name)
        {
            await _namesRepository.AddAsync(name);
        }

        public async Task UpdateNameAsync(Names name)
        {
            await _namesRepository.UpdateAsync(name);
        }

        public async Task DeleteNameAsync(int id)
        {
            await _namesRepository.DeleteAsync(id);
        }
    }
}
