namespace DSIN.Business.Models;

public sealed class TicketBook
{
    public Guid Id { get; private set; }
    public Guid AgentId { get; private set; }
    public Guid VehicleId { get; private set; }
    public Guid? DriverId { get; private set; }   
    public string PlateSnapshot { get; private set; } = default!;
    public string VehicleModelSnapshot { get; private set; } = default!;
    public string VehicleColorSnapshot { get; private set; } = default!;
    public string? DriverNameSnapshot { get; private set; }
    public string? DriverCpfSnapshot { get; private set; }
    public string ViolationCode { get; private set; } = default!;
    public string ViolationDescription { get; private set; } = default!;
    public DateTimeOffset OccurredAt { get; private set; }
    public string? Location { get; private set; }
    public Agent? Agent { get; private set; }
    public Vehicle? Vehicle { get; private set; }
    public Driver? Driver { get; private set; }
    public string? TicketImageBase64 { get; private set; }

    private TicketBook() { }

    public TicketBook(
        Guid id, Guid agentId, Guid vehicleId, Guid? driverId,
        string plateSnapshot, string vehicleModelSnapshot, string vehicleColorSnapshot,
        string? driverNameSnapshot, string? driverCpfSnapshot,
        string violationCode, string violationDescription,
        DateTimeOffset occurredAt, string? location,
        string? ticketImageBase64)
    {
        Id = id;
        AgentId = agentId;
        VehicleId = vehicleId;
        DriverId = driverId;
        PlateSnapshot = plateSnapshot;
        VehicleModelSnapshot = vehicleModelSnapshot;
        VehicleColorSnapshot = vehicleColorSnapshot;
        DriverNameSnapshot = driverNameSnapshot;
        DriverCpfSnapshot = driverCpfSnapshot;
        ViolationCode = violationCode;
        ViolationDescription = violationDescription;
        OccurredAt = occurredAt;
        Location = location;
        TicketImageBase64 = ticketImageBase64;
<<<<<<< HEAD

=======
>>>>>>> ace0807f5233eabb80d93694e2a95591bd775ee4
    }
}
