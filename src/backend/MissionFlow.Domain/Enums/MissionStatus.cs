namespace MissionFlow.Domain.Enums;

/// <summary>
/// Status of a mission order.
/// </summary>
public enum MissionStatus
{
    Draft = 1,
    Submitted = 2,
    ApprovedByManager = 3,
    ApprovedByHR = 4,
    ApprovedByFinance = 5,
    Rejected = 6,
    InProgress = 7,
    Completed = 8,
    Cancelled = 9
}
