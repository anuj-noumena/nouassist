using System;
using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.Common;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowTemplate
{
    /// <summary>
    /// DTO for workflow template data
    /// </summary>
    public class WorkflowTemplateDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// The name of the template
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The description of the template
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// The category of the template
        /// </summary>
        public string Category { get; set; }
        
        /// <summary>
        /// Tags for the template
        /// </summary>
        public List<string> Tags { get; set; }
        
        /// <summary>
        /// The BPMN diagram for this template
        /// </summary>
        public BpmnDiagramDto BpmnDiagram { get; set; }
        
        /// <summary>
        /// Indicates if this template is public and available to all tenants
        /// </summary>
        public bool IsPublic { get; set; }
    }
}
