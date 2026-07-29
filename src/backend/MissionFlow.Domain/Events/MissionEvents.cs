namespace MissionFlow.Domain.Events;

/// <<summary>
/// Raised when a new mission is created.
/// </summary>
public sealed record MissionCreatedDomainEvent(Entities.Mission Mission) : IDomainEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}

/// <summary>
/// Raised when a mission is submitted for approval.
/// </summary>
public sealed record MissionSubmittedDomainEvent(Entities.Mission Mission) : IDomainEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}

/// <summary>
/// Raised when a mission is fully approved (Manager → HR → Finance).
/// </summary>
public sealed record MissionFullyApprovedDomainEvent(Entities.Mission Mission) : IDomainEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}

/// <summary>
/// Raised when a mission is rejected.
/// </summary>
public sealed record MissionRejectedDomainEvent(Entities.Mission Mission, string Reason) : IDomainEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}

/// <<summary>
/// Raised when a mission is marked as completed.
/// </summary>
public sealed record MissionCompletedDomainEvent(Entities.Mission Mission) : IDomainEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}

/// <<summary>
/// Raised when a mission is cancelled.
/// </summary>
public sealed record MissionCancelledDomainEvent(Entities.Mission Mission, string Reason) : IDomainEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}
