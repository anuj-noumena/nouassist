using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.WorkManagement.TaskManagement.Data;
using NouAssist.WorkManagement.Workflow.Entities;
using NouAssist.WorkManagement.Workflow.Enums;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkManagement.Workflow.Services.Actions
{
    /// <summary>
    /// Executes task delegation actions.
    /// Follows Single Responsibility Principle - only handles task delegation
    /// </summary>
    public class DelegateTaskActionExecutor : DomainService, IWorkflowActionExecutor, ITransientDependency
    {
        public bool CanExecute(WorkflowAction action)
        {
            return action?.ActionType == WorkflowActionType.DelegateTask;
        }

        public async Task<ActionExecutionResult> ExecuteAsync(
            WorkflowAction action,
            WorkflowActionContext context)
        {
            try
            {
                // Extract configuration
                var targetUserIdObj = action.Configuration.GetValueOrDefault("targetUserId");
                if (targetUserIdObj == null)
                {
                    return ActionExecutionResult.Failure("Target user ID is required for delegation");
                }

                var targetUserId = Guid.Parse(targetUserIdObj.ToString());
                var note = action.Configuration.GetValueOrDefault("note")?.ToString();

                // Load task instance from Tasks module
                var taskInstanceRepo = LazyServiceProvider.LazyGetRequiredService<ITaskInstanceRepository>();
                var taskInstance = await taskInstanceRepo.GetAsync(context.TaskInstanceId);

                // Validate current user is the assignee
                if (taskInstance.AssigneeId != context.ExecutedBy)
                {
                    return ActionExecutionResult.Failure(
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

                var resultData = new System.Collections.Generic.Dictionary<string, object>
                {
                    ["oldAssignee"] = oldAssignee,
                    ["newAssignee"] = targetUserId,
                    ["delegatedAt"] = Clock.Now
                };

                return ActionExecutionResult.Success(resultData);
            }
            catch (BusinessException ex)
            {
                Logger.LogError(ex, "Business error during delegation action");
                return ActionExecutionResult.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error executing delegation action");
                return ActionExecutionResult.Failure($"Delegation failed: {ex.Message}");
            }
        }
    }
}
