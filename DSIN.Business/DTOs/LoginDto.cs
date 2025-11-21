namespace DSIN.Business.DTOs
{
    public sealed class LoginRequestDto
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }

    public sealed class LoginResponseDto
    {
        public string AccessToken { get; set; } = default!;
        public int ExpiresInSeconds { get; set; }
        public object Agent { get; set; } = default!;
    }
}