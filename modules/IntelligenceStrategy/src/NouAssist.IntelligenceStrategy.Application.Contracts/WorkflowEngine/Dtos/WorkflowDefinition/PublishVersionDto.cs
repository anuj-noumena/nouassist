using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowDefinition
{
    /// <summary>
    /// DTO for publishing a workflow version
    /// </summary>
    public class PublishVersionDto
    {
        /// <summary>
        /// The version number to publish
        /// </summary>
        [Required]
        public int VersionNumber { get; set; }
    }
}
