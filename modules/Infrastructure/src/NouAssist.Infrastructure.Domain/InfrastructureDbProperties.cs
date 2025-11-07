namespace NouAssist.Infrastructure;

public static class InfrastructureDbProperties
{
    public static string DbTablePrefix { get; set; } = "Infrastructure";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Infrastructure";
}
