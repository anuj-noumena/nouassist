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

                // Validate range (0-10)
                if (importance < 0 || importance > 10 || urgency < 0 || urgency > 10)
                {
                    return ActionExecutionResult.Failure("Importance and urgency must be between 0 and 10");
                }

                var taskInstanceRepo = LazyServiceProvider.LazyGetRequiredService<ITaskInstanceRepository>();
                var taskInstance = await taskInstanceRepo.GetAsync(context.TaskInstanceId);

                var oldImportance = taskInstance.PriorityMatrix.Importance;
                var oldUrgency = taskInstance.PriorityMatrix.Urgency;

                taskInstance.UpdatePriority(importance, urgency);

                await taskInstanceRepo.UpdateAsync(taskInstance);

                Logger.LogInformation(
                    "Updated priority for task instance {TaskInstanceId} from I:{OldImportance}/U:{OldUrgency} to I:{NewImportance}/U:{NewUrgency}",
                    context.TaskInstanceId, oldImportance, oldUrgency, importance, urgency);

                var resultData = new System.Collections.Generic.Dictionary<string, object>
                {
                    ["oldImportance"] = oldImportance,
                    ["oldUrgency"] = oldUrgency,
                    ["newImportance"] = importance,
                    ["newUrgency"] = urgency,
                    ["quadrant"] = taskInstance.PriorityMatrix.Quadrant.ToString(),
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
