namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Interfaces.Services
{
    /// <summary>
    /// Interface for the workflow manager domain service
    /// </summary>
    public interface IWorkflowManager
    {
        /// <summary>
        /// Creates a new workflow definition
        /// </summary>
        /// <param name="name">The name of the workflow</param>
        /// <param name="description">The description of the workflow</param>
        /// <param name="category">The category of the workflow</param>
        /// <returns>The created workflow definition</returns>
        Task<WorkflowDefinition> CreateWorkflowDefinitionAsync(
            string name,
            string description,
            string category);

        /// <summary>
        /// Creates a new version of a workflow definition
        /// </summary>
        /// <param name="workflowDefinitionId">The ID of the workflow definition</param>
        /// <param name="comments">Optional comments for the new version</param>
        /// <returns>The new workflow version</returns>
        Task<WorkflowVersion> CreateNewVersionAsync(
            Guid workflowDefinitionId,
            string comments = null);

        /// <summary>
        /// Publishes a specific version of a workflow definition
        /// </summary>
        /// <param name="workflowDefinitionId">The ID of the workflow definition</param>
        /// <param name="versionNumber">The version number to publish</param>
        /// <returns>The published workflow definition</returns>
        Task<WorkflowDefinition> PublishVersionAsync(
            Guid workflowDefinitionId,
            int versionNumber);

        /// <summary>
        /// Creates a new workflow instance from a published workflow definition
        /// </summary>
        /// <param name="workflowDefinitionId">The ID of the workflow definition</param>
        /// <param name="name">The name of the workflow instance</param>
        /// <param name="initialContextData">Optional initial context data</param>
        /// <returns>The created workflow instance</returns>
        Task<WorkflowInstance> StartWorkflowAsync(
            Guid workflowDefinitionId,
            string name,
            Dictionary<string, string> initialContextData = null);

        /// <summary>
        /// Assigns a task to a user, team, or role
        /// </summary>
        /// <param name="workflowInstanceId">The ID of the workflow instance</param>
        /// <param name="activityInstanceId">The ID of the activity instance</param>
        /// <param name="assigneeType">The type of assignee</param>
        /// <param name="assigneeId">The ID of the assignee</param>
        /// <param name="dueDate">Optional due date</param>
        /// <returns>The updated workflow instance</returns>
        Task<WorkflowInstance> AssignTaskAsync(
            Guid workflowInstanceId,
            Guid activityInstanceId,
            AssigneeType assigneeType,
            Guid assigneeId,
            DateTime? dueDate = null);

        /// <summary>
        /// Completes a task
        /// </summary>
        /// <param name="workflowInstanceId">The ID of the workflow instance</param>
        /// <param name="activityInstanceId">The ID of the activity instance</param>
        /// <param name="outputData">Optional output data</param>
        /// <returns>The updated workflow instance</returns>
        Task<WorkflowInstance> CompleteTaskAsync(
            Guid workflowInstanceId,
            Guid activityInstanceId,
            Dictionary<string, string> outputData = null);

        /// <summary>
        /// Analyzes the performance of a workflow definition
        /// </summary>
        /// <param name="workflowDefinitionId">The ID of the workflow definition</param>
        /// <returns>The workflow definition with updated performance metrics</returns>
        Task<WorkflowDefinition> AnalyzePerformanceAsync(
            Guid workflowDefinitionId);

        /// <summary>
        /// Generates documentation for a workflow definition
        /// </summary>
        /// <param name="workflowDefinitionId">The ID of the workflow definition</param>
        /// <returns>The workflow definition with updated documentation</returns>
        Task<WorkflowDefinition> GenerateDocumentationAsync(
            Guid workflowDefinitionId);
    }
}
