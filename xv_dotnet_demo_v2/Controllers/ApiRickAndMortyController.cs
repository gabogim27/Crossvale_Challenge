using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using xv_dotnet_demo.Dtos;
using xv_dotnet_demo_v2_services;

namespace xv_dotnet_demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiRickAndMortyController : ControllerBase
    {
        private readonly IRickAndMortyService _rickAndMortyService;

        public ApiRickAndMortyController(IRickAndMortyService rickAndMortyService)
        {
            _rickAndMortyService = rickAndMortyService;
        }

        [HttpGet("character/{id}")]
        public async Task<IActionResult> GetCharacter(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id should be greater than 0");
            }

            try
            {
                var character = await _rickAndMortyService.GetCharacterAsync(id);
                return Ok(JsonSerializer.Deserialize<CharacterDto>(character));
            }
            catch (HttpRequestException ex)
            {
                return StatusCode((int)HttpStatusCode.BadGateway, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = $"Internal server error: {ex.Message}" });
            }
        }
    }
}
