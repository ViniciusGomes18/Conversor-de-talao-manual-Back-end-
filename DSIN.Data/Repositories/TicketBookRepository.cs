using DSIN.Business.Interfaces.IRepositories;
using DSIN.Business.Models;
using DSIN.Data;
using DSIN.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DSIN.Data.Repositories;

public sealed class TicketBookRepository : ITicketBookRepository
{
    private readonly TicketingDbContext _db;
    public TicketBookRepository(TicketingDbContext db) => _db = db;

    public async Task<TicketBook?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _db.Set<TicketBook>().AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task AddAsync(TicketBook ticket, CancellationToken ct)
        => await _db.Set<TicketBook>().AddAsync(ticket, ct);

    public async Task<IReadOnlyList<TicketBook>> ListAsync(int skip, int take, CancellationToken ct)
        => await _db.Set<TicketBook>()
            .AsNoTracking()
            .OrderByDescending(t => t.OccurredAt)
            .Skip(skip).Take(take)
            .ToListAsync(ct);

    public async Task<int> CountByAgentAsync(Guid agentId, CancellationToken ct)
        => await _db.Set<TicketBook>().AsNoTracking().CountAsync(t => t.AgentId == agentId, ct);

    public async Task<IReadOnlyList<TicketBook>> ListByAgentAsync(Guid agentId, int skip, int take, CancellationToken ct)
        => await _db.Set<TicketBook>()
            .AsNoTracking()
            .Where(t => t.AgentId == agentId)
            .OrderByDescending(t => t.OccurredAt)
            .Skip(skip).Take(take)
            .ToListAsync(ct);
}