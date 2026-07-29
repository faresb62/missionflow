using Microsoft.Extensions.Logging;
using MissionFlow.Application.Common.Interfaces;

namespace MissionFlow.Infrastructure.Integration;

public sealed class NoOpEmployeeSyncService : IEmployeeSyncService
{
    private readonly ILogger<NoOpEmployeeSyncService> _logger;

    public NoOpEmployeeSyncService(ILogger<NoOpEmployeeSyncService> logger)
    {
        _logger = logger;
    }

    public Task SyncEmployeesAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("[HR FORCE] Employee sync would run here (future integration)");
        return Task.CompletedTask;
    }
}
