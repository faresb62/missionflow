using Microsoft.Extensions.Logging;

namespace MissionFlow.Infrastructure.Localization;

public sealed class LocalizationService
{
    private readonly ILogger<LocalizationService> _logger;

    public LocalizationService(ILogger<LocalizationService> logger)
    {
        _logger = logger;
    }

    public string GetLocalizedString(string key, string language)
    {
        return $"[{language}] {key}";
    }
}
