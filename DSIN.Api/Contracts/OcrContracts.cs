namespace DSIN.Api.Contracts;

public sealed class OcrAnalyzeRequest
{
    public Guid AgentId { get; set; }
    public string ImageBase64 { get; set; } = default!;
}

public sealed class OcrAnalyzeResponse
{
    public string Status { get; set; } = "OK";
    public Guid TicketId { get; set; }
}