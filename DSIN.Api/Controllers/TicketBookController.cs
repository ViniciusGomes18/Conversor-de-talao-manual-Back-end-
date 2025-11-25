using DSIN.Business.DTOs;
using DSIN.Business.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace DSIN.Api.Controllers;

[ApiController]
[Route("api/ticketbook")]
public sealed class TicketBookController : ControllerBase
{
    private readonly ITicketBookService _service;

    public TicketBookController(ITicketBookService service) => _service = service;

    [HttpPost("analyze")]
    public async Task<ActionResult<TicketBookResponseDto>> Analyze([FromBody] CreateTicketBookDto req, CancellationToken ct)
    {
        var dto = await _service.AnalyzeAsync(req.AgentId, req.ImageBase64, ct);
        return Ok(dto);
    }

    [HttpGet("agent/{agentId:guid}")]
    public async Task<IActionResult> ListByAgent([FromRoute] Guid agentId, [FromQuery] int skip = 0, [FromQuery] int take = 50, CancellationToken ct = default)
    {
        var result = await _service.ListByAgentAsync(agentId, skip, take, ct);
        return Ok(result);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var ticket = await _service.GetByIdAsync(id, ct);
        if (ticket is null)
            return NotFound();
    
        return Ok(ticket);
    }
}
