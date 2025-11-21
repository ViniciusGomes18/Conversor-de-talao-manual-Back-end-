using static BCrypt.Net.BCrypt;
using DSIN.Business.DTOs;
using DSIN.Business.Interfaces.IRepositories;
using DSIN.Business.Interfaces.IServices;
using DSIN.Business.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DSIN.Business.Services
{
    public sealed class AgentService : IAgentService
    {
        private readonly IAgentRepository _agents;
        private readonly IConfiguration _config;

        public AgentService(IAgentRepository agents, IConfiguration config)
        {
            _agents = agents;
            _config = config;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken)
        {
            var agent = await _agents.GetByEmailAsync(request.Email, cancellationToken);
            if (agent is null) return null;

            if (!Verify(request.Password, agent.PasswordHash))
                return null;

            var now = DateTime.UtcNow;
            var token = CreateJwt(agent, now, out var exp);

            return new LoginResponseDto
            {
                AccessToken = token,
                ExpiresInSeconds = (int)(exp - now).TotalSeconds,
                Agent = new { agent.Id, agent.Name, agent.Email }
            };
        }

        private string CreateJwt(Agent agent, DateTime nowUtc, out DateTime expiresUtc)
        {
            var issuer = _config["Jwt:Issuer"] ?? "DSIN.Api";
            var audience = _config["Jwt:Audience"] ?? "DSIN.Client";
            var keyStr = _config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key not configured");
            var minutes = int.TryParse(_config["Jwt:ExpiresMinutes"], out var m) ? m : 60;

            expiresUtc = nowUtc.AddMinutes(minutes);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, agent.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, agent.Id.ToString()),
                new Claim(ClaimTypes.Name, agent.Name),
                new Claim(ClaimTypes.Email, agent.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: nowUtc,
                expires: expiresUtc,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}