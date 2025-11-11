using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.Approval.Data;
using NouAssist.Tasks.Data;
using NouAssist.Workflow.Entities;
using NouAssist.Workflow.Enums;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;

namespace NouAssist.Workflow.Services.Actions
{
    /// <summary>
    /// Validates that an approval request has been completed before allowing transition.
    /// This action is typically used to block workflow progression until approval is granted.
    /// Follows Single Responsibility Principle - only handles approval completion validation
    /// </summary>
    public class CompleteApprovalActionExecutor : DomainService, IWorkflowActionExecutor, ITransientDependency
    {
        public bool CanExecute(WorkflowAction action)
        {
            return action?.ActionType == WorkflowActionType.CompleteApproval;
        }

        public async Task<ActionExecutionResult> ExecuteAsync(
            WorkflowAction action,
            WorkflowActionContext context)
        {
            try
            {
                // Get the task instance to check approval status
                var taskInstanceRepo = LazyServiceProvider.LazyGetRequiredService<ITaskInstanceRepository>();
                var taskInstance = await taskInstanceRepo.GetAsync(context.TaskInstanceId);

                if (taskInstance.Approval == null || !taskInstance.Approval.RequiresApproval)
                {
                    return ActionExecutionResult.Failure("Task does not require approval");
                }

                if (!taskInstance.Approval.IsLinkedToApproval)
                {
                    return ActionExecutionResult.Failure("Task is not linked to an approval request");
                }

                // Load the approval request to check its status
                var approvalRequestRepo = LazyServiceProvider.LazyGetRequiredService<IApprovalRequestRepository>();
                var approvalRequest = await approvalRequestRepo.GetAsync(taskInstance.Approval.ApprovalRequestId.Value);

                // Check if approval is completed
                if (!approvalRequest.IsComplete)
                {
                    return ActionExecutionResult.Failure(
                        $"Approval request {approvalRequest.Id} is still in progress. Current status: {approvalRequest.Status}");
                }

                // Check if approved or rejected
                if (!approvalRequest.IsApproved)
                {
                    var rejectAction = action.Configuration.GetValueOrDefault("onReject")?.ToString() ?? "block";

                    if (rejectAction == "block")
                    {
                        return ActionExecutionResult.Failure(
                            $"Approval request was rejected. Cannot proceed with transition.");
                    }
                    else if (rejectAction == "allow")
                    {
                        Logger.LogWarning(
                            "Approval was rejected but transition is configured to allow. TaskInstance: {TaskInstanceId}",
                            context.TaskInstanceId);
                    }
                }

                Logger.LogInformation(
                    "Approval validation passed for task instance {TaskInstanceId}, approval request {RequestId}",
                    context.TaskInstanceId, approvalRequest.Id);

                var resultData = new System.Collections.Generic.Dictionary<string, object>
                {
                    ["approvalRequestId"] = approvalRequest.Id,
                    ["approvalStatus"] = approvalRequest.Status.ToString(),
                    ["isApproved"] = approvalRequest.IsApproved,
                    ["completedAt"] = approvalRequest.CompletedAt
                };

                return ActionExecutionResult.Success(resultData);
            }
            catch (BusinessException ex)
            {
                Logger.LogError(ex, "Business error during approval completion validation");
                return ActionExecutionResult.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error executing approval completion validation");
                return ActionExecutionResult.Failure($"Approval validation failed: {ex.Message}");
            }
        }
    }
}
