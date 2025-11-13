using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.WorkManagement.TaskManagement.Data;
using NouAssist.WorkManagement.TaskManagement.Managers;
using NouAssist.WorkManagement.Workflow.Entities;
using NouAssist.WorkManagement.Workflow.Enums;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace NouAssist.WorkManagement.Workflow.Services.Actions
{
    /// <summary>
    /// Executes subtask creation actions.
    /// Follows Single Responsibility Principle - only handles subtask creation
    /// </summary>
    public class CreateSubtaskActionExecutor : DomainService, IWorkflowActionExecutor, ITransientDependency
    {
        public bool CanExecute(WorkflowAction action)
        {
            return action?.ActionType == WorkflowActionType.CreateSubtask;
        }

        public async Task<ActionExecutionResult> ExecuteAsync(
            WorkflowAction action,
            WorkflowActionContext context)
        {
            try
            {
                var taskIdObj = action.Configuration.GetValueOrDefault("taskId");
                if (taskIdObj == null)
                {
                    return ActionExecutionResult.Failure("Task ID (blueprint) is required for subtask creation");
                }

                var taskId = Guid.Parse(taskIdObj.ToString());

                // Optional: Override assignee
                Guid? assigneeId = null;
                var assigneeIdObj = action.Configuration.GetValueOrDefault("assigneeId");
                if (assigneeIdObj != null)
                {
                    assigneeId = Guid.Parse(assigneeIdObj.ToString());
                }

                // Optional: Custom importance and urgency
                var importanceObj = action.Configuration.GetValueOrDefault("importance");
                var urgencyObj = action.Configuration.GetValueOrDefault("urgency");

                double importance = importanceObj != null ? Convert.ToDouble(importanceObj) : 5.0;
                double urgency = urgencyObj != null ? Convert.ToDouble(urgencyObj) : 5.0;

                // Get parent task instance
                var taskInstanceRepo = LazyServiceProvider.LazyGetRequiredService<ITaskInstanceRepository>();
                var parentTaskInstance = await taskInstanceRepo.GetAsync(context.TaskInstanceId);

                // Get task instance manager to create the subtask
                var taskInstanceManager = LazyServiceProvider.LazyGetRequiredService<TaskInstanceManager>();

                // Create subtask as a child of the current task
                var subtaskInstance = await taskInstanceManager.CreateAsync(
                    taskId: taskId,
                    workflowId: parentTaskInstance.WorkflowId,
                    ownerId: parentTaskInstance.OwnerId,
                    reporterId: context.ExecutedBy,
                    assigneeId: assigneeId ?? parentTaskInstance.AssigneeId,
                    parentTaskInstanceId: context.TaskInstanceId,
                    importance: importance,
                    urgency: urgency,
                    tenantId: CurrentTenant.Id
                );

                await taskInstanceRepo.InsertAsync(subtaskInstance);

                Logger.LogInformation(
                    "Created subtask {SubtaskId} for task instance {ParentTaskInstanceId}",
                    subtaskInstance.Id, context.TaskInstanceId);

                var resultData = new System.Collections.Generic.Dictionary<string, object>
                {
                    ["subtaskId"] = subtaskInstance.Id,
                    ["parentTaskId"] = context.TaskInstanceId,
                    ["taskBlueprintId"] = taskId,
                    ["assigneeId"] = subtaskInstance.AssigneeId,
                    ["createdAt"] = Clock.Now,
                    ["createdBy"] = context.ExecutedBy
                };

                return ActionExecutionResult.Success(resultData);
            }
            catch (BusinessException ex)
            {
                Logger.LogError(ex, "Business error during subtask creation");
                return ActionExecutionResult.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error executing subtask creation action");
                return ActionExecutionResult.Failure($"Subtask creation failed: {ex.Message}");
            }
        }
    }
}
