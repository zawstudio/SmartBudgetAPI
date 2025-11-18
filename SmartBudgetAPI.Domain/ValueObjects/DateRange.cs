namespace SmartBudgetAPI.Domain.ValueObjects;

/// <summary>
/// Value Object representing a date range
/// </summary>
public class DateRange : IEquatable<DateRange>
{
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    private DateRange() { }

    public DateRange(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
            throw new ArgumentException("Start date cannot be after end date");

        StartDate = startDate;
        EndDate = endDate;
    }

    public int DurationInDays => (EndDate - StartDate).Days + 1;

    public bool Contains(DateTime date)
    {
        return date >= StartDate && date <= EndDate;
    }

    public bool Overlaps(DateRange other)
    {
        return StartDate <= other.EndDate && EndDate >= other.StartDate;
    }

    public bool Equals(DateRange? other)
    {
        if (other is null) return false;
        return StartDate == other.StartDate && EndDate == other.EndDate;
    }

    public override bool Equals(object? obj)
    {
        return obj is DateRange dateRange && Equals(dateRange);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StartDate, EndDate);
    }

    public static bool operator ==(DateRange? left, DateRange? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(DateRange? left, DateRange? right)
    {
        return !Equals(left, right);
    }

    public override string ToString()
    {
        return $"{StartDate:yyyy-MM-dd} - {EndDate:yyyy-MM-dd}";
    }
}

