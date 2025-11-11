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
    /// Executes user assignment actions.
    /// Follows Single Responsibility Principle - only handles user assignment
    /// </summary>
    public class AssignUserActionExecutor : DomainService, IWorkflowActionExecutor, ITransientDependency
    {
        public bool CanExecute(WorkflowAction action)
        {
            return action?.ActionType == WorkflowActionType.AssignToUser ||
                   action?.ActionType == WorkflowActionType.AssignToReporter ||
                   action?.ActionType == WorkflowActionType.AssignToCurrentUser ||
                   action?.ActionType == WorkflowActionType.ClearAssignee;
        }

        public async Task<ActionExecutionResult> ExecuteAsync(
            WorkflowAction action,
            WorkflowActionContext context)
        {
            try
            {
                var taskInstanceRepo = LazyServiceProvider.LazyGetRequiredService<ITaskInstanceRepository>();
                var taskInstance = await taskInstanceRepo.GetAsync(context.TaskInstanceId);

                Guid? targetUserId = action.ActionType switch
                {
                    WorkflowActionType.AssignToUser => GetTargetUserId(action),
                    WorkflowActionType.AssignToReporter => taskInstance.ReporterId,
                    WorkflowActionType.AssignToCurrentUser => context.ExecutedBy,
                    WorkflowActionType.ClearAssignee => null,
                    _ => throw new BusinessException("Invalid assignment action type")
                };

                if (action.ActionType == WorkflowActionType.ClearAssignee)
                {
                    taskInstance.Unassign();
                    Logger.LogInformation("Cleared assignee for task instance {TaskInstanceId}", context.TaskInstanceId);
                }
                else if (targetUserId.HasValue)
                {
                    taskInstance.Assign(targetUserId.Value, context.ExecutedBy);
                    Logger.LogInformation(
                        "Assigned task instance {TaskInstanceId} to user {UserId}",
                        context.TaskInstanceId, targetUserId);
                }
                else
                {
                    return ActionExecutionResult.Failure("No target user ID found for assignment");
                }

                await taskInstanceRepo.UpdateAsync(taskInstance);

                var resultData = new System.Collections.Generic.Dictionary<string, object>
                {
                    ["assignedTo"] = targetUserId,
                    ["assignedAt"] = Clock.Now
                };

                return ActionExecutionResult.Success(resultData);
            }
            catch (BusinessException ex)
            {
                Logger.LogError(ex, "Business error during assignment action");
                return ActionExecutionResult.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error executing assignment action");
                return ActionExecutionResult.Failure($"Assignment failed: {ex.Message}");
            }
        }

        private Guid GetTargetUserId(WorkflowAction action)
        {
            var userIdObj = action.Configuration.GetValueOrDefault("userId") ??
                           action.Configuration.GetValueOrDefault("targetUserId");

            if (userIdObj == null)
            {
                throw new BusinessException("Target user ID not found in action configuration");
            }

            return Guid.Parse(userIdObj.ToString());
        }
    }
}
