using Microsoft.AspNetCore.Http;
using MissionFlow.Application.Common.Interfaces;
using System.Security.Claims;

namespace MissionFlow.Infrastructure.Services;

public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var sub = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return sub is not null && Guid.TryParse(sub, out var id) ? id : null;
        }
    }

    public string? Email =>
        _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

    public string? Role =>
        _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);

    public string? PreferredLanguage =>
        _httpContextAccessor.HttpContext?.User.FindFirstValue("preferred_language");

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}
