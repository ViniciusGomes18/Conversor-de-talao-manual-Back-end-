using DSIN.Business.DTOs;
using DSIN.Business.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace DSIN.Api.Controllers;

[ApiController]
[Route("api/agent")]
public class AgentController : ControllerBase
{
    private readonly IAgentService _service;
    public AgentController(IAgentService service) => _service = service;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest("Email e senha são obrigatórios.");

        var result = await _service.LoginAsync(dto, ct);
        if (result is null) return Unauthorized(new { message = "Credenciais inválidas." });

        return Ok(result);
    }
}