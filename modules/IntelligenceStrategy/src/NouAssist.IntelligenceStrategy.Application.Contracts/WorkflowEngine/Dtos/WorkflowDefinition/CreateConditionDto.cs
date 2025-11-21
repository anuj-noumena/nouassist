using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Constants;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowDefinition
{
    /// <summary>
    /// DTO for creating a condition
    /// </summary>
    public class CreateConditionDto
    {
        /// <summary>
        /// The name of the condition
        /// </summary>
        [Required]
        [StringLength(WorkflowEngineConsts.MaxNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// The ID of the source activity
        /// </summary>
        [Required]
        public Guid SourceActivityId { get; set; }
        
        /// <summary>
        /// The ID of the target activity
        /// </summary>
        [Required]
        public Guid TargetActivityId { get; set; }
        
        /// <summary>
        /// The condition expression
        /// </summary>
        [Required]
        [StringLength(WorkflowEngineConsts.MaxExpressionLength)]
        public string Expression { get; set; }
        
        /// <summary>
        /// The language or format of the expression
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Language { get; set; }
        
        /// <summary>
        /// Optional description of the condition
        /// </summary>
        [StringLength(WorkflowEngineConsts.MaxDescriptionLength)]
        public string Description { get; set; }
    }
}
