namespace NouAssist.IntelligenceStrategy;

public static class IntelligenceStrategyDbProperties
{
    public static string DbTablePrefix { get; set; } = "IntelligenceStrategy";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "IntelligenceStrategy";
}
