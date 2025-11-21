using System;
using NouAssist.BusinessOperations.ProjectExecution.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.BusinessOperations.ProjectExecution.Events
{
    /// <summary>
    /// Event raised when a project status is updated
    /// </summary>
    [EventName("NouAssist.BusinessOperations.ProjectExecution.ProjectStatusUpdated")]
    public class ProjectStatusUpdatedEto : ProjectExecutionEtoBase
    {
        /// <summary>
        /// ID of the project
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Name of the project
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Previous status of the project
        /// </summary>
        public ProjectStatus PreviousStatus { get; set; }
        
        /// <summary>
        /// New status of the project
        /// </summary>
        public ProjectStatus NewStatus { get; set; }
    }
}
