using DSIN.Business.Interfaces.IRepositories;
using DSIN.Business.Models;
using DSIN.Data;
using DSIN.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DSIN.Data.Repositories;

public class DriverRepository : IDriverRepository
{
    private readonly TicketingDbContext _db;
    public DriverRepository(TicketingDbContext db) => _db = db;

    public Task<Driver?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Drivers.FirstOrDefaultAsync(d => d.Id == id, ct);

    public Task<Driver?> GetByCpfAsync(string cpf, CancellationToken ct)
        => _db.Drivers.FirstOrDefaultAsync(d => d.CPF == cpf, ct);

    public Task AddAsync(Driver driver, CancellationToken ct)
        => _db.Drivers.AddAsync(driver, ct).AsTask();
}