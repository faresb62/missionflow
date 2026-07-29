using Microsoft.Extensions.Logging;
using MissionFlow.Application.Common.Interfaces;

namespace MissionFlow.Infrastructure.Integration;

public sealed class NoOpExportService : IExportService
{
    private readonly ILogger<NoOpExportService> _logger;

    public NoOpExportService(ILogger<NoOpExportService> logger)
    {
        _logger = logger;
    }

    public Task<byte[]> ExportMissionToPdfAsync(Guid missionId, CancellationToken ct = default)
    {
        _logger.LogInformation("[EXPORT] PDF export for mission {MissionId} — no-op in dev", missionId);
        return Task.FromResult(Array.Empty<byte>());
    }

    public Task<byte[]> ExportMissionReportAsync(Guid missionId, CancellationToken ct = default)
    {
        _logger.LogInformation("[EXPORT] Report export for mission {MissionId} — no-op in dev", missionId);
        return Task.FromResult(Array.Empty<byte>());
    }
}
