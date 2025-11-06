using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Workflow.Aggregates;
using NouAssist.Workflow.Entities;
using NouAssist.Workflow.Enums;
using Volo.Abp.Domain.Services;
using Microsoft.Extensions.Logging;

namespace NouAssist.Workflow.Services
{
    public class WorkflowActionExecutorService : DomainService
    {
        public async Task<ActionExecutionResult> ExecuteActionsAsync(
            List<WorkflowAction> actions,
            WorkflowActionContext context)
        {
            var result = new ActionExecutionResult { IsSuccessful = true };

            foreach (var action in actions)
            {
                if (!action.IsEnabled)
                {
                    continue;
                }

                try
                {
                    if (action.IsAsync)
                    {
                        // Queue for async execution
                        result.QueuedActions.Add(action.Id);
                    }
                    else
                    {
                        await ExecuteSingleActionAsync(action, context);
                        result.ExecutedActions.Add(action.Id);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Error executing workflow action {ActionId}", action.Id);
                    result.FailedActions.Add(action.Id);

                    if (!action.ContinueOnError)
                    {
                        result.IsSuccessful = false;
                        result.ErrorMessage = $"Action {action.Name} failed: {ex.Message}";
                        break;
                    }
                }
            }

            return result;
        }

        private async Task ExecuteSingleActionAsync(WorkflowAction action, WorkflowActionContext context)
        {
            switch (action.ActionType)
            {
                case WorkflowActionType.DelegateTask: // New enum value
                {
                    var targetUserId = (Guid)action.Configuration["targetUserId"];
                    var reason = (string)action.Configuration.GetValueOrDefault("reason", "No reason provided");
                    var notifyOriginal = (bool?)action.Configuration.GetValueOrDefault("notifyOriginal", true) ?? true;

                    // Load task (assume Task in new Task module)
                    var taskRepo = LazyServiceProvider.LazyGetRequiredService<ITaskRepository>();
                    var task = await taskRepo.GetAsync(context.TaskId);
                    if (task.AssigneeId != context.ExecutedBy)
                        throw new BusinessException("Workflow:NotAssignedToYou");

                    // Reassign
                    var oldAssignee = task.AssigneeId;
                    task.AssigneeId = targetUserId;
                    task.AddHistoryEntry(new TaskHistoryEntry( /* ... */,
                        $"Delegated to {targetUserId} by {context.ExecutedBy}: {reason}"));

                    // Optional Approval tie-in (if configured)
                    var approvalWorkflowId = (Guid?)action.Configuration["approvalWorkflowId"];
                    if (approvalWorkflowId.HasValue)
                    {
                        // Use Approval module
                        var engine = LazyServiceProvider.LazyGetRequiredService<ApprovalWorkflowEngine>();
                        var request = engine.InitiateApprovalRequest(approvalWorkflowId.Value, "TaskDelegation",
                            context.TaskId, context.ExecutedBy, CurrentTenant.Id);
                        // Pause task until approved (set task.Status to "PendingDelegation")
                        task.CurrentStatus = new TaskStatus("PendingDelegation", ...);
                    }

                    // Notifications
                    await _notificationService.SendAsync(targetUserId,
                        $"Task '{task.Title}' delegated to you from {oldAssignee}");
                    if (notifyOriginal)
                        await _notificationService.SendAsync(oldAssignee,
                            $"Your task '{task.Title}' delegated to {targetUserId}: {reason}");

                    // Raise shared event
                    await DistributedEventBus.PublishAsync(new TaskDelegatedEto
                    {
                        TaskId = context.TaskId,
                        FromUserId = oldAssignee,
                        ToUserId = targetUserId,
                        Reason = reason,
                        ViaWorkflow = true // Flag for hybrid tracking
                    });

                    Logger.LogInformation("Integrated delegation: Task {TaskId} from {Old} to {New}", context.TaskId,
                        oldAssignee, targetUserId);
                    break;
                }
                case WorkflowActionType.InitiateApproval:
                    // Resolve ApprovalWorkflow from config
                {
                    var approvalWorkflowId = action.Configuration.GetValueOrDefault("ApprovalWorkflowId") as Guid?;
                    if (approvalWorkflowId.HasValue)
                    {
                        // Inject IApprovalWorkflowRepository or service
                        // var engine = LazyServiceProvider.LazyGetRequiredService<IApprovalWorkflowEngine>();
                        // var request = engine.InitiateApprovalRequest(... context.TaskId, approvalWorkflowId.Value ...);
                        Logger.LogInformation("Initiated approval for task {TaskId}", context.TaskId);
                    }

                    break;
                }
                // ... other cases
                default:
                    Logger.LogInformation("Executing action {ActionName} of type {ActionType}", action.Name,
                        action.ActionType);
                    break;
            }

            await Task.CompletedTask;
        }
    }

    public class WorkflowActionContext
    {
        public Guid TaskId { get; set; }
        public Guid TransitionId { get; set; }
        public Guid ExecutedBy { get; set; }
        public string Comment { get; set; }
        public Dictionary<string, object> CustomData { get; set; } = new();
    }

    public class ActionExecutionResult
    {
        public bool IsSuccessful { get; set; }
        public List<Guid> ExecutedActions { get; set; } = new();
        public List<Guid> QueuedActions { get; set; } = new();
        public List<Guid> FailedActions { get; set; } = new();
        public string ErrorMessage { get; set; }
    }
}