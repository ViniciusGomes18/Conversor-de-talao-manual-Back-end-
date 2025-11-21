using DSIN.Business.DTOs;

namespace DSIN.Business.Interfaces.IServices
{

    public interface IAgentService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request, CancellationToken ct);
    }
}
