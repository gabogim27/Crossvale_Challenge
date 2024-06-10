using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using xv_dotnet_demo.Dtos;
using xv_dotnet_demo_v2_domain.Entities;
using xv_dotnet_demo_v2_services;

namespace xv_dotnet_demo.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiMessageController : ControllerBase
{
    private readonly IMessageService _service;

    private readonly IMapper _mapper;

    public ApiMessageController(IMessageService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet("message/{id}")]
    public async Task<ActionResult<MessageDto>> GetMessage(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }

        var message = await _service.GetMessageAsync(id);
        return Ok(_mapper.Map<MessageDto>(message));
    }

    [HttpGet("message/all")]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessages()
    {
        var messages = await _service.GetMessagesAsync();
        return Ok(messages.Select(x => _mapper.Map<MessageDto>(x)));
    }

    [HttpPost("message/add")]
    public async Task<ActionResult> AddMessage(MessageDto message)
    {
        if (string.IsNullOrWhiteSpace(message?.message) || message?.id <= 0)
        {
            return BadRequest();
        }

        await _service.AddMessageAsync(_mapper.Map<Message>(message));
        return CreatedAtAction(nameof(GetMessage), new { id = message.id }, message);
    }

    [HttpPut("message/update")]
    public async Task<ActionResult> UpdateMessage(MessageDto message)
    {
        if (message?.id <= 0 || string.IsNullOrWhiteSpace(message?.message))
        {
            return BadRequest();
        }

        await _service.UpdateMessageAsync(_mapper.Map<Message>(message));
        return NoContent();
    }

    [HttpDelete("message/delete")]
    public async Task<ActionResult> DeleteMessage(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }

        await _service.DeleteMessageAsync(id);
        return NoContent();
    }
}
