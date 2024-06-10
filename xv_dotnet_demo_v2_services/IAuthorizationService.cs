using xv_dotnet_demo_v2_domain.Authorization;

namespace xv_dotnet_demo_v2_services
{
    public interface IAuthorizationService
    {
        Task<Token> BuildToken(string issuer, string subject);

        Task<IssuerData> ValidateToken(string token, string issuer);
    }
}
