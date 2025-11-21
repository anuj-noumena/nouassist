using System;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.Common;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowDefinition
{
    /// <summary>
    /// DTO for condition definition data
    /// </summary>
    public class ConditionDefinitionDto : EntityDto<Guid>
    {
        /// <summary>
        /// The ID of the workflow definition this condition belongs to
        /// </summary>
        public Guid WorkflowDefinitionId { get; set; }
        
        /// <summary>
        /// The name of the condition
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The ID of the source activity
        /// </summary>
        public Guid SourceActivityId { get; set; }
        
        /// <summary>
        /// The ID of the target activity
        /// </summary>
        public Guid TargetActivityId { get; set; }
        
        /// <summary>
        /// The condition expression
        /// </summary>
        public ConditionExpressionDto Expression { get; set; }
        
        /// <summary>
        /// Optional description of the condition
        /// </summary>
        public string Description { get; set; }
    }
}
