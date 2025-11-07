namespace NouAssist.CoreProductivity.TimeManagement;

public static class TimeManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = "TimeManagement";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "TimeManagement";
}
