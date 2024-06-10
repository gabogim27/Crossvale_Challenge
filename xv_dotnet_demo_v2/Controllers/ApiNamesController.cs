using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using xv_dotnet_demo.Dtos;
using xv_dotnet_demo_v2_domain.Entities;
using xv_dotnet_demo_v2_services;

namespace xv_dotnet_demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiNamesController : ControllerBase
    {
        private readonly INamesService _service;

        private readonly IMapper _mapper;

        public ApiNamesController(INamesService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("name/{id}")]
        public async Task<ActionResult<NameDto>> GetName(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var name = await _service.GetNameAsync(id);
            return Ok(_mapper.Map<NameDto>(name));
        }

        [HttpGet("name/all")]
        public async Task<ActionResult<IEnumerable<NameDto>>> GetNames()
        {
            var names = await _service.AllAsync();
            return Ok(names.Select(x => _mapper.Map<NameDto>(x)));
        }

        [HttpPost("name/add")]
        public async Task<ActionResult> AddName(NameDto nameDto)
        {
            if (string.IsNullOrWhiteSpace(nameDto?.Name))
            {
                return BadRequest();
            }

            await _service.AddNameAsync(_mapper.Map<Names>(nameDto));
            return CreatedAtAction(nameof(GetName), new { id = nameDto.Id }, nameDto.Name);
        }

        [HttpPut("name/update")]
        public async Task<ActionResult> UpdateName(NameDto nameDto)
        {
            if (nameDto?.Id <= 0 || string.IsNullOrWhiteSpace(nameDto?.Name))
            {
                return BadRequest();
            }

            await _service.UpdateNameAsync(_mapper.Map<Names>(nameDto));
            return NoContent();
        }

        [HttpDelete("name/delete")]
        public async Task<ActionResult> DeleteName(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            await _service.DeleteNameAsync(id);
            return NoContent();
        }
    }
}
