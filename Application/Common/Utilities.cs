namespace Application.Common;

public static class Utilities
{
    public static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    public static string NormalizeRequired(string? value, string propertyName)
    {
        return value is null ? throw new ArgumentException($"{propertyName} cannot be null.") : value.Trim();
    }

    public static void ValidateDuration(TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
        {
            throw new ArgumentException("Duration must be greater than zero.");
        }
    }

    public static void ValidateDates(DateTimeOffset startDate, DateTimeOffset endDate)
    {
        if (endDate < startDate)
        {
            throw new ArgumentException("EndDate must be greater than or equal to StartDate.");
        }
    }
}
