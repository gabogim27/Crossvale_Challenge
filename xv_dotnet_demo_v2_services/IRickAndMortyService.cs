namespace xv_dotnet_demo_v2_services
{
    public interface IRickAndMortyService
    {
        Task<string> GetCharacterAsync(int id);
    }
}
