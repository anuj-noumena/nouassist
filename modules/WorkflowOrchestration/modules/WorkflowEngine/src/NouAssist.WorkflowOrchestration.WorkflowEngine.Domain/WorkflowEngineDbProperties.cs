namespace NouAssist.WorkflowOrchestration.WorkflowEngine;

public static class WorkflowEngineDbProperties
{
    public static string DbTablePrefix { get; set; } = "WorkflowEngine";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "WorkflowEngine";
}
