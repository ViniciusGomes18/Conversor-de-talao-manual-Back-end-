namespace DSIN.Business.DTOs;

public sealed class CreateTicketBookDto
{
    public Guid AgentId { get; set; }
    public string ImageBase64 { get; set; } = default!;
}