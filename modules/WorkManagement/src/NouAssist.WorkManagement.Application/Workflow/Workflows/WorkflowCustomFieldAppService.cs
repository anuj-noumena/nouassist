using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.WorkManagement.Permissions;
using NouAssist.WorkManagement.Workflow.Data;
using NouAssist.WorkManagement.Workflow.Dtos;
using NouAssist.WorkManagement.Workflow.Entities;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.Workflow.Workflows
{
    [Authorize(WorkManagementPermissions.Workflow.Workflows.Default)]
    public class WorkflowCustomFieldAppService : ApplicationService, IWorkflowCustomFieldAppService
    {
        private readonly IWorkflowCustomFieldRepository _customFieldRepository;

        public WorkflowCustomFieldAppService(IWorkflowCustomFieldRepository customFieldRepository)
        {
            _customFieldRepository = customFieldRepository;
        }

        public async Task<WorkflowCustomFieldDto> GetAsync(Guid id)
        {
            var customField = await _customFieldRepository.GetAsync(id);
            return ObjectMapper.Map<WorkflowCustomField, WorkflowCustomFieldDto>(customField);
        }

        public async Task<List<WorkflowCustomFieldDto>> GetByWorkflowSchemeIdAsync(Guid workflowSchemeId)
        {
            var customFields = await _customFieldRepository.GetByWorkflowSchemeIdAsync(workflowSchemeId);
            return ObjectMapper.Map<List<WorkflowCustomField>, List<WorkflowCustomFieldDto>>(customFields);
        }

        public async Task<List<WorkflowCustomFieldDto>> GetVisibleInStatusAsync(Guid workflowSchemeId, Guid statusId)
        {
            var customFields = await _customFieldRepository.GetVisibleInStatusAsync(workflowSchemeId, statusId);
            return ObjectMapper.Map<List<WorkflowCustomField>, List<WorkflowCustomFieldDto>>(customFields);
        }

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Create)]
        public async Task<WorkflowCustomFieldDto> CreateAsync(CreateWorkflowCustomFieldDto input)
        {
            var customField = new WorkflowCustomField(
                GuidGenerator.Create(),
                input.WorkflowSchemeId,
                input.CustomFieldDefinitionId,
                input.IsRequired,
                input.DefaultValue,
                input.DisplayOrder
            );

            // Set rules if provided
            if (input.VisibilityRules != null)
            {
                var visibilityRules = ObjectMapper.Map<FieldVisibilityRulesDto, FieldVisibilityRules>(input.VisibilityRules);
                customField.SetVisibilityRules(visibilityRules);
            }

            if (input.ValidationRules != null)
            {
                var validationRules = ObjectMapper.Map<FieldValidationRulesDto, FieldValidationRules>(input.ValidationRules);
                customField.SetValidationRules(validationRules);
            }

            await _customFieldRepository.InsertAsync(customField);

            return ObjectMapper.Map<WorkflowCustomField, WorkflowCustomFieldDto>(customField);
        }

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Update)]
        public async Task<WorkflowCustomFieldDto> UpdateAsync(Guid id, UpdateWorkflowCustomFieldDto input)
        {
            var customField = await _customFieldRepository.GetAsync(id);

            customField.IsRequired = input.IsRequired;
            customField.DefaultValue = input.DefaultValue;
            customField.DisplayOrder = input.DisplayOrder;
            customField.IsActive = input.IsActive;

            // Update rules if provided
            if (input.VisibilityRules != null)
            {
                var visibilityRules = ObjectMapper.Map<FieldVisibilityRulesDto, FieldVisibilityRules>(input.VisibilityRules);
                customField.SetVisibilityRules(visibilityRules);
            }

            if (input.ValidationRules != null)
            {
                var validationRules = ObjectMapper.Map<FieldValidationRulesDto, FieldValidationRules>(input.ValidationRules);
                customField.SetValidationRules(validationRules);
            }

            await _customFieldRepository.UpdateAsync(customField);

            return ObjectMapper.Map<WorkflowCustomField, WorkflowCustomFieldDto>(customField);
        }

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _customFieldRepository.DeleteAsync(id);
        }

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Update)]
        public async Task<WorkflowCustomFieldDto> ActivateAsync(Guid id)
        {
            var customField = await _customFieldRepository.GetAsync(id);
            customField.IsActive = true;
            await _customFieldRepository.UpdateAsync(customField);

            return ObjectMapper.Map<WorkflowCustomField, WorkflowCustomFieldDto>(customField);
        }

        [Authorize(WorkManagementPermissions.Workflow.Workflows.Update)]
        public async Task<WorkflowCustomFieldDto> DeactivateAsync(Guid id)
        {
            var customField = await _customFieldRepository.GetAsync(id);
            customField.IsActive = false;
            await _customFieldRepository.UpdateAsync(customField);

            return ObjectMapper.Map<WorkflowCustomField, WorkflowCustomFieldDto>(customField);
        }
    }
}