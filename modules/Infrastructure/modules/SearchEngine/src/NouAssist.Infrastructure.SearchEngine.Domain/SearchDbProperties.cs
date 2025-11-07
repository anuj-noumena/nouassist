namespace NouAssist.Infrastructure.SearchEngine;

public static class SearchDbProperties
{
    public static string DbTablePrefix { get; set; } = "Search";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Search";
}
