namespace MissionFlow.Application;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Dependency injection registration for the Application layer.
/// Registers MediatR, FluentValidation, and AutoMapper.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        // FluentValidation
        services.AddValidatorsFromAssembly(assembly);

        // AutoMapper
        services.AddAutoMapper(assembly);

        return services;
    }
}
