namespace xv_dotnet_demo_v2_domain.Settings
{
    public class JwtSettings
    {
        public string Key { get; set; }

        public int TokenValiditySeconds { get; set; }

        public JwtDefault Default { get; set; }

        public JwtValid Valid { get; set; }

        public RickAndMortyApiSettings RickAndMortyAPI { get; set; }
    }
}
