namespace MissionFlow.Domain.ValueObjects;

/// <summary>
/// Represents a range of dates for a mission or travel period.
/// </summary>
public sealed class DateRange : ValueObject
{
    public DateOnly StartDate { get; }
    public DateOnly EndDate { get; }

    public DateRange(DateOnly startDate, DateOnly endDate)
    {
        if (endDate < startDate)
            throw new ArgumentException("End date must be on or after start date", nameof(endDate));

        StartDate = startDate;
        EndDate = endDate;
    }

    public int DurationInDays => EndDate.DayNumber - StartDate.DayNumber + 1;

    public bool Overlaps(DateRange other)
    {
        return StartDate <= other.EndDate && EndDate >= other.StartDate;
    }

    public bool Contains(DateOnly date)
    {
        return date >= StartDate && date <= EndDate;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StartDate;
        yield return EndDate;
    }

    public override string ToString() => $'{StartDate:dd/MM/yyyy} - {EndDate:dd/MM/yyyy}';
}
