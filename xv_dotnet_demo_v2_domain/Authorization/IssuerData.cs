namespace xv_dotnet_demo_v2_domain.Authorization
{
    public class IssuerData
    {
        public string Issuer { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }

        public DateTime Now { get; set; }
    }
}
