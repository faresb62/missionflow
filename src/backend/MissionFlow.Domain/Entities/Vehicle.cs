namespace MissionFlow.Domain;

/// <summary>
/// Represents a vehicle in the company fleet.
/// </summary>
public sealed class Vehicle : Entity
{
    public string Brand { get; private set; }
    public string Model { get; private set; }
    public string RegistrationNumber { get; private set; }
    public VehicleType Type { get; private set; }
    public int? Year { get; private set; }
    public bool IsAvailable { get; private set; }
    public decimal? MileageKm { get; private set; }
    public string? Notes { get; private set; }

    private Vehicle() { } // EF Core

    public Vehicle(string brand, string model, string regNumber, VehicleType type, int? year = null, decimal? mileageKm = null)
    {
        SetInfo(brand, model, regNumber, type, year, mileageKm);
        IsAvailable = true;
    }

    public void SetInfo(string brand, string model, string regNumber, VehicleType type, int? year, decimal? mileageKm)
    {
        if (string.IsNullOrWhiteSpace(brand)) throw new ArgumentException(nameof(brand));
        if (string.IsNullOrWhiteSpace(model)) throw new ArgumentException(nameof(model));
        if (string.IsNullOrWhiteSpace(regNumber)) throw new ArgumentException(nameof(regNumber));

        Brand = brand.Trim();
        Model = model.Trim();
        RegistrationNumber = regNumber.Trim();
        Type = type;
        Year = year;
        MileageKm = mileageKm;
        SetUpdated();
    }

    public void MarkUnavailable() { IsAvailable = false; SetUpdated(); }
    public void MarkAvailable() { IsAvailable = true; SetUpdated(); }
}
