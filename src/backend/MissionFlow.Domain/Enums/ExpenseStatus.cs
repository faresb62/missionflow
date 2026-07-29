namespace MissionFlow.Domain.Enums;

/// <summary>
/// Status of an expense report.
/// </summary>
public enum ExpenseStatus
{
    Draft = 1,
    Submitted = 2,
    ApprovedByManager = 3,
    ApprovedByFinance = 4,
    Rejected = 5,
    Reimbursed = 6,
    Cancelled = 7
}
