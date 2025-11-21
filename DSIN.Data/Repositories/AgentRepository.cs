using DSIN.Business.Interfaces.IRepositories;
using DSIN.Business.Models;
using DSIN.Data;
using DSIN.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DSIN.Data.Repositories;

public class AgentRepository : IAgentRepository
{
    private readonly TicketingDbContext _db;
    public AgentRepository(TicketingDbContext db) => _db = db;

    public Task<Agent?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Agents.FirstOrDefaultAsync(a => a.Id == id, ct);

    public Task<Agent?> GetByEmailAsync(string email, CancellationToken ct)
        => _db.Agents.FirstOrDefaultAsync(a => a.Email == email, ct);

    public Task AddAsync(Agent agent, CancellationToken ct)
        => _db.Agents.AddAsync(agent, ct).AsTask();
}