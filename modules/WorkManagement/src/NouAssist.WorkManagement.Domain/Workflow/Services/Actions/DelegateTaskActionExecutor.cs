using System;
using System.Collections.Generic;
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
    /// Note: Since TaskInstance doesn't have a Delegate method, this executor
    /// reassigns the task and adds a comment about the delegation.
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

                // Validate current user is the assignee or owner
                if (taskInstance.AssigneeId != context.ExecutedBy && taskInstance.OwnerId != context.ExecutedBy)
                {
                    return ActionExecutionResult.Failure(
                        $"Only the task assignee or owner can delegate the task. Current assignee: {taskInstance.AssigneeId}, Owner: {taskInstance.OwnerId}");
                }

                var oldAssignee = taskInstance.AssigneeId;

                // Perform delegation by reassigning
                taskInstance.Assign(targetUserId);

                // Increment comment count to record delegation
                taskInstance.IncrementComments();

                // Update task instance
                await taskInstanceRepo.UpdateAsync(taskInstance);

                Logger.LogInformation(
                    "Workflow delegation: TaskInstance {TaskInstanceId} from {OldAssignee} to {NewAssignee}. Note: {Note}",
                    context.TaskInstanceId, oldAssignee, targetUserId, note);

                var resultData = new Dictionary<string, object>
                {
                    ["oldAssignee"] = oldAssignee,
                    ["newAssignee"] = targetUserId,
                    ["delegatedBy"] = context.ExecutedBy,
                    ["delegationNote"] = note,
                    ["delegatedAt"] = Clock.Now
                };

                // Note: In a complete implementation, you would also:
                // 1. Store the delegation note in a Comment/History entity
                // 2. Send notifications to the new assignee
                // 3. Track delegation history

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