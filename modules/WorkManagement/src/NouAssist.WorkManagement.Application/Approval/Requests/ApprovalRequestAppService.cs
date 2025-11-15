using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.SharedKernel.Specifications;
using NouAssist.WorkManagement.Approval.Aggregates;
using NouAssist.WorkManagement.Approval.Data;
using NouAssist.WorkManagement.Approval.Entities;
using NouAssist.WorkManagement.Approval.Requests.Dtos;
using NouAssist.WorkManagement.Approval.Services;
using NouAssist.WorkManagement.Approval.Specifications;
using NouAssist.WorkManagement.Permissions;
using NouAssist.WorkManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.Approval.Requests
{
    [Authorize(WorkManagementPermissions.Approval.Requests.Default)]
    public class ApprovalRequestAppService : ApplicationService, IApprovalRequestAppService
    {
        private readonly IApprovalRequestRepository _requestRepository;
        private readonly IApprovalWorkflowRepository _workflowRepository;
        private readonly ApprovalWorkflowEngine _workflowEngine;
        private readonly ApprovalPermissionChecker _permissionChecker;

        public ApprovalRequestAppService(
            IApprovalRequestRepository requestRepository,
            IApprovalWorkflowRepository workflowRepository,
            ApprovalWorkflowEngine workflowEngine,
            ApprovalPermissionChecker permissionChecker)
        {
            _requestRepository = requestRepository;
            _workflowRepository = workflowRepository;
            _workflowEngine = workflowEngine;
            _permissionChecker = permissionChecker;
        }

        [Authorize(WorkManagementPermissions.Approval.Requests.View)]
        public async Task<ApprovalRequestDto> GetAsync(Guid id)
        {
            var request = await _requestRepository.GetWithDecisionsAsync(id);
            return await MapToDto(request);
        }

        public async Task<PagedResultDto<ApprovalRequestDto>> GetListAsync(
            GetApprovalRequestListDto input)
        {
            var spec = CreateListSpecification(input);

            var requests = await _requestRepository.GetListAsync(spec);
            var totalCount = await _requestRepository.CountAsync(spec.ToExpression());

            var dtos = new List<ApprovalRequestDto>();
            foreach (var request in requests)
            {
                dtos.Add(await MapToDto(request));
            }

            return new PagedResultDto<ApprovalRequestDto>(totalCount, dtos);
        }

        public async Task<PagedResultDto<ApprovalRequestDto>> GetMyRequestsAsync(
            GetApprovalRequestListDto input)
        {
            var spec = CreateListSpecification(input)
                .And(new BaseSpecification<ApprovalRequest>(
                    r => r.RequestedBy == CurrentUser.Id.Value));

            var requests = await _requestRepository.GetListAsync(spec);
            var totalCount = await _requestRepository.CountAsync(spec.ToExpression());

            var dtos = new List<ApprovalRequestDto>();
            foreach (var request in requests)
            {
                dtos.Add(await MapToDto(request));
            }

            return new PagedResultDto<ApprovalRequestDto>(totalCount, dtos);
        }

        public async Task<PagedResultDto<ApprovalRequestDto>> GetPendingApprovalsAsync(
            GetApprovalRequestListDto input)
        {
            var spec = new PendingApprovalRequestsSpecification();

            var requests = await _requestRepository.GetListAsync(spec);
            var totalCount = await _requestRepository.CountAsync(spec);

            var dtos = new List<ApprovalRequestDto>();
            foreach (var request in requests)
            {
                dtos.Add(await MapToDto(request));
            }

            return new PagedResultDto<ApprovalRequestDto>(totalCount, dtos);
        }

        public async Task<PagedResultDto<ApprovalRequestDto>> GetOverdueRequestsAsync(
            GetApprovalRequestListDto input)
        {
            var requests = await _requestRepository.GetOverdueRequestsAsync();
            
            // Apply additional filters if needed
            if (input.WorkflowId.HasValue)
            {
                requests = requests.Where(r => r.WorkflowId == input.WorkflowId.Value).ToList();
            }

            var dtos = new List<ApprovalRequestDto>();
            foreach (var request in requests)
            {
                dtos.Add(await MapToDto(request));
            }

            return new PagedResultDto<ApprovalRequestDto>(requests.Count, dtos);
        }

        public async Task<PagedResultDto<ApprovalRequestDto>> GetSlaBreachedRequestsAsync(
            GetApprovalRequestListDto input)
        {
            var requests = await _requestRepository.GetSlaBreachedRequestsAsync();
            
            // Apply additional filters if needed
            if (input.WorkflowId.HasValue)
            {
                requests = requests.Where(r => r.WorkflowId == input.WorkflowId.Value).ToList();
            }

            var dtos = new List<ApprovalRequestDto>();
            foreach (var request in requests)
            {
                dtos.Add(await MapToDto(request));
            }

            return new PagedResultDto<ApprovalRequestDto>(requests.Count, dtos);
        }

        [Authorize(WorkManagementPermissions.Approval.Requests.Create)]
        public async Task<ApprovalRequestDto> CreateAsync(CreateApprovalRequestDto input)
        {
            var workflow = await _workflowRepository.GetWithStepsAsync(input.WorkflowId);

            // Convert Dictionary to CustomFieldValues
            var customFields = input.CustomFields != null 
                ? CustomFieldValues.Create(input.CustomFields)
                : CustomFieldValues.Create();

            var request = _workflowEngine.InitiateApprovalRequest(
                workflow,
                input.EntityType,
                input.EntityId,
                CurrentUser.Id.Value,
                CurrentTenant.Id,
                customFields
            );

            if (input.Priority > 0)
            {
                request.SetPriority(input.Priority);
            }

            if (input.DueDate.HasValue)
            {
                request.SetDueDate(input.DueDate.Value);
            }

            if (input.SlaDeadline.HasValue)
            {
                request.SetSlaDeadline(input.SlaDeadline.Value);
            }

            if (input.Metadata != null)
            {
                request.Metadata = Metadata.Create(input.Metadata);
            }

            await _requestRepository.InsertAsync(request);

            return await MapToDto(request);
        }

        [Authorize(WorkManagementPermissions.Approval.Requests.Update)]
        public async Task<ApprovalRequestDto> UpdateAsync(Guid id, UpdateApprovalRequestDto input)
        {
            var request = await _requestRepository.GetAsync(id);

            if (request.IsComplete)
            {
                throw new UserFriendlyException("Cannot update completed request");
            }

            if (input.Priority > 0)
            {
                request.SetPriority(input.Priority);
            }

            if (input.DueDate.HasValue)
            {
                request.SetDueDate(input.DueDate.Value);
            }

            if (input.SlaDeadline.HasValue)
            {
                request.SetSlaDeadline(input.SlaDeadline.Value);
            }

            await _requestRepository.UpdateAsync(request);

            return await MapToDto(request);
        }

        [Authorize(WorkManagementPermissions.Approval.Requests.Approve)]
        public async Task<ApprovalRequestDto> MakeDecisionAsync(
            Guid id, 
            MakeApprovalDecisionDto input)
        {
            var request = await _requestRepository.GetWithDecisionsAsync(id);
            var workflow = await _workflowRepository.GetWithStepsAsync(request.WorkflowId);

            if (!request.CurrentStepId.HasValue)
            {
                throw new UserFriendlyException("No current step to approve");
            }

            var currentStep = workflow.GetStep(request.CurrentStepId.Value);

            // Simplified permission check
            var userRoles = new List<Guid>();
            var userTeams = new List<Guid>();

            if (!_permissionChecker.CanUserApproveStep(
                CurrentUser.Id.Value,
                currentStep,
                userRoles,
                userTeams,
                request.RequestedBy))
            {
                throw new UserFriendlyException("You are not authorized to approve this request");
            }

            // Create decision
            var decision = new ApprovalDecision(
                GuidGenerator.Create(),
                request.Id,
                currentStep.Id,
                CurrentUser.Id.Value,
                input.Action,
                input.Comments,
                input.Signature
            );

            // Process through workflow engine
            var result = _workflowEngine.ProcessDecision(
                request,
                workflow,
                decision,
                CurrentUser.Id.Value
            );

            if (result.HasErrors)
            {
                throw new UserFriendlyException(string.Join(", ", result.Errors));
            }

            await _requestRepository.UpdateAsync(request);

            return await MapToDto(request);
        }

        [Authorize(WorkManagementPermissions.Approval.Requests.Cancel)]
        public async Task<ApprovalRequestDto> CancelAsync(Guid id, string reason)
        {
            var request = await _requestRepository.GetAsync(id);

            // Only requester or admin can cancel
            if (request.RequestedBy != CurrentUser.Id.Value)
            {
                await AuthorizationService.CheckAsync(WorkManagementPermissions.Approval.Requests.Cancel);
            }

            request.Cancel(CurrentUser.Id.Value, reason);

            await _requestRepository.UpdateAsync(request);

            return await MapToDto(request);
        }

        [Authorize(WorkManagementPermissions.Approval.Requests.Update)]
        public async Task<ApprovalRequestDto> SetCustomFieldAsync(Guid id, SetCustomFieldDto input)
        {
            var request = await _requestRepository.GetAsync(id);

            if (request.IsComplete)
            {
                throw new UserFriendlyException("Cannot modify completed request");
            }

            request.SetCustomFieldValue(input.Key, input.Value);

            await _requestRepository.UpdateAsync(request);

            return await MapToDto(request);
        }

        private ISpecification<ApprovalRequest> CreateListSpecification(
            GetApprovalRequestListDto input)
        {
            ISpecification<ApprovalRequest> spec = new BaseSpecification<ApprovalRequest>();

            if (input.Status.HasValue)
            {
                spec = spec.And(new BaseSpecification<ApprovalRequest>(
                    r => r.Status == input.Status.Value));
            }

            if (input.WorkflowId.HasValue)
            {
                spec = spec.And(new ApprovalRequestsByWorkflowSpecification(
                    input.WorkflowId.Value));
            }

            if (!string.IsNullOrEmpty(input.EntityType) && input.EntityId.HasValue)
            {
                spec = spec.And(new ApprovalRequestsByEntitySpecification(
                    input.EntityType,
                    input.EntityId.Value));
            }

            if (input.RequestedBy.HasValue)
            {
                spec = spec.And(new BaseSpecification<ApprovalRequest>(
                    r => r.RequestedBy == input.RequestedBy.Value));
            }

            if (input.IsOverdue.HasValue && input.IsOverdue.Value)
            {
                spec = spec.And(new OverdueApprovalRequestsSpecification());
            }

            if (input.IsSlaBreached.HasValue && input.IsSlaBreached.Value)
            {
                var now = DateTime.UtcNow;
                spec = spec.And(new BaseSpecification<ApprovalRequest>(
                    r => r.SlaDeadline.HasValue && r.SlaDeadline.Value < now && !r.IsComplete));
            }

            return spec;
        }

        private async Task<ApprovalRequestDto> MapToDto(ApprovalRequest request)
        {
            var dto = ObjectMapper.Map<ApprovalRequest, ApprovalRequestDto>(request);

            // Load additional data
            var workflow = await _workflowRepository.GetWithStepsAsync(request.WorkflowId);
            dto.WorkflowName = workflow.Name;

            if (request.CurrentStepId.HasValue)
            {
                var currentStep = workflow.GetStep(request.CurrentStepId.Value);
                dto.CurrentStepName = currentStep.Name;
            }

            // In real scenario, load user names from identity service
            dto.RequestedByName = $"User-{request.RequestedBy}";

            return dto;
        }
    }
}
