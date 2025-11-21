using System;

namespace NouAssist.BusinessOperations.ProjectExecution.Events.Base
{
    /// <summary>
    /// Base class for all Project Execution event transfer objects
    /// </summary>
    [Serializable]
    public abstract class ProjectExecutionEtoBase
    {
        /// <summary>
        /// Tenant ID for multi-tenancy support
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
