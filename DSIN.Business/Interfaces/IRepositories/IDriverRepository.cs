namespace DSIN.Business.Interfaces.IRepositories;

using global::DSIN.Business.Models;

public interface IDriverRepository
{
    Task<Driver?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Driver?> GetByCpfAsync(string cpf, CancellationToken ct);
    Task AddAsync(Driver driver, CancellationToken ct);
}