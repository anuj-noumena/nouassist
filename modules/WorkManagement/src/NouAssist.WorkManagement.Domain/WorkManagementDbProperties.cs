namespace NouAssist.WorkManagement;

public static class WorkManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = "WorkManagement";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "WorkManagement";
}
