namespace MissionFlow.Domain.Events;

/// <<summary>
/// Raised when a new expense is added to a mission.
/// </summary>
public sealed record ExpenseAddedDomainEvent(Entities.Expense Expense) : IDomainEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}

/// <summary>
/// Raised when an expense is approved.
/// </summary>
public sealed record ExpenseApprovedDomainEvent(Entities.Expense Expense) : IDomainEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}

/// <summary>
/// Raised when an expense is rejected.
/// </summary>
public sealed record ExpenseRejectedDomainEvent(Entities.Expense Expense, string Reason) : IDomainEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}

/// <summary>
/// Raised when an expense is reimbursed.
/// </summary>
public sealed record ExpenseReimbursedDomainEvent(Entities.Expense Expense) : IDomainEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}
