using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MissionFlow.Application.Common.Interfaces;
using MissionFlow.Domain.Interfaces;
using MissionFlow.Infrastructure.Auth;
using MissionFlow.Infrastructure.Integration;
using MissionFlow.Infrastructure.Persistence;
using MissionFlow.Infrastructure.Persistence.Repositories;
using MissionFlow.Infrastructure.Services;

namespace MissionFlow.Infrastructure;

/// <summary>
/// Dependency injection registration for the Infrastructure layer.
/// Registers DB context, repositories, auth services, and integrations.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        services.AddDbContext<MissionFlowDbContext>(options =>
            options.UseNpqsql(
                configuration.GetConnectionString("DefaultConnection"),
                npqsqlOptions =>
                {
                    npqsqlOptions.MigrationsAssembly(typeof(MissionFlowDbContext).Assembly.FullName);
                    npqsqlOptions.EnableRetryOnFailure(3);
                }));

        // Repositories
        services.AddScoped<IMissionRepository, MissionRepository>();
        services.AddScoped<IExpenseRepository, ExpenseRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Auth
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        // Services
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        // Integration (future HR Force)
        services.AddSingleton<IEmployeeSyncService, NoOpEmployeeSyncService>();

        // Email notifications (no-op in dev, replace for production)
        services.AddSingleton<IEmailNotificationService, NoOpEmailNotificationService>();

        // Export / Reporting (no-op in dev, replace with EPPlus/QuestPFF for production)
        services.AddSingleton<IExportService, NoOpExportService>();

        return services;
    }
}
