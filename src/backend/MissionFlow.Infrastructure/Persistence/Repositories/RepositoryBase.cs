using Microsoft.EntityFrameworkCore;
using MissionFlow.Domain;
using MissionFlow.Domain.Interfaces;

namespace MissionFlow.Infrastructure.Persistence.Repositories;

public abstract class RepositoryBase<T> : IRepository<T> where T : Entity
{
    protected readonly MissionFlowDbContext Context;
    protected readonly DbSet<T> DbSet;

    protected RepositoryBase(MissionFlowDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await DbSet.FindAsync([id], ct);

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default)
        => await DbSet.ToListAsync(ct);

    public virtual async Task<IReadOnlyList<T>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
        => await DbSet.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);

    public virtual async Task<int> CountAsync(CancellationToken ct = default)
        => await DbSet.CountAsync(ct);

    public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
        => await DbSet.AnyAsync(e => e.Id == id, ct);

    public virtual async Task AddAsync(T entity, CancellationToken ct = default)
        => await DbSet.AddAsync(entity, ct);

    public virtual Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
        => DbSet.AddRangeAsync(entities, ct);

    public virtual void Update(T entity)
        => DbSet.Update(entity);

    public virtual void Delete(T entity)
        => DbSet.Remove(entity);

    public virtual void DeleteRange(IEnumerable<T> entities)
        => DbSet.RemoveRange(entities);
}
