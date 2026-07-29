using MissionFlow.Application.Common.Interfaces;

namespace MissionFlow.Infrastructure.Services;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
