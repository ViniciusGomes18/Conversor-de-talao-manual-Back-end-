using DSIN.Business.DTOs;
using DSIN.Business.Interfaces.IRepositories;
using DSIN.Business.Interfaces.IServices;
using DSIN.Business.Models;

namespace DSIN.Business.Services;

public sealed class TicketBookService : ITicketBookService
{
    private readonly IOcrClient _ocr;
    private readonly ITicketBookRepository _tickets;
    private readonly IVehicleRepository _vehicles;
    private readonly IDriverRepository _drivers;
    private readonly IUnitOfWork _uow;

    public TicketBookService(
        IOcrClient ocr,
        ITicketBookRepository tickets,
        IVehicleRepository vehicles,
        IDriverRepository drivers,
        IUnitOfWork uow)
    {
        _ocr = ocr;
        _tickets = tickets;
        _vehicles = vehicles;
        _drivers = drivers;
        _uow = uow;
    }

    public async Task<TicketBookResponseDto> AnalyzeAsync(
        Guid agentId,
        string imageBase64,
        CancellationToken ct)
    {
        if (agentId == Guid.Empty)
            throw new ArgumentException("AgentId inválido.", nameof(agentId));
        if (string.IsNullOrWhiteSpace(imageBase64))
            throw new ArgumentException("ImageBase64 é obrigatório.", nameof(imageBase64));

        // 1) Chama o OCR externo
        var ocr = await _ocr.AnalyzeAsync(
            new OcrExternalRequestDto { ImageBase64 = imageBase64 },
            ct);

        // 2) Garante/resolve o veículo pela placa
        if (string.IsNullOrWhiteSpace(ocr.Plate))
            throw new InvalidOperationException("OCR não retornou placa do veículo.");

        var vehicle = await _vehicles.GetByPlateAsync(ocr.Plate, ct);
        if (vehicle is null)
        {
            var model = string.IsNullOrWhiteSpace(ocr.VehicleModel)
                ? "Desconhecido"
                : ocr.VehicleModel;

            var color = string.IsNullOrWhiteSpace(ocr.VehicleColor)
                ? "Desconhecido"
                : ocr.VehicleColor;

            vehicle = new Vehicle(
                id: Guid.NewGuid(),
                plate: ocr.Plate,
                color: color,
                model: model
            );

            await _vehicles.AddAsync(vehicle, ct);
        }

        // 3) Garante/resolve o condutor (se vier do OCR)
        Driver? driver = null;
        if (!string.IsNullOrWhiteSpace(ocr.DriverCpf) ||
            !string.IsNullOrWhiteSpace(ocr.DriverName))
        {
            if (!string.IsNullOrWhiteSpace(ocr.DriverCpf))
            {
                driver = await _drivers.GetByCpfAsync(ocr.DriverCpf, ct);
            }

            if (driver is null && !string.IsNullOrWhiteSpace(ocr.DriverName))
            {
                driver = new Driver(
                    id: Guid.NewGuid(),
                    name: ocr.DriverName!,
                    cpf: ocr.DriverCpf
                );

                await _drivers.AddAsync(driver, ct);
            }
        }

        // 4) Cria o TicketBook com snapshots
        var ticket = new TicketBook(
            id: Guid.NewGuid(),
            agentId: agentId,
            vehicleId: vehicle.Id,
            driverId: driver?.Id,
            plateSnapshot: ocr.Plate,
            vehicleModelSnapshot: ocr.VehicleModel,
            vehicleColorSnapshot: ocr.VehicleColor,
            driverNameSnapshot: ocr.DriverName,
            driverCpfSnapshot: ocr.DriverCpf,
            violationCode: ocr.ViolationCode,
            violationDescription: ocr.ViolationDescription,
            occurredAt: ocr.OccurredAt,
            location: ocr.Location
        );

        await _tickets.AddAsync(ticket, ct);
        await _uow.SaveChangesAsync(ct);

        // 5) Retorna DTO para o front (se quiser exibir resumo na hora)
        return new TicketBookResponseDto
        {
            AgentId = agentId,
            Plate = ticket.PlateSnapshot,
            VehicleModel = ticket.VehicleModelSnapshot,
            VehicleColor = ticket.VehicleColorSnapshot,
            ViolationCode = ticket.ViolationCode,
            ViolationDescription = ticket.ViolationDescription,
            OccurredAt = ticket.OccurredAt,
            Location = ticket.Location,
            DriverName = ticket.DriverNameSnapshot,
            DriverCpf = ticket.DriverCpfSnapshot
        };
    }

    public async Task<PagedResult<TicketBookSummaryDto>> ListByAgentAsync(
        Guid agentId,
        int skip,
        int take,
        CancellationToken ct)
    {
        if (agentId == Guid.Empty)
            throw new ArgumentException("AgentId inválido.", nameof(agentId));
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
