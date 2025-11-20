namespace NouAssist.IntelligenceStrategy.WorkflowEngine
{
    /// <summary>
    /// Implementation of the workflow template application service
    /// </summary>
    [Authorize(WorkflowEnginePermissions.WorkflowTemplates.Default)]
    public class WorkflowTemplateAppService : ApplicationService, IWorkflowTemplateAppService
    {
        private readonly IWorkflowTemplateRepository _workflowTemplateRepository;
        private readonly IWorkflowTemplateManager _workflowTemplateManager;
        private readonly ILogger<WorkflowTemplateAppService> _logger;

        /// <summary>
        /// Creates a new instance of <see cref="WorkflowTemplateAppService"/>
        /// </summary>
        public WorkflowTemplateAppService(
            IWorkflowTemplateRepository workflowTemplateRepository,
            IWorkflowTemplateManager workflowTemplateManager,
            ILogger<WorkflowTemplateAppService> logger)
        {
            _workflowTemplateRepository = workflowTemplateRepository;
            _workflowTemplateManager = workflowTemplateManager;
            _logger = logger;
            LocalizationResource = typeof(WorkflowEngineResource);
        }

        /// <inheritdoc/>
        public async Task<WorkflowTemplateDto> GetAsync(Guid id)
        {
            try
            {
                var workflowTemplate = await _workflowTemplateRepository.GetAsync(id);
                
                return ObjectMapper.Map<WorkflowTemplate, WorkflowTemplateDto>(workflowTemplate);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow template {Id} not found", id);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowTemplateNotFound"])
                    .WithData("templateId", id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<PagedResultDto<WorkflowTemplateDto>> GetListAsync(GetWorkflowTemplateListDto input)
        {
            try
            {
                var query = await _workflowTemplateRepository.GetQueryableAsync();
                
                query = ApplyFilter(query, input);
                
                var totalCount = await AsyncExecuter.CountAsync(query);
                
                query = ApplySorting(query, input);
                query = query.PageBy(input);
                
                var templates = await AsyncExecuter.ToListAsync(query);
                
                var dtos = ObjectMapper.Map<List<WorkflowTemplate>, List<WorkflowTemplateDto>>(templates);
                
                return new PagedResultDto<WorkflowTemplateDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowTemplates.Create)]
        public async Task<WorkflowTemplateDto> CreateAsync(CreateWorkflowTemplateDto input)
        {
            try
            {
                var template = await _workflowTemplateManager.CreateTemplateAsync(
                    input.Name,
                    input.Description,
                    input.Category,
                    input.BpmnXmlContent,
                    input.BpmnVersion);
                
                if (input.Tags != null && input.Tags.Any())
                {
                    await _workflowTemplateManager.SetTemplateTagsAsync(template.Id, input.Tags);
                }
                
                return ObjectMapper.Map<WorkflowTemplate, WorkflowTemplateDto>(template);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to create workflow template {Name}", input.Name);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorCreatingTemplate"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowTemplates.Edit)]
        public async Task<WorkflowTemplateDto> UpdateAsync(Guid id, UpdateWorkflowTemplateDto input)
        {
            try
            {
                var template = await _workflowTemplateRepository.GetAsync(id);
                
                template.UpdateBasicInfo(input.Name, input.Description, input.Category);
                
                await _workflowTemplateRepository.UpdateAsync(template);
                
                return ObjectMapper.Map<WorkflowTemplate, WorkflowTemplateDto>(template);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow template {Id} not found for update", id);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowTemplateNotFound"])
                    .WithData("templateId", id);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to update workflow template {Id}", id);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorUpdatingTemplate"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowTemplates.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await _workflowTemplateRepository.DeleteAsync(id);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow template {Id} not found for deletion", id);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowTemplateNotFound"])
                    .WithData("templateId", id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowTemplates.Edit)]
        public async Task<WorkflowTemplateDto> UpdateBpmnAsync(Guid id, UpdateTemplateBpmnDto input)
        {
            try
            {
                var template = await _workflowTemplateRepository.GetAsync(id);
                
                template.UpdateBpmnDiagram(input.XmlContent, input.BpmnVersion);
                
                await _workflowTemplateRepository.UpdateAsync(template);
                
                return ObjectMapper.Map<WorkflowTemplate, WorkflowTemplateDto>(template);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow template {Id} not found", id);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowTemplateNotFound"])
                    .WithData("templateId", id);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to update BPMN for template {Id}", id);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorUpdatingTemplateBpmn"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowTemplates.Edit)]
        public async Task<WorkflowTemplateDto> UpdateTagsAsync(Guid id, UpdateTemplateTagsDto input)
        {
            try
            {
                var template = await _workflowTemplateManager.SetTemplateTagsAsync(id, input.Tags);
                
                return ObjectMapper.Map<WorkflowTemplate, WorkflowTemplateDto>(template);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow template {Id} not found", id);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowTemplateNotFound"])
                    .WithData("templateId", id);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to update tags for template {Id}", id);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorUpdatingTemplateTags"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowTemplates.MakePublic)]
        public async Task<WorkflowTemplateDto> MakePublicAsync(Guid id)
        {
            try
            {
                var template = await _workflowTemplateManager.MakeTemplatePublicAsync(id);
                
                return ObjectMapper.Map<WorkflowTemplate, WorkflowTemplateDto>(template);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow template {Id} not found", id);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowTemplateNotFound"])
                    .WithData("templateId", id);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to make template {Id} public", id);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorMakingTemplatePublic"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowTemplates.MakePublic)]
        public async Task<WorkflowTemplateDto> MakePrivateAsync(Guid id)
        {
            try
            {
                var template = await _workflowTemplateManager.MakeTemplatePrivateAsync(id);
                
                return ObjectMapper.Map<WorkflowTemplate, WorkflowTemplateDto>(template);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow template {Id} not found", id);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowTemplateNotFound"])
                    .WithData("templateId", id);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to make template {Id} private", id);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorMakingTemplatePrivate"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowDefinitions.Create)]
        public async Task<WorkflowDefinitionDto> CreateFromTemplateAsync(Guid id, CreateFromTemplateDto input)
        {
            try
            {
                var workflowDefinition = await _workflowTemplateManager.CreateFromTemplateAsync(
                    id,
                    input.Name);
                
                return ObjectMapper.Map<WorkflowDefinition, WorkflowDefinitionDto>(workflowDefinition);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow template {Id} not found", id);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowTemplateNotFound"])
                    .WithData("templateId", id);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to create workflow from template {Id}", id);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorCreatingWorkflowFromTemplate"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        private IQueryable<WorkflowTemplate> ApplyFilter(
            IQueryable<WorkflowTemplate> query,
            GetWorkflowTemplateListDto input)
        {
            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                query = query.Where(t => 
                    t.Name.Contains(input.Filter) ||
                    (t.Description != null && t.Description.Contains(input.Filter)) ||
                    (t.Category != null && t.Category.Contains(input.Filter)));
            }
            
            if (!string.IsNullOrWhiteSpace(input.Category))
            {
                query = query.Where(t => t.Category == input.Category);
            }
            
            if (!string.IsNullOrWhiteSpace(input.Tag))
            {
                query = query.Where(t => EF.Functions.JsonContains(t.Tags, input.Tag));
            }
            
            if (input.IsPublic.HasValue)
            {
                query = query.Where(t => t.IsPublic == input.IsPublic.Value);
            }
            
            return query;
        }

        private IQueryable<WorkflowTemplate> ApplySorting(
            IQueryable<WorkflowTemplate> query,
            GetWorkflowTemplateListDto input)
        {
            if (!string.IsNullOrWhiteSpace(input.Sorting))
            {
                return query.OrderBy(input.Sorting);
            }
            
            return query.OrderByDescending(t => t.CreationTime);
        }

        private Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            _logger.Log(logLevel, exception, "An error occurred in workflow template app service");
            return Task.CompletedTask;
        }
    }
}
