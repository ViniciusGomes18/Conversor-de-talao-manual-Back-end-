namespace DSIN.Business.Models;

public sealed class Driver
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public string? CPF { get; private set; }

    private Driver() { }
    public Driver(Guid id, string name, string? cpf)
    {
        Id = id; Name = name; CPF = cpf;
    }
}