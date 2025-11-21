namespace DSIN.Business.Interfaces.IRepositories;

using global::DSIN.Business.Models;

public interface IVehicleRepository
{
    Task<Vehicle?> GetByPlateAsync(string plate, CancellationToken ct);
    Task AddAsync(Vehicle vehicle, CancellationToken ct);
}