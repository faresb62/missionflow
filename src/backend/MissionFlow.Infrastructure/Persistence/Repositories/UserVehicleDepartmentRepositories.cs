using Microsoft.EntityFrameworkCore;
using MissionFlow.Domain.Entities;
using MissionFlow.Domain.Interfaces;

namespace MissionFlow.Infrastructure.Persistence.Repositories;

public sealed class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(MissionFlowDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        => await DbSet.FirstOrDefaultAsync(u => u.Email.Value == email.ToLowerInvariant(), ct);

    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken ct = default)
        => !await DbSet.AnyAsync(u => u.Email.Value == email.ToLowerInvariant(), ct);

    public async Task<IReadOnlyList<User>> GetByRoleAsync(string role, CancellationToken ct = default)
        => await DbSet.Where(u => u.Role == Enum.Parse<Domain.Enums.UserRole>(role)).OrderBy(u => u.LastName).ToListAsync(ct);
}

public sealed class VehicleRepository : RepositoryBase<Vehicle>, IVehicleRepository
{
    public VehicleRepository(MissionFlowDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Vehicle>> GetAvailableAsync(CancellationToken ct = default)
        => await DbSet.Where(v => v.IsAvailable).OrderBy(v => v.Brand).ThenBy(v => v.Model).ToListAsync(ct);
}

public sealed class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
{
    public DepartmentRepository(MissionFlowDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Department>> GetAllActiveAsync(CancellationToken ct = default)
        => await DbSet.Where(d => d.IsActive).Include(d => d.ParentDepartment).OrderBy(d => d.Name).ToListAsync(ct);
}
