namespace NouAssist.WorkflowOrchestration.GTDSystem;

public static class GTDDbProperties
{
    public static string DbTablePrefix { get; set; } = "GTD";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "GTD";
}
