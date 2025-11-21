using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Constants;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowTemplate
{
    /// <summary>
    /// DTO for creating a workflow definition from a template
    /// </summary>
    public class CreateFromTemplateDto
    {
        /// <summary>
        /// The name for the new workflow definition
        /// </summary>
        [Required]
        [StringLength(WorkflowEngineConsts.MaxNameLength)]
        public string Name { get; set; }
    }
}
