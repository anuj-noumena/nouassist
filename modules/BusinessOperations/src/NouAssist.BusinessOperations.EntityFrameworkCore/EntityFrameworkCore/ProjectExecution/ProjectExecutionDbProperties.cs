namespace NouAssist.BusinessOperations.ProjectExecution.EntityFrameworkCore
{
    /// <summary>
    /// Database properties for Project Execution module
    /// </summary>
    public static class ProjectExecutionDbProperties
    {
        /// <summary>
        /// Connection string name for Project Execution module
        /// </summary>
        public const string ConnectionStringName = "ProjectExecution";
        
        /// <summary>
        /// Table prefix for Project Execution module tables
        /// </summary>
        public const string DbTablePrefix = "ProjectExecution_";
        
        /// <summary>
        /// Schema name for Project Execution module tables
        /// </summary>
        public const string DbSchema = "proj"; // Using a dedicated schema for better organization
    }
}
