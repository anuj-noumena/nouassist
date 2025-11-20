namespace NouAssist.IntelligenceStrategy.WorkflowEngine.EntityFrameworkCore
{
    /// <summary>
    /// Database properties for the Workflow Engine module
    /// </summary>
    public static class WorkflowEngineDbProperties
    {
        /// <summary>
        /// Connection string name for the Workflow Engine module
        /// </summary>
        public const string ConnectionStringName = "WorkflowEngine";

        /// <summary>
        /// Table prefix for all tables in the Workflow Engine module
        /// </summary>
        public const string DbTablePrefix = "WE_";

        /// <summary>
        /// Schema name for all tables in the Workflow Engine module
        /// </summary>
        public static string? DbSchema { get; set; } = null;
    }
}
