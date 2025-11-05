using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.Approval.Aggregates;
using NouAssist.Approval.Entities;
using NouAssist.Approval.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Microsoft.Extensions.Logging;
using NouAssist.Approval.Events;

namespace NouAssist.Approval.Services
{
    public class ApprovalWorkflowEngine : DomainService
    {
        public ApprovalWorkflowEngine()
        {
        }

        public WorkflowExecutionResult ProcessDecision(
            ApprovalRequest request,
            ApprovalWorkflow workflow,
            ApprovalDecision decision,
            Guid userId
        )
        {
            var result = new WorkflowExecutionResult { RequestId = request.Id };

            // Add decision to request
            request.AddDecision(decision);

            // Get current step
            if (!request.CurrentStepId.HasValue)
            {
                result.AddError("No current step in progress");
                return result;
            }

            var currentStep = workflow.GetStep(request.CurrentStepId.Value);

            // Check if step requirements are met
            var stepComplete = IsStepComplete(request, currentStep, workflow.Strategy);

            if (!stepComplete)
            {
                result.Message = "Awaiting additional approvals";
                result.IsStepComplete = false;
                return result;
            }

            // Mark step as complete
            request.CompleteStep(currentStep.Id);
            result.IsStepComplete = true;

            // Check if step was rejected
            var stepDecisions = request.GetDecisionsForStep(currentStep.Id);
            var hasRejection = stepDecisions.Any(d => d.Action == ApprovalAction.Rejected);

            if (hasRejection)
            {
                request.Reject(userId);
                result.IsFinalDecision = true;
                result.IsApproved = false;
                result.Message = "Request rejected";
                return result;
            }

            // Move to next step or complete
            var nextStep = workflow.GetNextStep(currentStep.Id);
            if (nextStep != null)
            {
                request.MoveToStep(nextStep.Id);
                result.NextStepId = nextStep.Id;
                result.Message = $"Moved to next step: {nextStep.Name}";
                result.IsFinalDecision = false;
            }
            else
            {
                // Workflow complete
                request.Approve(userId);
                result.IsFinalDecision = true;
                result.IsApproved = true;
                result.Message = "Request approved";

                request.AddDistributedEvent(new ApprovalWorkflowCompletedEto
                {
                    RequestId = request.Id,
                    WorkflowId = workflow.Id,
                    TenantId = request.TenantId,
                    IsApproved = true,
                    CompletedAt = Clock.Now,
                    TotalSteps = workflow.GetTotalSteps(),
                    CompletedSteps = request.CompletedStepIds.Count
                });
            }

            return result;
        }

        private bool IsStepComplete(
            ApprovalRequest request,
            ApprovalStep step,
            ApprovalStrategy strategy
        )
        {
            var decisions = request.GetDecisionsForStep(step.Id);
            var approvalCount = decisions.Count(d => d.Action == ApprovalAction.Approved);
            var rejectionCount = decisions.Count(d => d.Action == ApprovalAction.Rejected);

            // Check for rejection first
            if (rejectionCount > 0)
            {
                return true; // Complete with rejection
            }

            switch (step.Requirement)
            {
                case ApprovalRequirement.All:
                    var totalApprovers = step.ApproverUserIds.Count + 
                                        step.ApproverRoleIds.Count + 
                                        step.ApproverTeamIds.Count;
                    return approvalCount >= totalApprovers;

                case ApprovalRequirement.Any:
                    return approvalCount >= 1;

                case ApprovalRequirement.Majority:
                    var total = step.ApproverUserIds.Count + 
                               step.ApproverRoleIds.Count + 
                               step.ApproverTeamIds.Count;
                    return approvalCount > (total / 2);

                case ApprovalRequirement.Custom:
                    if (step.RequiredApprovals.HasValue)
                    {
                        return approvalCount >= step.RequiredApprovals.Value;
                    }
                    return false;

                default:
                    return false;
            }
        }

        public ApprovalRequest InitiateApprovalRequest(
            ApprovalWorkflow workflow,
            string entityType,
            Guid entityId,
            Guid requestedBy,
            Guid? tenantId,
            Dictionary<string, object> customFieldValues = null
        )
        {
            if (!workflow.IsActive)
            {
                throw new BusinessException("Approval:WorkflowNotActive");
            }

            if (!workflow.Steps.Any())
            {
                throw new BusinessException("Approval:WorkflowHasNoSteps");
            }

            var request = new ApprovalRequest(
                Guid.NewGuid(),
                workflow.Id,
                entityType,
                entityId,
                requestedBy,
                tenantId
            );

            // Set custom field values
            if (customFieldValues != null)
            {
                foreach (var kvp in customFieldValues)
                {
                    request.SetCustomFieldValue(kvp.Key, kvp.Value);
                }
            }

            // Start at first step
            var firstStep = workflow.Steps.OrderBy(s => s.OrderIndex).First();
            request.MoveToStep(firstStep.Id);

            // Set due date if workflow has timeout
            if (workflow.TimeoutDays.HasValue)
            {
                request.SetDueDate(Clock.Now.AddDays(workflow.TimeoutDays.Value));
            }

            return request;
        }
    }

    public class WorkflowExecutionResult
    {
        public Guid RequestId { get; set; }
        public bool IsStepComplete { get; set; }
        public bool IsFinalDecision { get; set; }
        public bool IsApproved { get; set; }
        public Guid? NextStepId { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; private set; } = new();

        public void AddError(string error)
        {
            Errors.Add(error);
        }

        public bool HasErrors => Errors.Any();
    }
}
