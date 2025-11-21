using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Constants;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Enums;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowDefinition
{
    /// <summary>
    /// DTO for creating an activity
    /// </summary>
    public class CreateActivityDto
    {
        /// <summary>
        /// The name of the activity
        /// </summary>
        [Required]
        [StringLength(WorkflowEngineConsts.MaxNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// The type of the activity
        /// </summary>
        [Required]
        public ActivityType ActivityType { get; set; }
        
        /// <summary>
        /// The description of the activity
        /// </summary>
        [StringLength(WorkflowEngineConsts.MaxDescriptionLength)]
        public string Description { get; set; }
        
        /// <summary>
        /// Additional properties for the activity
        /// </summary>
        public Dictionary<string, string> Properties { get; set; }
    }
}
