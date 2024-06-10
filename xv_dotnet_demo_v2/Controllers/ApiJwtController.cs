using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using xv_dotnet_demo.Dtos;
using xv_dotnet_demo_v2_domain.Exceptions;
using xv_dotnet_demo_v2_services;

namespace xv_dotnet_demo.Controllers;

[ApiController]
[Route("jwt")]
public class ApiJwtController : ControllerBase
{
    private readonly ILogger<ApiJwtController> _logger;

    private readonly IAuthorizationService _authorizationService;

    private readonly IMapper _mapper;

    public ApiJwtController(ILogger<ApiJwtController> logger, IAuthorizationService authorizationService, IMapper mapper)
    {
        _logger = logger;
        _authorizationService = authorizationService;
        _mapper = mapper;
    }

    [HttpGet("build")]
    public async Task<ActionResult<TokenDto>> buildToken(string issuer = null, string subject = null)
    {
        var token = await _authorizationService.BuildToken(issuer, subject);

        return Ok(_mapper.Map<TokenDto>(token));
    }

    [HttpGet("validate")]
    public async Task<IActionResult> validateToken(string issuer = null)
    {
        try
        {
            var token = CheckIfTokenIsPresent();
            var issuerData = await _authorizationService.ValidateToken(token, issuer);

            if (!string.IsNullOrWhiteSpace(issuerData.Issuer))
            {
                return Ok(_mapper.Map<IssuerDataDto>(issuerData));
            }
            else
            {
                _logger.LogWarning("The token is not valid");
                return StatusCode(403);
            }
        }
        catch (ValidateTokenException ex)
        {
            // if something wrong happens when getting the token, we need to return a 401
            _logger.LogError("An error occured when trying to get the token from Authorization Header.", ex);
            return StatusCode(401);
        }
        catch (ValidateIssuerException ex)
        {
            _logger.LogError("An error occured when trying to validate the token.", ex);
            return StatusCode(403);
        }
        catch (Exception ex)
        {
            _logger.LogError("An unexpected error occured when trying to process the token.", ex);
            return StatusCode(401);
        }
    }

    private string CheckIfTokenIsPresent()
    {
        if (Request.Headers.ContainsKey("Authorization"))
        {
            var token = Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrWhiteSpace(token) && token.Contains("bearer", StringComparison.CurrentCultureIgnoreCase))
            {
                return token.Replace("bearer ", string.Empty, StringComparison.CurrentCultureIgnoreCase);
            }
        }

        _logger.LogWarning("Authorization header is not present");
        throw new ValidateTokenException("3001");
    }
}
