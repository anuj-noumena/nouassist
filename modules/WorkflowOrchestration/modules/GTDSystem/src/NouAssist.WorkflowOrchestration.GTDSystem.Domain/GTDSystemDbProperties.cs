namespace NouAssist.WorkflowOrchestration.GTDSystem;

public static class GTDSystemDbProperties
{
    public static string DbTablePrefix { get; set; } = "GTDSystem";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "GTDSystem";
}
