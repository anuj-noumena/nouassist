namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows.EntityFrameworkCore
{
    public static class ApprovalDbProperties
    {
        public const string ConnectionStringName = "Approval";
        
        public static string DbTablePrefix { get; set; } = "App";
        
        public static string DbSchema { get; set; } = null;
    }
}
