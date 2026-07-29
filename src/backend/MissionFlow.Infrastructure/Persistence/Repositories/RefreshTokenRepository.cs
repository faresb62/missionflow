using Microsoft.EntityFrameworkCore;
using MissionFlow.Domain.Entities;
using MissionFlow.Domain.Interfaces;

namespace MissionFlow.Infrastructure.Persistence.Repositories;

public sealed class RefreshTokenRepository : RepositoryBase<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(MissionFlowDbContext context) : base(context) { }

    public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken ct = default)
        => await DbSet.FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash, ct);

    public async Task RevokeAllForUserAsync(Guid userId, CancellationToken ct = default)
    {
        var tokens = await DbSet.Where(rt => rt.UserId == userId && !rt.IsRevoked).ToListAsync(ct);
        foreach (var token in tokens) token.Revoke("Manual logout");
    }
}
