namespace NouAssist.ReflectionAndSupport.ActivityTracking;

public static class ActivityTrackingDbProperties
{
    public static string DbTablePrefix { get; set; } = "ActivityTracking";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "ActivityTracking";
}
