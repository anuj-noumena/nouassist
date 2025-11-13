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
                var taskInstance = await taskInstanceRepo.GetAsync(context.TaskInstanceId);

                var oldImportance = taskInstance.Priority.Importance;
                var oldUrgency = taskInstance.Priority.Urgency;
                var oldQuadrant = taskInstance.Priority.Quadrant;

                taskInstance.UpdatePriority(importance, urgency);

                await taskInstanceRepo.UpdateAsync(taskInstance);

                Logger.LogInformation(
                    "Updated priority for task instance {TaskInstanceId} from I:{OldImportance}/U:{OldUrgency} ({OldQuadrant}) to I:{NewImportance}/U:{NewUrgency} ({NewQuadrant})",
                    context.TaskInstanceId, oldImportance, oldUrgency, oldQuadrant, 
                    importance, urgency, taskInstance.Priority.Quadrant);

                var resultData = new Dictionary<string, object>
                {
                    ["oldImportance"] = oldImportance,
                    ["oldUrgency"] = oldUrgency,
                    ["oldQuadrant"] = oldQuadrant.ToString(),
                    ["newImportance"] = importance,
                    ["newUrgency"] = urgency,
                    ["newQuadrant"] = taskInstance.Priority.Quadrant.ToString(),
                    ["isHighPriority"] = taskInstance.Priority.IsHighPriority,
                    ["shouldSchedule"] = taskInstance.Priority.ShouldSchedule,
                    ["shouldDelegate"] = taskInstance.Priority.ShouldDelegate,
                    ["shouldEliminate"] = taskInstance.Priority.ShouldEliminate,
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