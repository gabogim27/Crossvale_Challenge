namespace xv_dotnet_demo.Dtos
{
    public class IssuerDataDto
    {
        public string Issuer { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }

        public DateTime Now { get; set; }
    }
}
