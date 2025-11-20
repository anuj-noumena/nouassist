namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Repositories
{
    /// <summary>
    /// Repository interface for workflow instances
    /// </summary>
    public interface IWorkflowInstanceRepository : IRepository<WorkflowInstance, Guid>
    {
        /// <summary>
        /// Finds workflow instances matching the given specification
        /// </summary>
        /// <param name="specification">The specification to match</param>
        /// <param name="includeDetails">Whether to include all details</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A list of matching workflow instances</returns>
        Task<List<WorkflowInstance>> GetListAsync(
            Specification<WorkflowInstance> specification,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all workflow instances for a specific workflow definition
        /// </summary>
        /// <param name="workflowDefinitionId">The ID of the workflow definition</param>
        /// <param name="includeDetails">Whether to include all details</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A list of workflow instances for the given workflow definition</returns>
        Task<List<WorkflowInstance>> GetByWorkflowDefinitionIdAsync(
            Guid workflowDefinitionId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all active workflow instances
        /// </summary>
        /// <param name="includeDetails">Whether to include all details</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A list of active workflow instances</returns>
        Task<List<WorkflowInstance>> GetActiveInstancesAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all tasks assigned to a specific assignee
        /// </summary>
        /// <param name="assigneeType">The type of assignee</param>
        /// <param name="assigneeId">The ID of the assignee</param>
        /// <param name="includeDetails">Whether to include all details</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A list of activity instances assigned to the given assignee</returns>
        Task<List<ActivityInstance>> GetAssignedTasksAsync(
            AssigneeType assigneeType,
            Guid assigneeId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);
    }
}
