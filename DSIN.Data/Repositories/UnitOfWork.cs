using DSIN.Business.Interfaces.IRepositories;
using DSIN.Data;
using DSIN.Data.Contexts;

namespace DSIN.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly TicketingDbContext _db;
    public UnitOfWork(TicketingDbContext db) => _db = db;
    public Task<int> SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
}