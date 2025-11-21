using DSIN.Business.DTOs;
using DSIN.Business.Interfaces.IRepositories;
using DSIN.Business.Interfaces.IServices;

namespace DSIN.Business.Services;

public sealed class TicketBookService : ITicketBookService
{
    private readonly IOcrClient _ocr;
    private readonly ITicketBookRepository _tickets;

    public TicketBookService(IOcrClient ocr, ITicketBookRepository tickets)
    {
        _ocr = ocr;
        _tickets = tickets;
    }

    public async Task<TicketBookResponseDto> AnalyzeAsync(Guid agentId, string imageBase64, CancellationToken ct)
    {
        if (agentId == Guid.Empty) throw new ArgumentException("AgentId inválido.", nameof(agentId));
        if (string.IsNullOrWhiteSpace(imageBase64)) throw new ArgumentException("ImageBase64 é obrigatório.", nameof(imageBase64));

        var ocr = await _ocr.AnalyzeAsync(new OcrExternalRequestDto { ImageBase64 = imageBase64 }, ct);
        return new TicketBookResponseDto
        {
            AgentId = agentId,
            Plate = ocr.Plate,
            VehicleModel = ocr.VehicleModel,
            VehicleColor = ocr.VehicleColor,
            ViolationCode = ocr.ViolationCode,
            ViolationDescription = ocr.ViolationDescription,
            OccurredAt = ocr.OccurredAt,
            Location = ocr.Location,
            DriverName = ocr.DriverName,
            DriverCpf = ocr.DriverCpf
        };
    }

    public async Task<PagedResult<TicketBookSummaryDto>> ListByAgentAsync(Guid agentId, int skip, int take, CancellationToken ct)
    {
        if (agentId == Guid.Empty) throw new ArgumentException("AgentId inválido.", nameof(agentId));
        if (take <= 0) take = 50;
        if (take > 200) take = 200;

        var total = await _tickets.CountByAgentAsync(agentId, ct);
        var items = await _tickets.ListByAgentAsync(agentId, skip, take, ct);

        var mapped = items.Select(t => new TicketBookSummaryDto
        {
            Id = t.Id,
            AgentId = t.AgentId,
            VehicleId = t.VehicleId,
            DriverId = t.DriverId,

            Plate = t.PlateSnapshot,
            VehicleModel = t.VehicleModelSnapshot,
            VehicleColor = t.VehicleColorSnapshot,
            DriverName = t.DriverNameSnapshot,
            DriverCpf = t.DriverCpfSnapshot,

            ViolationCode = t.ViolationCode,
            ViolationDescription = t.ViolationDescription,
            OccurredAt = t.OccurredAt,
            Location = t.Location
        }).ToList();

        return new PagedResult<TicketBookSummaryDto>
        {
            Total = total,
            Items = mapped
        };
    }
}