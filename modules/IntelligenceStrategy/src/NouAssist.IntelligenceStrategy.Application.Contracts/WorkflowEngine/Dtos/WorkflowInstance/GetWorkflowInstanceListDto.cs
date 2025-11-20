namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowInstance
{
    /// <summary>
    /// DTO for getting a list of workflow instances with filtering and paging
    /// </summary>
    public class GetWorkflowInstanceListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Filter by name or context data
        /// </summary>
        public string Filter { get; set; }
        
        /// <summary>
        /// Filter by workflow definition ID
        /// </summary>
        public Guid? WorkflowDefinitionId { get; set; }
        
        /// <summary>
        /// Filter by status
        /// </summary>
        public InstanceStatus? Status { get; set; }
        
        /// <summary>
        /// Filter by start date (from)
        /// </summary>
        public DateTime? StartDateFrom { get; set; }
        
        /// <summary>
        /// Filter by start date (to)
        /// </summary>
        public DateTime? StartDateTo { get; set; }
    }
}
