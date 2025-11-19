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
    /// Executes priority update actions.
    /// Follows Single Responsibility Principle - only handles priority updates
    /// </summary>
    public class UpdatePriorityActionExecutor : DomainService, IWorkflowActionExecutor, ITransientDependency
    {
        public bool CanExecute(WorkflowAction action)
        {
            return action?.ActionType == WorkflowActionType.UpdatePriority;
        }

        public async Task<ActionExecutionResult> ExecuteAsync(
            WorkflowAction action,
            WorkflowActionContext context)
        {
            try
            {
                var importanceObj = action.Configuration.GetValueOrDefault("importance");
                var urgencyObj = action.Configuration.GetValueOrDefault("urgency");

                if (importanceObj == null || urgencyObj == null)
                {
                    return ActionExecutionResult.Failure("Both importance and urgency values are required");
                }

                var importance = Convert.ToDouble(importanceObj);
                var urgency = Convert.ToDouble(urgencyObj);

                // Validate range (0.0-1.0 based on ImportanceUrgencyMatrix validation)
                if (importance < 0.0 || importance > 1.0 || urgency < 0.0 || urgency > 1.0)
                {
                    return ActionExecutionResult.Failure("Importance and urgency must be between 0.0 and 1.0");
                }

                var taskInstanceRepo = LazyServiceProvider.LazyGetRequiredService<ITaskInstanceRepository>();
                var taskInstanceWorkRepo = LazyServiceProvider.LazyGetRequiredService<ITaskInstanceWorkRepository>();

                var taskInstance = await taskInstanceRepo.GetAsync(context.TaskInstanceId);
                var taskWork = await taskInstanceWorkRepo.GetByTaskInstanceIdAsync(context.TaskInstanceId);

                var oldImportance = taskWork?.Priority.Importance ?? 0;
                var oldUrgency = taskWork?.Priority.Urgency ?? 0;
                var oldQuadrant = taskWork?.Priority.Quadrant;

                if (taskWork != null)
                {
                    taskWork.UpdatePriority(importance, urgency);
                    await taskInstanceWorkRepo.UpdateAsync(taskWork);
                }

                Logger.LogInformation(
                    "Updated priority for task instance {TaskInstanceId} from I:{OldImportance}/U:{OldUrgency} ({OldQuadrant}) to I:{NewImportance}/U:{NewUrgency} ({NewQuadrant})",
                    context.TaskInstanceId, oldImportance, oldUrgency, oldQuadrant,
                    importance, urgency, taskWork?.Priority.Quadrant);

                var resultData = new Dictionary<string, object>
                {
                    ["oldImportance"] = oldImportance,
                    ["oldUrgency"] = oldUrgency,
                    ["oldQuadrant"] = oldQuadrant?.ToString() ?? "Unknown",
                    ["newImportance"] = importance,
                    ["newUrgency"] = urgency,
                    ["newQuadrant"] = taskWork?.Priority.Quadrant.ToString() ?? "Unknown",
                    ["isHighPriority"] = taskWork?.Priority.IsHighPriority ?? false,
                    ["shouldSchedule"] = taskWork?.Priority.ShouldSchedule ?? false,
                    ["shouldDelegate"] = taskWork?.Priority.ShouldDelegate ?? false,
                    ["shouldEliminate"] = taskWork?.Priority.ShouldEliminate ?? false,
                    ["updatedAt"] = Clock.Now
                };

                return ActionExecutionResult.Success(resultData);
            }
            catch (BusinessException ex)
            {
                Logger.LogError(ex, "Business error during priority update");
                return ActionExecutionResult.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error executing priority update action");
                return ActionExecutionResult.Failure($"Priority update failed: {ex.Message}");
            }
        }
    }
}