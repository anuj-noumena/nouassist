using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowDefinition
{
    /// <summary>
    /// DTO for creating a new workflow version
    /// </summary>
    public class CreateVersionDto
    {
        /// <summary>
        /// Optional comments for the new version
        /// </summary>
        [StringLength(1000)]
        public string Comments { get; set; }
    }
}
