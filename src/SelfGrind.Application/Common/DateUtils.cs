namespace SelfGrind.Application.Common;

public static class DateUtils
{
    /// <summary>Today's date in server-local time — matches how task occurrences are scheduled.</summary>
    public static DateOnly LocalToday => DateOnly.FromDateTime(DateTime.Today);

    /// <summary>Today's date in UTC — used for completion timestamps and weekly leaderboard windows.</summary>
    public static DateOnly UtcToday => DateOnly.FromDateTime(DateTime.UtcNow);

    public static DateOnly GetMondayOf(DateOnly date)
    {
        var diff = ((int)date.DayOfWeek + 6) % 7;
        return date.AddDays(-diff);
    }
}
