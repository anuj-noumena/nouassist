namespace NouAssist.CollaborationHub;

public static class CollaborationHubDbProperties
{
    public static string DbTablePrefix { get; set; } = "CollaborationHub";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "CollaborationHub";
}
