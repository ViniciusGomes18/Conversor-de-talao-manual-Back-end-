namespace DSIN.Business.DTOs;

public sealed class OcrExternalRequestDto
{
    public string ImageBase64 { get; set; } = default!;
}

public sealed class OcrExternalResultDto
{
    public string Plate { get; set; } = default!;
    public string VehicleModel { get; set; } = default!;
    public string VehicleColor { get; set; } = default!;
    public string ViolationCode { get; set; } = default!;
    public string ViolationDescription { get; set; } = default!;
    public DateTimeOffset OccurredAt { get; set; }
    public string? Location { get; set; }

    public string? DriverName { get; set; }
    public string? DriverCpf { get; set; }
}