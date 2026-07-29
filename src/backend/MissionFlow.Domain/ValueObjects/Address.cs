using System.Text.RegularExpressions;

namespace MissionFlow.Domain.ValueObjects;

/// <summary>
/// Represents an Algerian address value object.
/// </summary>
public sealed class Address : ValueObject
{
    public string Street { get; }
    public string? Complement { get; }
    public string City { get; }
    public string Wilaya { get; }
    public string? PostalCode { get; }
    public string Country { get; }

    public Address(string street, string city, string wilaya, string? complement = null, string? postalCode = null, string country = "Algérie")
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street is required", nameof(street));
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City is required", nameof(city));
        if (string.IsNullOrWhiteSpace(wilaya))
            throw new ArgumentException("Wilaya is required", nameof(wilaya));

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
        => $"{Street}{(Complement is not null ? $", {Complement}" : "")}, {City}, {Wilaya}{(PostalCode is not null ? $" {PostalCode}" : "")}, {Country}";
}