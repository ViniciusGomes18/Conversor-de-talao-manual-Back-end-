using DSIN.Business.Interfaces.IRepositories;
using DSIN.Business.Models;
using DSIN.Data;
using DSIN.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DSIN.Data.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly TicketingDbContext _db;
    public VehicleRepository(TicketingDbContext db) => _db = db;

    public Task<Vehicle?> GetByPlateAsync(string plate, CancellationToken ct)
        => _db.Vehicles.FirstOrDefaultAsync(v => v.Plate == plate, ct);

    public Task AddAsync(Vehicle vehicle, CancellationToken ct)
        => _db.Vehicles.AddAsync(vehicle, ct).AsTask();
}