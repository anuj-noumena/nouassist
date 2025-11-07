namespace NouAssist.Infrastructure.SearchEngine;

public static class SearchEngineDbProperties
{
    public static string DbTablePrefix { get; set; } = "SearchEngine";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "SearchEngine";
}
