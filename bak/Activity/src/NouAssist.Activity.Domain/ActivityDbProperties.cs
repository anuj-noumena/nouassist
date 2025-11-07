namespace NouAssist.Activity;

public static class ActivityDbProperties
{
    public static string DbTablePrefix { get; set; } = "Activity";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Activity";
}
