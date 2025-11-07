namespace NouAssist.CoreProductivity.GoalTracking;

public static class GoalTrackingDbProperties
{
    public static string DbTablePrefix { get; set; } = "GoalTracking";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "GoalTracking";
}
