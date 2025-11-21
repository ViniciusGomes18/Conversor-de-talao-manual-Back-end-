using DSIN.Business.DTOs;

namespace DSIN.Business.Interfaces.IServices;

public interface ITicketBookService
{
    Task<TicketBookResponseDto> AnalyzeAsync(Guid agentId, string imageBase64, CancellationToken ct);
    Task<PagedResult<TicketBookSummaryDto>> ListByAgentAsync(Guid agentId, int skip, int take, CancellationToken ct);
}