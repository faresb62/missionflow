using Microsoft.EntityFrameworkCore;
using MissionFlow.Domain.Entities;
using MissionFlow.Domain.Interfaces;

namespace MissionFlow.Infrastructure.Persistence.Repositories;

public sealed class ExpenseRepository : RepositoryBase<Expense>, IExpenseRepository
{
    public ExpenseRepository(MissionFlowDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Expense>> GetByMissionIdAsync(Guid missionId, CancellationToken ct = default)
        => await DbSet.Where(e => e.MissionId == missionId).Include(e => e.SubmittedBy).OrderBy(e => e.ExpenseDate).ToListAsync(ct);

    public async Task<IReadOnlyList<Expense>> GetBySubmitterIdAsync(Guid submitterId, CancellationToken ct = default)
        => await DbSet.Where(e => e.SubmittedById == submitterId).Include(e => e.Mission).OrderByDescending(e => e.ExpenseDate).ToListAsync(ct);
}
