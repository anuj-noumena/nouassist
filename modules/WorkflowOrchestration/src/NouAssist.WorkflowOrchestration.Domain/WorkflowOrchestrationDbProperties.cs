namespace NouAssist.WorkflowOrchestration;

public static class WorkflowOrchestrationDbProperties
{
    public static string DbTablePrefix { get; set; } = "WorkflowOrchestration";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "WorkflowOrchestration";
}
