namespace MissionFlow.Domain.Enums;

/// <summary>
/// User roles within the MissionFlow platform.
/// Only Administrator is initially used (V1).
/// Architecture supports future HR Force integration.
/// </summary>
public enum UserRole
{
    Administrator = 1,
    HR = 2,
    Finance = 3,
    Manager = 4,
    Employee = 5,
    IT = 6,
    Director = 7
}
