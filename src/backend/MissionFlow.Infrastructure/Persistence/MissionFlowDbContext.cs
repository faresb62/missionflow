using Microsoft.EntityFrameworkCore;
using MissionFlow.Domain.Entities;

namespace MissionFlow.Infrastructure.Persistence;

public sealed class MissionFlowDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Mission> Missions => Set<Mission>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<Approval> Approvals => Set<Approval>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public MissionFlowDbContext(DbContextOptions<MissionFlowDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MissionFlowDbContext).Assembly);
    }
}
