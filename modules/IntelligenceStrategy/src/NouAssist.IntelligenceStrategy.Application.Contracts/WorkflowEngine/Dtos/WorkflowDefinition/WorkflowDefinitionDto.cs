using System;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.Common;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowDefinition
{
    /// <summary>
    /// DTO for workflow definition data
    /// </summary>
    public class WorkflowDefinitionDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// The name of the workflow definition
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The description of the workflow definition
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// The category of the workflow definition
        /// </summary>
        public string Category { get; set; }
        
        /// <summary>
        /// Indicates if this workflow definition is published
        /// </summary>
        public bool IsPublished { get; set; }
        
        /// <summary>
        /// The ID of the published version, if any
        /// </summary>
        public int? PublishedVersionNumber { get; set; }
        
        /// <summary>
        /// The latest version number
        /// </summary>
        public int LatestVersionNumber { get; set; }
        
        /// <summary>
        /// Performance metrics for this workflow definition
        /// </summary>
        public PerformanceMetricsDto PerformanceMetrics { get; set; }
    }
}
