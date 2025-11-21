using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowInstance;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine
{
    /// <summary>
    /// Application service for workflow instances
    /// </summary>
    public interface IWorkflowInstanceAppService : IApplicationService
    {
        /// <summary>
        /// Gets a workflow instance by ID
        /// </summary>
        /// <param name="id">The ID of the workflow instance</param>
        /// <returns>The workflow instance</returns>
        Task<WorkflowInstanceDetailDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a list of workflow instances
        /// </summary>
        /// <param name="input">The filtering and paging parameters</param>
        /// <returns>The list of workflow instances</returns>
        Task<PagedResultDto<WorkflowInstanceDto>> GetListAsync(GetWorkflowInstanceListDto input);
        
        /// <summary>
        /// Gets a list of active tasks assigned to a user, team, or role
        /// </summary>
        /// <param name="input">The filtering and paging parameters</param>
        /// <returns>The list of assigned tasks</returns>
        Task<PagedResultDto<ActivityInstanceDto>> GetAssignedTasksAsync(GetAssignedTasksDto input);
        
        /// <summary>
        /// Starts a new workflow instance
        /// </summary>
        /// <param name="workflowDefinitionId">The ID of the workflow definition</param>
        /// <param name="input">The data for the new workflow instance</param>
        /// <returns>The created workflow instance</returns>
        Task<WorkflowInstanceDetailDto> StartAsync(Guid workflowDefinitionId, StartWorkflowInstanceDto input);
        
        /// <summary>
        /// Assigns a task to a user, team, or role
        /// </summary>
        /// <param name="workflowInstanceId">The ID of the workflow instance</param>
        /// <param name="activityInstanceId">The ID of the activity instance</param>
        /// <param name="input">The assignment data</param>
        /// <returns>The updated activity instance</returns>
        Task<ActivityInstanceDto> AssignTaskAsync(Guid workflowInstanceId, Guid activityInstanceId, AssignTaskDto input);
        
        /// <summary>
        /// Starts an activity
        /// </summary>
        /// <param name="workflowInstanceId">The ID of the workflow instance</param>
        /// <param name="activityInstanceId">The ID of the activity instance</param>
        /// <returns>The updated activity instance</returns>
        Task<ActivityInstanceDto> StartActivityAsync(Guid workflowInstanceId, Guid activityInstanceId);
        
        /// <summary>
        /// Completes a task
        /// </summary>
        /// <param name="workflowInstanceId">The ID of the workflow instance</param>
        /// <param name="activityInstanceId">The ID of the activity instance</param>
        /// <param name="input">The completion data</param>
        /// <returns>The updated activity instance</returns>
        Task<ActivityInstanceDto> CompleteTaskAsync(Guid workflowInstanceId, Guid activityInstanceId, CompleteTaskDto input);
        
        /// <summary>
        /// Fails a task
        /// </summary>
        /// <param name="workflowInstanceId">The ID of the workflow instance</param>
        /// <param name="activityInstanceId">The ID of the activity instance</param>
        /// <param name="errorMessage">The error message</param>
        /// <returns>The updated activity instance</returns>
        Task<ActivityInstanceDto> FailTaskAsync(Guid workflowInstanceId, Guid activityInstanceId, string errorMessage);
        
        /// <summary>
        /// Pauses a workflow instance
        /// </summary>
        /// <param name="id">The ID of the workflow instance</param>
        /// <param name="reason">The reason for pausing</param>
        /// <returns>The updated workflow instance</returns>
        Task<WorkflowInstanceDto> PauseAsync(Guid id, string reason);
        
        /// <summary>
        /// Resumes a paused workflow instance
        /// </summary>
        /// <param name="id">The ID of the workflow instance</param>
        /// <returns>The updated workflow instance</returns>
        Task<WorkflowInstanceDto> ResumeAsync(Guid id);
        
        /// <summary>
        /// Cancels a workflow instance
        /// </summary>
        /// <param name="id">The ID of the workflow instance</param>
        /// <param name="reason">The reason for cancellation</param>
        /// <returns>The updated workflow instance</returns>
        Task<WorkflowInstanceDto> CancelAsync(Guid id, string reason);
    }
}
