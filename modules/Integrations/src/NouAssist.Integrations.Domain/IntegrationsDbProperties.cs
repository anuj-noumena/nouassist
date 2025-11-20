namespace NouAssist.Integrations;

public static class IntegrationsDbProperties
{
    public static string DbTablePrefix { get; set; } = "Integrations";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "Integrations";
}
