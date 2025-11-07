using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.Approval.Data;
using NouAssist.Approval.Services;
using NouAssist.WorkflowOrchestration.WorkflowEngine.Entities;
using NouAssist.WorkflowOrchestration.WorkflowEngine.Enums;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.Services.Actions
{
    /// <summary>
    /// Executes approval initiation actions.
    /// Follows Single Responsibility Principle - only handles approval initiation
    /// Integrates workflow transitions with approval workflows
    /// </summary>
    public class InitiateApprovalActionExecutor : DomainService, IWorkflowActionExecutor, ITransientDependency
    {
        public bool CanExecute(WorkflowAction action)
        {
            return action?.ActionType == WorkflowActionType.InitiateApproval;
        }

        public async Task<ActionExecutionResult> ExecuteAsync(
            WorkflowAction action,
            WorkflowActionContext context)
        {
            try
            {
                // Extract configuration
                var approvalWorkflowIdObj = action.Configuration.GetValueOrDefault("ApprovalWorkflowId");
                if (approvalWorkflowIdObj == null)
                {
                    return ActionExecutionResult.Failure("Approval workflow ID is required");
                }

                var approvalWorkflowId = Guid.Parse(approvalWorkflowIdObj.ToString());

                // Get approval workflow
                var approvalWorkflowRepo = LazyServiceProvider.LazyGetRequiredService<IApprovalWorkflowRepository>();
                var approvalWorkflow = await approvalWorkflowRepo.GetAsync(approvalWorkflowId);

                if (!approvalWorkflow.IsActive)
                {
                    return ActionExecutionResult.Failure($"Approval workflow {approvalWorkflowId} is not active");
                }

                // Get approval workflow engine
                var approvalEngine = LazyServiceProvider.LazyGetRequiredService<ApprovalWorkflowEngine>();

                // Extract custom field values from context if provided
                Dictionary<string, object> customFields = null;
                if (action.Configuration.ContainsKey("customFieldValues") &&
                    action.Configuration["customFieldValues"] is Dictionary<string, object> fields)
                {
                    customFields = fields;
                }

                // Initiate approval request
                var approvalRequest = approvalEngine.InitiateApprovalRequest(
                    approvalWorkflow,
                    "TaskInstance",  // Entity type
                    context.TaskInstanceId,  // Entity ID
                    context.ExecutedBy,  // Requested by
                    CurrentTenant.Id,
                    customFields
                );

                // Save approval request
                var approvalRequestRepo = LazyServiceProvider.LazyGetRequiredService<IApprovalRequestRepository>();
                await approvalRequestRepo.InsertAsync(approvalRequest);

                Logger.LogInformation(
                    "Initiated approval request {RequestId} for task instance {TaskInstanceId} using workflow {WorkflowId}",
                    approvalRequest.Id, context.TaskInstanceId, approvalWorkflowId);

                var resultData = new Dictionary<string, object>
                {
                    ["approvalRequestId"] = approvalRequest.Id,
                    ["approvalWorkflowId"] = approvalWorkflowId,
                    ["currentStepId"] = approvalRequest.CurrentStepId,
                    ["initiatedAt"] = Clock.Now
                };

                return ActionExecutionResult.Success(resultData);
            }
            catch (BusinessException ex)
            {
                Logger.LogError(ex, "Business error during approval initiation");
                return ActionExecutionResult.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error executing approval initiation action");
                return ActionExecutionResult.Failure($"Approval initiation failed: {ex.Message}");
            }
        }
    }
}
