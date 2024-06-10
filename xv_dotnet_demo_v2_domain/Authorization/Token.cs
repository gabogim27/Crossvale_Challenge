namespace xv_dotnet_demo_v2_domain.Authorization
{
    public class Token
    {
        public string AccessToken { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }
    }
}
