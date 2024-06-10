using xv_dotnet_demo_v2_domain.Entities;

namespace xv_dotnet_demo_v2_services
{
    public interface INamesService
    {
        Task<IEnumerable<Names>> AllAsync();

        Task<Names> GetNameAsync(int id);

        Task AddNameAsync(Names name);

        Task UpdateNameAsync(Names name);

        Task DeleteNameAsync(int id);
    }
}
