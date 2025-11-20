namespace NouAssist.IntelligenceStrategy.WorkflowEngine
{
    /// <summary>
    /// Implementation of the workflow instance application service
    /// </summary>
    [Authorize(WorkflowEnginePermissions.WorkflowInstances.Default)]
    public class WorkflowInstanceAppService : ApplicationService, IWorkflowInstanceAppService
    {
        private readonly IWorkflowInstanceRepository _workflowInstanceRepository;
        private readonly IWorkflowDefinitionRepository _workflowDefinitionRepository;
        private readonly IWorkflowManager _workflowManager;
        private readonly ILogger<WorkflowInstanceAppService> _logger;

        /// <summary>
        /// Creates a new instance of <see cref="WorkflowInstanceAppService"/>
        /// </summary>
        public WorkflowInstanceAppService(
            IWorkflowInstanceRepository workflowInstanceRepository,
            IWorkflowDefinitionRepository workflowDefinitionRepository,
            IWorkflowManager workflowManager,
            ILogger<WorkflowInstanceAppService> logger)
        {
            _workflowInstanceRepository = workflowInstanceRepository;
            _workflowDefinitionRepository = workflowDefinitionRepository;
            _workflowManager = workflowManager;
            _logger = logger;
            LocalizationResource = typeof(WorkflowEngineResource);
        }

        /// <inheritdoc/>
        public async Task<WorkflowInstanceDetailDto> GetAsync(Guid id)
        {
            try
            {
                var workflowInstance = await _workflowInstanceRepository.GetAsync(id, includeDetails: true);
                
                var dto = ObjectMapper.Map<WorkflowInstance, WorkflowInstanceDetailDto>(workflowInstance);
                
                await EnrichWorkflowInstanceDto(dto);
                
                return dto;
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow instance {Id} not found", id);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowInstanceNotFound"])
                    .WithData("instanceId", id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<PagedResultDto<WorkflowInstanceDto>> GetListAsync(GetWorkflowInstanceListDto input)
        {
            try
            {
                var query = await _workflowInstanceRepository.GetQueryableAsync();
                
                query = ApplyFilter(query, input);
                
                var totalCount = await AsyncExecuter.CountAsync(query);
                
                query = ApplySorting(query, input);
                query = query.PageBy(input);
                
                var instances = await AsyncExecuter.ToListAsync(query);
                
                var dtos = ObjectMapper.Map<List<WorkflowInstance>, List<WorkflowInstanceDto>>(instances);
                
                foreach (var dto in dtos)
                {
                    await EnrichWorkflowInstanceDto(dto);
                }
                
                return new PagedResultDto<WorkflowInstanceDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<PagedResultDto<ActivityInstanceDto>> GetAssignedTasksAsync(GetAssignedTasksDto input)
        {
            try
            {
                var tasks = await _workflowInstanceRepository.GetAssignedTasksAsync(
                    input.AssigneeType,
                    input.AssigneeId);
                
                // Apply additional filtering
                var filteredTasks = tasks.AsQueryable();
                
                if (input.Status.HasValue)
                {
                    filteredTasks = filteredTasks.Where(t => t.Status == input.Status.Value);
                }
                
                if (input.DueDateFrom.HasValue)
                {
                    filteredTasks = filteredTasks.Where(t => t.DueDate >= input.DueDateFrom.Value);
                }
                
                if (input.DueDateTo.HasValue)
                {
                    filteredTasks = filteredTasks.Where(t => t.DueDate <= input.DueDateTo.Value);
                }
                
                // Apply sorting and paging
                filteredTasks = ApplyTaskSorting(filteredTasks, input);
                
                var totalCount = filteredTasks.Count();
                
                filteredTasks = filteredTasks.PageBy(input);
                
                var result = filteredTasks.ToList();
                
                var dtos = ObjectMapper.Map<List<ActivityInstance>, List<ActivityInstanceDto>>(result);
                
                return new PagedResultDto<ActivityInstanceDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowInstances.Create)]
        public async Task<WorkflowInstanceDetailDto> StartAsync(Guid workflowDefinitionId, StartWorkflowInstanceDto input)
        {
            try
            {
                var workflowInstance = await _workflowManager.StartWorkflowAsync(
                    workflowDefinitionId,
                    input.Name,
                    input.ContextData);
                
                var dto = ObjectMapper.Map<WorkflowInstance, WorkflowInstanceDetailDto>(workflowInstance);
                
                await EnrichWorkflowInstanceDto(dto);
                
                return dto;
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow definition {Id} not found", workflowDefinitionId);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowNotFound"])
                    .WithData("workflowId", workflowDefinitionId);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to start workflow instance for definition {Id}", workflowDefinitionId);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorStartingWorkflowInstance"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowInstances.AssignTasks)]
        public async Task<ActivityInstanceDto> AssignTaskAsync(Guid workflowInstanceId, Guid activityInstanceId, AssignTaskDto input)
        {
            try
            {
                var assignee = new AssigneeReference(input.AssigneeType, input.AssigneeId, input.AssigneeDisplayName);
                
                var workflowInstance = await _workflowManager.AssignTaskAsync(
                    workflowInstanceId,
                    activityInstanceId,
                    input.AssigneeType,
                    input.AssigneeId,
                    input.DueDate);
                
                var activityInstance = workflowInstance.ActivityInstances
                    .First(a => a.Id == activityInstanceId);
                
                return ObjectMapper.Map<ActivityInstance, ActivityInstanceDto>(activityInstance);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow instance {Id} not found", workflowInstanceId);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowInstanceNotFound"])
                    .WithData("instanceId", workflowInstanceId);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to assign task {ActivityId} in workflow instance {Id}", 
                    activityInstanceId, workflowInstanceId);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorAssigningTask"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowInstances.CompleteTasks)]
        public async Task<ActivityInstanceDto> StartActivityAsync(Guid workflowInstanceId, Guid activityInstanceId)
        {
            try
            {
                var workflowInstance = await _workflowInstanceRepository.GetAsync(workflowInstanceId, includeDetails: true);
                
                var activityInstance = workflowInstance.GetActivityInstance(activityInstanceId);
                
                // Check ownership if needed
                await CheckActivityOwnershipAsync(activityInstance);
                
                workflowInstance.StartActivity(activityInstanceId);
                
                await _workflowInstanceRepository.UpdateAsync(workflowInstance);
                
                return ObjectMapper.Map<ActivityInstance, ActivityInstanceDto>(activityInstance);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow instance {Id} not found", workflowInstanceId);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowInstanceNotFound"])
                    .WithData("instanceId", workflowInstanceId);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to start activity {ActivityId} in workflow instance {Id}", 
                    activityInstanceId, workflowInstanceId);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorStartingActivity"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowInstances.CompleteTasks)]
        public async Task<ActivityInstanceDto> CompleteTaskAsync(Guid workflowInstanceId, Guid activityInstanceId, CompleteTaskDto input)
        {
            try
            {
                var workflowInstance = await _workflowManager.CompleteTaskAsync(
                    workflowInstanceId,
                    activityInstanceId,
                    input.OutputData);
                
                var activityInstance = workflowInstance.ActivityInstances
                    .First(a => a.Id == activityInstanceId);
                
                return ObjectMapper.Map<ActivityInstance, ActivityInstanceDto>(activityInstance);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow instance {Id} not found", workflowInstanceId);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowInstanceNotFound"])
                    .WithData("instanceId", workflowInstanceId);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to complete task {ActivityId} in workflow instance {Id}", 
                    activityInstanceId, workflowInstanceId);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorCompletingTask"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowInstances.CompleteTasks)]
        public async Task<ActivityInstanceDto> FailTaskAsync(Guid workflowInstanceId, Guid activityInstanceId, string errorMessage)
        {
            try
            {
                var workflowInstance = await _workflowInstanceRepository.GetAsync(workflowInstanceId, includeDetails: true);
                
                var activityInstance = workflowInstance.GetActivityInstance(activityInstanceId);
                
                // Check ownership if needed
                await CheckActivityOwnershipAsync(activityInstance);
                
                workflowInstance.FailActivity(activityInstanceId, errorMessage);
                
                await _workflowInstanceRepository.UpdateAsync(workflowInstance);
                
                return ObjectMapper.Map<ActivityInstance, ActivityInstanceDto>(activityInstance);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow instance {Id} not found", workflowInstanceId);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowInstanceNotFound"])
                    .WithData("instanceId", workflowInstanceId);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to fail task {ActivityId} in workflow instance {Id}", 
                    activityInstanceId, workflowInstanceId);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorFailingTask"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowInstances.Manage)]
        public async Task<WorkflowInstanceDto> PauseAsync(Guid id, string reason)
        {
            try
            {
                var workflowInstance = await _workflowInstanceRepository.GetAsync(id);
                
                workflowInstance.Pause(reason);
                
                await _workflowInstanceRepository.UpdateAsync(workflowInstance);
                
                var dto = ObjectMapper.Map<WorkflowInstance, WorkflowInstanceDto>(workflowInstance);
                
                await EnrichWorkflowInstanceDto(dto);
                
                return dto;
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow instance {Id} not found", id);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowInstanceNotFound"])
                    .WithData("instanceId", id);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to pause workflow instance {Id}", id);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorPausingWorkflowInstance"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowInstances.Manage)]
        public async Task<WorkflowInstanceDto> ResumeAsync(Guid id)
        {
            try
            {
                var workflowInstance = await _workflowInstanceRepository.GetAsync(id);
                
                workflowInstance.Resume();
                
                await _workflowInstanceRepository.UpdateAsync(workflowInstance);
                
                var dto = ObjectMapper.Map<WorkflowInstance, WorkflowInstanceDto>(workflowInstance);
                
                await EnrichWorkflowInstanceDto(dto);
                
                return dto;
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow instance {Id} not found", id);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowInstanceNotFound"])
                    .WithData("instanceId", id);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to resume workflow instance {Id}", id);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorResumingWorkflowInstance"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowInstances.Manage)]
        public async Task<WorkflowInstanceDto> CancelAsync(Guid id, string reason)
        {
            try
            {
                var workflowInstance = await _workflowInstanceRepository.GetAsync(id);
                
                workflowInstance.FailWorkflow(reason);
                
                await _workflowInstanceRepository.UpdateAsync(workflowInstance);
                
                var dto = ObjectMapper.Map<WorkflowInstance, WorkflowInstanceDto>(workflowInstance);
                
                await EnrichWorkflowInstanceDto(dto);
                
                return dto;
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow instance {Id} not found", id);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowInstanceNotFound"])
                    .WithData("instanceId", id);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to cancel workflow instance {Id}", id);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorCancellingWorkflowInstance"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        private async Task EnrichWorkflowInstanceDto(WorkflowInstanceDto dto)
        {
            try
            {
                var workflowDefinition = await _workflowDefinitionRepository.GetAsync(dto.WorkflowDefinitionId);
                dto.WorkflowDefinitionName = workflowDefinition.Name;
                
                if (dto is WorkflowInstanceDetailDto detailDto)
                {
                    if (detailDto.ActivityInstances != null)
                    {
                        dto.ActiveTasksCount = detailDto.ActivityInstances.Count(a => 
                            a.Status == TaskStatus.Created || 
                            a.Status == TaskStatus.Assigned || 
                            a.Status == TaskStatus.InProgress ||
                            a.Status == TaskStatus.Waiting);
                        
                        dto.TotalTasksCount = detailDto.ActivityInstances.Count;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log but don't fail the whole request just because we couldn't enrich the DTOs
                _logger.LogWarning(ex, "Error enriching workflow instance DTO");
            }
        }

        private IQueryable<WorkflowInstance> ApplyFilter(
            IQueryable<WorkflowInstance> query,
            GetWorkflowInstanceListDto input)
        {
            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                query = query.Where(i => i.Name.Contains(input.Filter));
                // Cannot easily filter on context data as it's stored as JSON
            }
            
            if (input.WorkflowDefinitionId.HasValue)
            {
                query = query.Where(i => i.WorkflowDefinitionId == input.WorkflowDefinitionId.Value);
            }
            
            if (input.Status.HasValue)
            {
                query = query.Where(i => i.Status == input.Status.Value);
            }
            
            if (input.StartDateFrom.HasValue)
            {
                query = query.Where(i => i.StartTime >= input.StartDateFrom.Value);
            }
            
            if (input.StartDateTo.HasValue)
            {
                query = query.Where(i => i.StartTime <= input.StartDateTo.Value);
            }
            
            return query;
        }

        private IQueryable<WorkflowInstance> ApplySorting(
            IQueryable<WorkflowInstance> query,
            GetWorkflowInstanceListDto input)
        {
            if (!string.IsNullOrWhiteSpace(input.Sorting))
            {
                return query.OrderBy(input.Sorting);
            }
            
            return query.OrderByDescending(i => i.CreationTime);
        }

        private IQueryable<ActivityInstance> ApplyTaskSorting(
            IQueryable<ActivityInstance> query,
            GetAssignedTasksDto input)
        {
            if (!string.IsNullOrWhiteSpace(input.Sorting))
            {
                return query.OrderBy(input.Sorting);
            }
            
            // Default: sort by due date (ascending) if available, then by activity name
            return query
                .OrderBy(t => t.DueDate != null ? 0 : 1) // Tasks with due dates first
                .ThenBy(t => t.DueDate)                  // Sorted by due date (nearest first)
                .ThenBy(t => t.Name);                    // Then by name
        }

        private async Task CheckActivityOwnershipAsync(ActivityInstance activityInstance)
        {
            // This method can be extended to enforce ownership checks
            // For example, only allow a user to operate on tasks assigned to them
            
            // If the activity is assigned to a specific user, verify ownership
            if (activityInstance.Assignee != null && 
                activityInstance.Assignee.Type == AssigneeType.User)
            {
                var currentUserId = CurrentUser.Id;
                if (currentUserId.HasValue && activityInstance.Assignee.Id != currentUserId.Value)
                {
                    // Log the access attempt
                    _logger.LogWarning(
                        "User {UserId} attempted to operate on task {ActivityId} assigned to user {AssigneeId}",
                        CurrentUser.Id, activityInstance.Id, activityInstance.Assignee.Id);
                    
                    // For now, we'll just log this - but in a real app you might want to enforce permissions
                    // throw new AbpAuthorizationException(L["NotAuthorizedToOperateOnTask"]);
                }
            }
            
            // Permission checks for team or role assignments could be implemented here
            
            await Task.CompletedTask;
        }

        private Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            _logger.Log(logLevel, exception, "An error occurred in workflow instance app service");
            return Task.CompletedTask;
        }
    }
}
