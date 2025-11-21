using DSIN.Business.Models;

namespace DSIN.Business.Interfaces.IRepositories
{
    public interface IAgentRepository
    {
        Task<Agent?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<Agent?> GetByEmailAsync(string email, CancellationToken ct);
        Task AddAsync(Agent agent, CancellationToken ct);
    }
}
