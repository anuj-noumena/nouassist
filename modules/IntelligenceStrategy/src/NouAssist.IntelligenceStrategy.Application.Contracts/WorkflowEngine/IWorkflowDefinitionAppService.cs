namespace NouAssist.IntelligenceStrategy.WorkflowEngine
{
    /// <summary>
    /// Application service for workflow definitions
    /// </summary>
    public interface IWorkflowDefinitionAppService : IApplicationService
    {
        /// <summary>
        /// Gets a workflow definition by ID
        /// </summary>
        /// <param name="id">The ID of the workflow definition</param>
        /// <returns>The workflow definition</returns>
        Task<WorkflowDefinitionDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a list of workflow definitions
        /// </summary>
        /// <param name="input">The filtering and paging parameters</param>
        /// <returns>The list of workflow definitions</returns>
        Task<PagedResultDto<WorkflowDefinitionDto>> GetListAsync(GetWorkflowDefinitionListDto input);
        
        /// <summary>
        /// Creates a new workflow definition
        /// </summary>
        /// <param name="input">The data for the new workflow definition</param>
        /// <returns>The created workflow definition</returns>
        Task<WorkflowDefinitionDto> CreateAsync(CreateWorkflowDefinitionDto input);
        
        /// <summary>
        /// Updates a workflow definition
        /// </summary>
        /// <param name="id">The ID of the workflow definition</param>
        /// <param name="input">The new data for the workflow definition</param>
        /// <returns>The updated workflow definition</returns>
        Task<WorkflowDefinitionDto> UpdateAsync(Guid id, UpdateWorkflowDefinitionDto input);
        
        /// <summary>
        /// Deletes a workflow definition
        /// </summary>
        /// <param name="id">The ID of the workflow definition</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Gets a specific version of a workflow definition
        /// </summary>
        /// <param name="id">The ID of the workflow definition</param>
        /// <param name="versionNumber">The version number</param>
        /// <returns>The workflow version</returns>
        Task<WorkflowVersionDto> GetVersionAsync(Guid id, int versionNumber);
        
        /// <summary>
        /// Creates a new version of a workflow definition
        /// </summary>
        /// <param name="id">The ID of the workflow definition</param>
        /// <param name="input">The data for the new version</param>
        /// <returns>The created workflow version</returns>
        Task<WorkflowVersionDto> CreateVersionAsync(Guid id, CreateVersionDto input);
        
        /// <summary>
        /// Updates the BPMN diagram of a workflow version
        /// </summary>
        /// <param name="id">The ID of the workflow definition</param>
        /// <param name="versionNumber">The version number</param>
        /// <param name="input">The new BPMN data</param>
        /// <returns>The updated workflow version</returns>
        Task<WorkflowVersionDto> UpdateBpmnAsync(Guid id, int versionNumber, UpdateBpmnDto input);
        
        /// <summary>
        /// Publishes a version of a workflow definition
        /// </summary>
        /// <param name="id">The ID of the workflow definition</param>
        /// <param name="input">The version to publish</param>
        /// <returns>The updated workflow definition</returns>
        Task<WorkflowDefinitionDto> PublishVersionAsync(Guid id, PublishVersionDto input);
        
        /// <summary>
        /// Adds an activity to a workflow version
        /// </summary>
        /// <param name="id">The ID of the workflow definition</param>
        /// <param name="versionNumber">The version number</param>
        /// <param name="input">The data for the new activity</param>
        /// <returns>The created activity</returns>
        Task<ActivityDefinitionDto> AddActivityAsync(Guid id, int versionNumber, CreateActivityDto input);
        
        /// <summary>
        /// Adds a condition to a workflow version
        /// </summary>
        /// <param name="id">The ID of the workflow definition</param>
        /// <param name="versionNumber">The version number</param>
        /// <param name="input">The data for the new condition</param>
        /// <returns>The created condition</returns>
        Task<ConditionDefinitionDto> AddConditionAsync(Guid id, int versionNumber, CreateConditionDto input);
        
        /// <summary>
        /// Removes an activity from a workflow version
        /// </summary>
        /// <param name="id">The ID of the workflow definition</param>
        /// <param name="versionNumber">The version number</param>
        /// <param name="activityId">The ID of the activity to remove</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task RemoveActivityAsync(Guid id, int versionNumber, Guid activityId);
        
        /// <summary>
        /// Analyzes the performance of a workflow definition
        /// </summary>
        /// <param name="id">The ID of the workflow definition</param>
        /// <returns>The workflow definition with updated performance metrics</returns>
        Task<WorkflowDefinitionDto> AnalyzePerformanceAsync(Guid id);
        
        /// <summary>
        /// Generates documentation for a workflow definition
        /// </summary>
        /// <param name="id">The ID of the workflow definition</param>
        /// <returns>The workflow definition with updated documentation</returns>
        Task<string> GenerateDocumentationAsync(Guid id);
    }
}
