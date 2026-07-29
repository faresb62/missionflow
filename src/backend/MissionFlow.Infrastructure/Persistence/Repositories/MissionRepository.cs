using Microsoft.EntityFrameworkCore;
using MissionFlow.Domain.Entities;
using MissionFlow.Domain.Interfaces;

namespace MissionFlow.Infrastructure.Persistence.Repositories;

public sealed class MissionRepository : RepositoryBase<Mission>, IMissionRepository
{
    public MissionRepository(MissionFlowDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Mission>> GetByRequesterIdAsync(Guid requesterId, CancellationToken ct = default)
        => await DbSet.Where(m => m.RequesterId == requesterId).Include(m => m.Requester).OrderByDescending(m => m.CreatedAt).ToListAsync(ct);

    public async Task<IReadOnlyList<Mission>> GetByStatusAsync(string status, CancellationToken ct = default)
        => await DbSet.Where(m => m.Status == Enum.Parse<Domain.Enums.MissionStatus>(status)).Include(m => m.Requester).OrderByDescending(m => m.CreatedAt).ToListAsync(ct);

    public async Task<Mission?> GetWithExpensesAsync(Guid id, CancellationToken ct = default)
        => await DbSet.Include(m => m.Expenses).Include(m => m.Requester).FirstOrDefaultAsync(m => m.Id == id, ct);

    public async Task<IReadOnlyList<Mission>> GetPagedFilteredAsync(int page, int pageSize, string? status = null, Guid? requesterId = null, CancellationToken ct = default)
    {
        var query = DbSet.Include(m => m.Requester).AsQueryable();
        if (!string.IsNullOrWhiteSpace(status)) query = query.Where(m => m.Status == Enum.Parse<Domain.Enums.MissionStatus>(status));
        if (requesterId.HasValue) query = query.Where(m => m.RequesterId == requesterId.Value);
        return await query.OrderByDescending(m => m.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
    }
}
