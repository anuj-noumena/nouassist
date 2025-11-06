using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Workflow.Aggregates;
using NouAssist.Workflow.Entities;
using NouAssist.Workflow.Enums;
using Volo.Abp;
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
                case WorkflowActionType.DelegateTask:
                {
                    // Extract configuration
                    var targetUserIdObj = action.Configuration.GetValueOrDefault("targetUserId");
                    if (targetUserIdObj == null)
                    {
                        throw new BusinessException("Workflow:DelegateTask:TargetUserIdRequired");
                    }

                    var targetUserId = Guid.Parse(targetUserIdObj.ToString());
                    var note = action.Configuration.GetValueOrDefault("note")?.ToString();

                    // Load task instance from Tasks module
                    var taskInstanceRepo = LazyServiceProvider.LazyGetRequiredService<NouAssist.Tasks.Data.ITaskInstanceRepository>();
                    var taskInstance = await taskInstanceRepo.GetAsync(context.TaskInstanceId);

                    // Validate current user is the assignee
                    if (taskInstance.AssigneeId != context.ExecutedBy)
                    {
                        throw new BusinessException("Workflow:NotAssignedToYou",
                            $"Task must be assigned to you to delegate it. Current assignee: {taskInstance.AssigneeId}");
                    }

                    var oldAssignee = taskInstance.AssigneeId;

                    // Perform delegation using TaskInstance domain method
                    taskInstance.Delegate(context.ExecutedBy, targetUserId, note);

                    // Update task instance
                    await taskInstanceRepo.UpdateAsync(taskInstance);

                    Logger.LogInformation(
                        "Workflow delegation: TaskInstance {TaskInstanceId} from {OldAssignee} to {NewAssignee}",
                        context.TaskInstanceId, oldAssignee, targetUserId);

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
        public Guid TaskInstanceId { get; set; }
        public Guid WorkflowId { get; set; }
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