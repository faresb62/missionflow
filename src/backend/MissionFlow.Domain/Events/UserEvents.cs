namespace MissionFlow.Domain.Events;

/// <<summary>
/// Raised when a new user is registered or synced from HR Force.
/// </summary>
public sealed record UserCreatedDomainEvent(Entities.User User) : IDomainEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}

/// <summary>
/// Raised when a user's role is changed.
/// </summary>
public sealed record UserRoleChangedDomainEvent(Entities.User User, Enums.UserRole OldRole, Enums.UserRole NewRole) : IDomainEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}

/// <summary>
/// Raised when a user is deactivated.
/// </summary>
public sealed record UserDeactivatedDomainEvent(Entities.User User) : IDomainEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}
