namespace DSIN.Business.Interfaces.IRepositories;

using global::DSIN.Business.Models;

public interface ITicketBookRepository
{
    Task<TicketBook?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(TicketBook ticket, CancellationToken ct);
    Task<IReadOnlyList<TicketBook>> ListAsync(int skip, int take, CancellationToken ct);
    Task<int> CountByAgentAsync(Guid agentId, CancellationToken ct);
    Task<IReadOnlyList<TicketBook>> ListByAgentAsync(Guid agentId, int skip, int take, CancellationToken ct);
}