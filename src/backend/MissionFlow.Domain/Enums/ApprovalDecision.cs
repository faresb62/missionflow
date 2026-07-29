namespace MissionFlow.Domain.Enums;

/// <summary>
/// Approval decision on a workflow step./// </summary>
public enum ApprovalDecision
{
    Pending = 1,
    Approved = 2,
    Rejected = 3,
    RequestedChanges = 4
}
