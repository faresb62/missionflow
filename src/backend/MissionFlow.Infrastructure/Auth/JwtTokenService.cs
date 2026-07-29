using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MissionFlow.Application.Common.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MissionFlow.Infrastructure.Auth;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccessToken(Guid userId, string email, string role, string preferredLanguage)
    {
        var secret = _configuration["Jwt:Secret"]!;
        var issuer = _configuration["Jwt:Issuer"]!;
        var audience = _configuration["Jwt:Audience"]!;
        var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "1440");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim("preferred_language", preferredLanguage),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    public (Guid userId, string email, string role) ValidateToken(string token)
    {
        return ValidateTokenInternal(token, true);
    }

    public (Guid userId, string email, string role) ValidateExpiredToken(string token)
    {
        return ValidateTokenInternal(token, false);
    }

    public DateTime GetTokenExpiration(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        return jwt.ValidTo;
    }

    private (Guid userId, string email, string role) ValidateTokenInternal(string token, bool validateLifetime)
    {
        var secret = _configuration["Jwt:Secret"]!;
        var issuer = _configuration["Jwt:Issuer"]!;
        var audience = _configuration["Jwt:Audience"]!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        var handler = new JwtSecurityTokenHandler();
        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = validateLifetime,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.Zero
        };

        var principal = handler.ValidateToken(token, parameters, out _);

        var userId = Guid.Parse(principal.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        var email = principal.FindFirstValue(JwtRegisteredClaimNames.Email)!;
        var role = principal.FindFirstValue(ClaimTypes.Role)!;

        return (userId, email, role);
    }
}
