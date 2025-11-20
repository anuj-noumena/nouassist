namespace NouAssist.RequirementsManagement;

public static class RequirementsManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = "RequirementsManagement";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "RequirementsManagement";
}
