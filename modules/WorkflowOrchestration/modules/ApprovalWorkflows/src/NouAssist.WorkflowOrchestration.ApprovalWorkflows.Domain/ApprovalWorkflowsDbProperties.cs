namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows;

public static class ApprovalWorkflowsDbProperties
{
    public static string DbTablePrefix { get; set; } = "ApprovalWorkflows";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "ApprovalWorkflows";
}
