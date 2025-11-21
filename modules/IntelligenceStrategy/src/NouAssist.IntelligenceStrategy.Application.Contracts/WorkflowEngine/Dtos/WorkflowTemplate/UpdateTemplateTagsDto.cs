using System.Collections.Generic;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowTemplate
{
    /// <summary>
    /// DTO for updating the tags of a workflow template
    /// </summary>
    public class UpdateTemplateTagsDto
    {
        /// <summary>
        /// The tags to set
        /// </summary>
        public List<string> Tags { get; set; }
    }
}
