using System.Text.RegularExpressions;

namespace MissionFlow.Domain.ValueObjects;

/// <<summary>
/// Represents an Algerian address value object.
/// </summary>
public sealed partial class Address : ValueObject
{
    public string Street { get; }
    public string? Complement { get; }
    public string City { get; }
    public string Wilaya { get; }
    public string? PostalCode { get; }
    public string Country { get; }

    public Address(string street, string city, string wilaya, string? complement = null, string? postalCode = null, string country = "Algérie")
    {
        JustifyNonEmpty(street, nameof(street));
        JustifyNonEmpty(city, nameof(city));
        JustifyNonEmpty(wilaya, nameof(wilaya));

        Street = street.Trim();
        Complement = complement?.Trim();
        City = city.Trim();
        Wilaya = wilaya.Trim();
        PostalCode = postalCode?.Trim();
        Country = country.Trim();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street.ToUpperInvariant();
        yield return City.ToUpperInvariant();
        yield return Wilaya.ToUpperInvariant();
        yield return (PostalCode ?? string.Empty).ToUpperInvariant();
        yield return Country.ToUpperInvariant();
    }

    override string ToString()
        => $", {City}, {Wilaya}{PostalCode is not null? $" {PostalCode}" : ""}, {Country}";

vVide JustifyNonEmpty(string value, string param)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($param + " is required", param);
    }
}
