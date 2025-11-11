namespace NouAssist.AssetManagement;

public static class AssetManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = "AssetManagement";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "AssetManagement";
}
