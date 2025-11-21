using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowTemplate
{
    /// <summary>
    /// DTO for getting a list of workflow templates with filtering and paging
    /// </summary>
    public class GetWorkflowTemplateListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Filter by name, description, or category
        /// </summary>
        public string Filter { get; set; }
        
        /// <summary>
        /// Filter by category
        /// </summary>
        public string Category { get; set; }
        
        /// <summary>
        /// Filter by tag
        /// </summary>
        public string Tag { get; set; }
        
        /// <summary>
        /// Filter by public status
        /// </summary>
        public bool? IsPublic { get; set; }
    }
}
