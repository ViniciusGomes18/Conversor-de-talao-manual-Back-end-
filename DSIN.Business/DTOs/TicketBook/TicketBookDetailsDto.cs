namespace DSIN.Business.DTOs;

public sealed class TicketBookDetailsDto
{
    public Guid Id { get; set; }

    public Guid AgentId { get; set; }
    public Guid VehicleId { get; set; }
    public Guid? DriverId { get; set; }

    public string Plate { get; set; } = default!;
    public string VehicleModel { get; set; } = default!;
    public string VehicleColor { get; set; } = default!;
    public string? DriverName { get; set; }
    public string? DriverCpf { get; set; }

    public string ViolationCode { get; set; } = default!;
    public string ViolationDescription { get; set; } = default!;
    public DateTimeOffset OccurredAt { get; set; }
    public string? Location { get; set; }

    public string? TicketImageBase64 { get; set; }
}
