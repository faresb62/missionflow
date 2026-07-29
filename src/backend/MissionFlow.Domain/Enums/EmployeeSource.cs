namespace MissionFlow.Domain.Enums;

/// <summary>
/// The source from which employee data originates.
/// Supports future HR Force integration.
/// </summary>
public enum EmployeeSource
{
    Manual = 1,
    HRForce = 2,
    LDAP = 3,
    ActiveDirectory = 4,
    RestApi = 5
}
