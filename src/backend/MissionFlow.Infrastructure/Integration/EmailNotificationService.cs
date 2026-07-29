using Microsoft.Extensions.Logging;
using MissionFlow.Application.Common.Interfaces;

namespace MissionFlow.Infrastructure.Integration;

public sealed class NoOpEmailNotificationService : IEmailNotificationService
{
    private readonly ILogger<NoOpEmailNotificationService> _logger;

    public NoOpEmailNotificationService(ILogger<NoOpEmailNotificationService> logger)
    {
        _logger = logger;
    }

    public Task SendMissionApprovedNotification(Guid missionId, string requesterEmail, CancellationToken ct = default)
    {
        _logger.LogInformation("[EMAIL] Mission {MissionId} approved — would notify {Email}", missionId, requesterEmail);
        return Task.CompletedTask;
    }

    public Task SendMissionRejectedNotification(Guid missionId, string requesterEmail, string reason, CancellationToken ct = default)
    {
        _logger.LogInformation("[EMAIL] Mission {MissionId} rejected — would notify {Email}", missionId, requesterEmail);
        return Task.CompletedTask;
    }

    public Task SendExpenseApprovedNotification(Guid expenseId, string requesterEmail, CancellationToken ct = default)
    {
        _logger.LogInformation("[EMAIL] Expense {ExpenseId} approved — would notify {Email}", expenseId, requesterEmail);
        return Task.CompletedTask;
    }

    public Task SendExpenseRejectedNotification(Guid expenseId, string requesterEmail, string reason, CancellationToken ct = default)
    {
        _logger.LogInformation("[EMAIL] Expense {ExpenseId} rejected — would notify {Email}", expenseId, requesterEmail);
        return Task.CompletedTask;
    }
}
