using Microsoft.Extensions.Options;
using xv_dotnet_demo_v2_domain.Settings;

namespace xv_dotnet_demo_v2_services.Implementations
{
    public class RickAndMortyService : IRickAndMortyService
    {
        private readonly HttpClient _httpClient;

        private readonly RickAndMortyApiSettings _mortyApiSettings;

        public RickAndMortyService(HttpClient httpClient, IOptions<RickAndMortyApiSettings> settings)
        {
            _httpClient = httpClient;
            _mortyApiSettings = settings.Value;
        }

        public async Task<string> GetCharacterAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_mortyApiSettings.CharacterUri}/{id}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return data;
            }

            throw new HttpRequestException(
                $"Request to the external API failed with status {response?.StatusCode}-{response?.ReasonPhrase}");
        }
    }
}
