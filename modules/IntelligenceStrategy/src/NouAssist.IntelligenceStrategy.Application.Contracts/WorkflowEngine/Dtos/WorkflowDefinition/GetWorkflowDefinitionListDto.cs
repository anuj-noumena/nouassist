using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowDefinition
{
    /// <summary>
    /// DTO for getting a list of workflow definitions with filtering and paging
    /// </summary>
    public class GetWorkflowDefinitionListDto : PagedAndSortedResultRequestDto
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
        /// Filter by published status
        /// </summary>
        public bool? IsPublished { get; set; }
    }
}
