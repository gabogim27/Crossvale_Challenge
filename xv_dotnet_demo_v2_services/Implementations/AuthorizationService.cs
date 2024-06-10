using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using xv_dotnet_demo_v2_domain.Authorization;
using xv_dotnet_demo_v2_domain.Exceptions;
using xv_dotnet_demo_v2_domain.Settings;

namespace xv_dotnet_demo_v2_services.Implementations
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly JwtSettings _jwtSettings;

        public AuthorizationService(IOptions<JwtSettings> settings)
        {
            _jwtSettings = settings.Value;
        }

        public async Task<Token> BuildToken(string issuer, string subject)
        {
            var iss = !string.IsNullOrWhiteSpace(issuer) ? issuer : _jwtSettings.Default.Issuer;
            var sub = !string.IsNullOrWhiteSpace(subject) ? subject : _jwtSettings.Default.Issuer;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var secToken = GetSecToken(credentials, iss.Trim(), sub.Trim());
            var handler = new JwtSecurityTokenHandler();

            var token = new Token
            {
                AccessToken = handler.WriteToken(secToken),
                ValidFrom = secToken.ValidFrom,
                ValidTo = secToken.ValidTo
            };

            return await Task.FromResult(token);
        }

        public async Task<IssuerData> ValidateToken(string token, string issuer)
        {
            var issuerObj = new IssuerData();
            try
            {
                var validIssuer = !string.IsNullOrWhiteSpace(issuer) ? issuer : _jwtSettings.Valid.Issuer;
                var tokenHandler = new JwtSecurityTokenHandler();
                var now = DateTime.UtcNow;
                var validationParameters = GetTokenValidationParameters(validIssuer);
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                if (principal != null && principal.Identity.IsAuthenticated && now >= validatedToken.ValidFrom && now <= validatedToken.ValidTo)
                {
                    issuerObj.Issuer = issuer;
                    issuerObj.ValidFrom = validatedToken.ValidFrom;
                    issuerObj.ValidTo = validatedToken.ValidTo;
                    issuerObj.Now = now;
                }
            }
            catch (Exception ex)
            {
                throw new ValidateIssuerException();
            }

            return await Task.FromResult(issuerObj);
        }

        private TokenValidationParameters GetTokenValidationParameters(string validIssuer)
        {
            return new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                ValidIssuer = validIssuer,
                ValidateIssuer = true,
                ValidateAudience = false
            };
        }

        private JwtSecurityToken GetSecToken(SigningCredentials credentials, string iss, string sub)
        {
            return new JwtSecurityToken(
                signingCredentials: credentials,
                issuer: iss,
                claims: new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, sub)
                },
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddSeconds(_jwtSettings.TokenValiditySeconds));
        }
    }
}
