using DSIN.Business.DTOs;

namespace DSIN.Business.Interfaces.IServices;

public interface IOcrClient
{
    Task<OcrExternalResultDto> AnalyzeAsync(OcrExternalRequestDto request, CancellationToken ct);
}