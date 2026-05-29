namespace SelfGrind.Application.Analytics;

internal static class StreakUtil
{
    public static int LongestRun(DateOnly[] sortedAscendingDates)
    {
        if (sortedAscendingDates.Length == 0) return 0;

        var longest = 1;
        var current = 1;

        for (var i = 1; i < sortedAscendingDates.Length; i++)
        {
            if (sortedAscendingDates[i].DayNumber == sortedAscendingDates[i - 1].DayNumber + 1)
            {
                current++;
                if (current > longest) longest = current;
            }
            else
            {
                current = 1;
            }
        }

        return longest;
    }

    public static int CurrentRunEndingAt(DateOnly[] sortedAscendingDates, DateOnly today)
    {
        if (sortedAscendingDates.Length == 0) return 0;

        var set = new HashSet<DateOnly>(sortedAscendingDates);

        var streak = 0;
        var cursor = today;

        // Allow streak to start from today OR yesterday if today has no activity yet.
        if (!set.Contains(cursor))
        {
            cursor = cursor.AddDays(-1);
            if (!set.Contains(cursor)) return 0;
        }

        while (set.Contains(cursor))
        {
            streak++;
            cursor = cursor.AddDays(-1);
        }

        return streak;
    }
}
