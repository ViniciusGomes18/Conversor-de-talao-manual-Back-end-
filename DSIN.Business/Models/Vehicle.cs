namespace DSIN.Business.Models;

public sealed class Vehicle
{
    public Guid Id { get; private set; }
    public string Plate { get; private set; } = default!;
    public string Color { get; private set; } = default!;
    public string Model { get; private set; } = default!;

    private Vehicle() { } // EF
    public Vehicle(Guid id, string plate, string color, string model)
    {
        Id = id; Plate = plate; Color = color; Model = model;
    }
}