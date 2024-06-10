namespace xv_dotnet_demo.Dtos
{
    public class TokenDto
    {
        public string AccessToken { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }
    }
}
