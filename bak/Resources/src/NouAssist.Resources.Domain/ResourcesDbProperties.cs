namespace NouAssist.Resources;

public static class ResourcesDbProperties
{
    public static string DbTablePrefix { get; set; } = "Resources";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Resources";
}
