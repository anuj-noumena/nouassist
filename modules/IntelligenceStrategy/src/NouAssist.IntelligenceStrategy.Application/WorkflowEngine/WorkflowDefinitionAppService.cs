using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Aggregates;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Dtos.WorkflowDefinition;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Entities;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Exceptions;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Interfaces.Services;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Localization;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Permissions;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Repositories;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Specifications;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Guids;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine
{
    /// <summary>
    /// Implementation of the workflow definition application service
    /// </summary>
    [Authorize(WorkflowEnginePermissions.WorkflowDefinitions.Default)]
    public class WorkflowDefinitionAppService : ApplicationService, IWorkflowDefinitionAppService
    {
        private readonly IWorkflowDefinitionRepository _workflowDefinitionRepository;
        private readonly IWorkflowManager _workflowManager;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<WorkflowDefinitionAppService> _logger;

        /// <summary>
        /// Creates a new instance of <see cref="WorkflowDefinitionAppService"/>
        /// </summary>
        public WorkflowDefinitionAppService(
            IWorkflowDefinitionRepository workflowDefinitionRepository,
            IWorkflowManager workflowManager,
            IGuidGenerator guidGenerator,
            ILogger<WorkflowDefinitionAppService> logger)
        {
            _workflowDefinitionRepository = workflowDefinitionRepository;
            _workflowManager = workflowManager;
            _guidGenerator = guidGenerator;
            _logger = logger;
            LocalizationResource = typeof(WorkflowEngineResource);
        }

        /// <inheritdoc/>
        public async Task<WorkflowDefinitionDto> GetAsync(Guid id)
        {
            try
            {
                var workflowDefinition = await _workflowDefinitionRepository.GetAsync(id);
                
                return ObjectMapper.Map<WorkflowDefinition, WorkflowDefinitionDto>(workflowDefinition);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow definition {Id} not found", id);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowNotFound"])
                    .WithData("workflowId", id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<PagedResultDto<WorkflowDefinitionDto>> GetListAsync(GetWorkflowDefinitionListDto input)
        {
            try
            {
                var specification = CreateSpecification(input);
                
                var count = await _workflowDefinitionRepository.CountAsync(specification);
                
                var query = await _workflowDefinitionRepository.GetQueryableAsync();
                query = query.Where(specification.ToExpression());
                
                query = ApplySorting(query, input);
                query = query.PageBy(input);
                
                var workflowDefinitions = await AsyncExecuter.ToListAsync(query);
                
                var dtos = ObjectMapper.Map<List<WorkflowDefinition>, List<WorkflowDefinitionDto>>(workflowDefinitions);
                
                return new PagedResultDto<WorkflowDefinitionDto>(count, dtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowDefinitions.Create)]
        public async Task<WorkflowDefinitionDto> CreateAsync(CreateWorkflowDefinitionDto input)
        {
            try
            {
                var workflowDefinition = await _workflowManager.CreateWorkflowDefinitionAsync(
                    input.Name,
                    input.Description,
                    input.Category);
                
                return ObjectMapper.Map<WorkflowDefinition, WorkflowDefinitionDto>(workflowDefinition);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to create workflow definition {Name}", input.Name);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorCreatingWorkflow"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowDefinitions.Edit)]
        public async Task<WorkflowDefinitionDto> UpdateAsync(Guid id, UpdateWorkflowDefinitionDto input)
        {
            try
            {
                var workflowDefinition = await _workflowDefinitionRepository.GetAsync(id);
                
                workflowDefinition.UpdateBasicInfo(input.Name, input.Description, input.Category);
                
                await _workflowDefinitionRepository.UpdateAsync(workflowDefinition);
                
                return ObjectMapper.Map<WorkflowDefinition, WorkflowDefinitionDto>(workflowDefinition);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow definition {Id} not found for update", id);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowNotFound"])
                    .WithData("workflowId", id);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to update workflow definition {Id}", id);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorUpdatingWorkflow"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowDefinitions.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var workflowDefinition = await _workflowDefinitionRepository.GetAsync(id);
                
                if (workflowDefinition.IsPublished)
                {
                    throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:CannotDeletePublishedWorkflow"]);
                }
                
                await _workflowDefinitionRepository.DeleteAsync(id);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow definition {Id} not found for deletion", id);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowNotFound"])
                    .WithData("workflowId", id);
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<WorkflowVersionDto> GetVersionAsync(Guid id, int versionNumber)
        {
            try
            {
                var workflowDefinition = await _workflowDefinitionRepository.GetAsync(id, includeDetails: true);
                
                var version = workflowDefinition.GetVersion(versionNumber);
                
                return ObjectMapper.Map<WorkflowVersion, WorkflowVersionDto>(version);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow definition {Id} not found", id);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowNotFound"])
                    .WithData("workflowId", id);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Version {VersionNumber} not found for workflow {Id}", versionNumber, id);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorGettingWorkflowVersion"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowDefinitions.Edit)]
        public async Task<WorkflowVersionDto> CreateVersionAsync(Guid id, CreateVersionDto input)
        {
            try
            {
                var version = await _workflowManager.CreateNewVersionAsync(id, input.Comments);
                
                return ObjectMapper.Map<WorkflowVersion, WorkflowVersionDto>(version);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow definition {Id} not found", id);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowNotFound"])
                    .WithData("workflowId", id);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to create new version for workflow {Id}", id);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorCreatingWorkflowVersion"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowDefinitions.Edit)]
        public async Task<WorkflowVersionDto> UpdateBpmnAsync(Guid id, int versionNumber, UpdateBpmnDto input)
        {
            try
            {
                var workflowDefinition = await _workflowDefinitionRepository.GetAsync(id, includeDetails: true);
                
                var version = workflowDefinition.GetVersion(versionNumber);
                
                version.UpdateBpmnDiagram(input.XmlContent, input.BpmnVersion);
                
                await _workflowDefinitionRepository.UpdateAsync(workflowDefinition);
                
                return ObjectMapper.Map<WorkflowVersion, WorkflowVersionDto>(version);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow definition {Id} not found", id);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowNotFound"])
                    .WithData("workflowId", id);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to update BPMN for workflow {Id} version {VersionNumber}", id, versionNumber);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorUpdatingBpmn"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowDefinitions.Publish)]
        public async Task<WorkflowDefinitionDto> PublishVersionAsync(Guid id, PublishVersionDto input)
        {
            try
            {
                var workflowDefinition = await _workflowManager.PublishVersionAsync(
                    id,
                    input.VersionNumber);
                
                return ObjectMapper.Map<WorkflowDefinition, WorkflowDefinitionDto>(workflowDefinition);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow definition {Id} not found", id);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowNotFound"])
                    .WithData("workflowId", id);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to publish version {VersionNumber} of workflow {Id}", input.VersionNumber, id);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorPublishingWorkflowVersion"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowDefinitions.Edit)]
        public async Task<ActivityDefinitionDto> AddActivityAsync(Guid id, int versionNumber, CreateActivityDto input)
        {
            try
            {
                var workflowDefinition = await _workflowDefinitionRepository.GetAsync(id, includeDetails: true);
                
                var version = workflowDefinition.GetVersion(versionNumber);
                
                var activity = version.AddActivity(
                    _guidGenerator.Create(),
                    input.Name,
                    input.ActivityType,
                    input.Properties);
                
                activity.UpdateDescription(input.Description);
                
                await _workflowDefinitionRepository.UpdateAsync(workflowDefinition);
                
                return ObjectMapper.Map<ActivityDefinition, ActivityDefinitionDto>(activity);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow definition {Id} not found", id);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowNotFound"])
                    .WithData("workflowId", id);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to add activity to workflow {Id} version {VersionNumber}", id, versionNumber);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorAddingActivity"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowDefinitions.Edit)]
        public async Task<ConditionDefinitionDto> AddConditionAsync(Guid id, int versionNumber, CreateConditionDto input)
        {
            try
            {
                var workflowDefinition = await _workflowDefinitionRepository.GetAsync(id, includeDetails: true);
                
                var version = workflowDefinition.GetVersion(versionNumber);
                
                var condition = version.AddCondition(
                    _guidGenerator.Create(),
                    input.Name,
                    input.SourceActivityId,
                    input.TargetActivityId,
                    input.Expression,
                    input.Language);
                
                condition.UpdateDescription(input.Description);
                
                await _workflowDefinitionRepository.UpdateAsync(workflowDefinition);
                
                return ObjectMapper.Map<ConditionDefinition, ConditionDefinitionDto>(condition);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow definition {Id} not found", id);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowNotFound"])
                    .WithData("workflowId", id);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to add condition to workflow {Id} version {VersionNumber}", id, versionNumber);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorAddingCondition"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowDefinitions.Edit)]
        public async Task RemoveActivityAsync(Guid id, int versionNumber, Guid activityId)
        {
            try
            {
                var workflowDefinition = await _workflowDefinitionRepository.GetAsync(id, includeDetails: true);
                
                var version = workflowDefinition.GetVersion(versionNumber);
                
                version.RemoveActivity(activityId);
                
                await _workflowDefinitionRepository.UpdateAsync(workflowDefinition);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow definition {Id} not found", id);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowNotFound"])
                    .WithData("workflowId", id);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to remove activity {ActivityId} from workflow {Id} version {VersionNumber}", 
                    activityId, id, versionNumber);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorRemovingActivity"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        [Authorize(WorkflowEnginePermissions.WorkflowDefinitions.Analyze)]
        public async Task<WorkflowDefinitionDto> AnalyzePerformanceAsync(Guid id)
        {
            try
            {
                var workflowDefinition = await _workflowManager.AnalyzePerformanceAsync(id);
                
                return ObjectMapper.Map<WorkflowDefinition, WorkflowDefinitionDto>(workflowDefinition);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow definition {Id} not found", id);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowNotFound"])
                    .WithData("workflowId", id);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to analyze performance for workflow {Id}", id);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorAnalyzingPerformance"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<string> GenerateDocumentationAsync(Guid id)
        {
            try
            {
                var workflowDefinition = await _workflowManager.GenerateDocumentationAsync(id);
                
                return workflowDefinition.Documentation;
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Workflow definition {Id} not found", id);
                throw new UserFriendlyException(L["WorkflowEngine:ErrorMessage:WorkflowNotFound"])
                    .WithData("workflowId", id);
            }
            catch (WorkflowEngineDomainException ex)
            {
                _logger.LogWarning(ex, "Failed to generate documentation for workflow {Id}", id);
                throw new UserFriendlyException(L[ex.Code ?? "ErrorGeneratingDocumentation"], ex.Data);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        private Specification<WorkflowDefinition> CreateSpecification(GetWorkflowDefinitionListDto input)
        {
            Specification<WorkflowDefinition> specification = new IdentitySpecification<WorkflowDefinition>();
            
            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                specification = specification.And(new FilterSpecification(input.Filter));
            }
            
            if (!string.IsNullOrWhiteSpace(input.Category))
            {
                specification = specification.And(new WorkflowCategorySpecification(input.Category));
            }
            
            if (input.IsPublished.HasValue)
            {
                var publishedSpec = new PublishedWorkflowSpecification();
                specification = input.IsPublished.Value 
                    ? specification.And(publishedSpec) 
                    : specification.And(publishedSpec.Not());
            }
            
            return specification;
        }

        private IQueryable<WorkflowDefinition> ApplySorting(IQueryable<WorkflowDefinition> query, GetWorkflowDefinitionListDto input)
        {
            if (!string.IsNullOrWhiteSpace(input.Sorting))
            {
                return query.OrderBy(input.Sorting);
            }
            
            return query.OrderByDescending(w => w.CreationTime);
        }

        private Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            _logger.Log(logLevel, exception, "An error occurred in workflow definition app service");
            return Task.CompletedTask;
        }

        private class FilterSpecification : Specification<WorkflowDefinition>
        {
            private readonly string _filter;

            public FilterSpecification(string filter)
            {
                _filter = filter;
            }

            public override Expression<Func<WorkflowDefinition, bool>> ToExpression()
            {
                return w => w.Name.Contains(_filter) ||
                            (w.Description != null && w.Description.Contains(_filter)) ||
                            (w.Category != null && w.Category.Contains(_filter));
            }
        }
    }
}
