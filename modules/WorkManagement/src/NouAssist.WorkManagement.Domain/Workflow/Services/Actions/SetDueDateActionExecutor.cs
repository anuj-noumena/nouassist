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
    /// Executes due date setting actions.
    /// Follows Single Responsibility Principle - only handles due date updates
    /// </summary>
    public class SetDueDateActionExecutor : DomainService, IWorkflowActionExecutor, ITransientDependency
    {
        public bool CanExecute(WorkflowAction action)
        {
            return action?.ActionType == WorkflowActionType.SetDueDate;
        }

        public async Task<ActionExecutionResult> ExecuteAsync(
            WorkflowAction action,
            WorkflowActionContext context)
        {
            try
            {
                var taskInstanceRepo = LazyServiceProvider.LazyGetRequiredService<ITaskInstanceRepository>();
                var taskInstance = await taskInstanceRepo.GetAsync(context.TaskInstanceId);

                DateTime? dueDate = null;
                var oldDueDate = taskInstance.Schedule.PlannedDueDate;

                // Check if a specific date is provided
                var dueDateObj = action.Configuration.GetValueOrDefault("dueDate");
                if (dueDateObj != null)
                {
                    if (dueDateObj is DateTime dt)
                    {
                        dueDate = dt;
                    }
                    else if (DateTime.TryParse(dueDateObj.ToString(), out var parsedDate))
                    {
                        dueDate = parsedDate;
                    }
                }

                // Check if an offset in days is provided
                var offsetDaysObj = action.Configuration.GetValueOrDefault("offsetDays");
                if (offsetDaysObj != null && dueDate == null)
                {
                    var offsetDays = Convert.ToInt32(offsetDaysObj);
                    dueDate = Clock.Now.AddDays(offsetDays);
                }

                // Check if offset in hours is provided
                var offsetHoursObj = action.Configuration.GetValueOrDefault("offsetHours");
                if (offsetHoursObj != null && dueDate == null)
                {
                    var offsetHours = Convert.ToInt32(offsetHoursObj);
                    dueDate = Clock.Now.AddHours(offsetHours);
                }

                // Update the schedule with the new due date
                taskInstance.Schedule = taskInstance.Schedule.WithPlannedDates(due: dueDate);

                await taskInstanceRepo.UpdateAsync(taskInstance);

                Logger.LogInformation(
                    "Updated due date for task instance {TaskInstanceId} from {OldDueDate} to {NewDueDate}",
                    context.TaskInstanceId, oldDueDate, dueDate);

                var resultData = new System.Collections.Generic.Dictionary<string, object>
                {
                    ["oldDueDate"] = oldDueDate,
                    ["newDueDate"] = dueDate,
                    ["updatedAt"] = Clock.Now
                };

                return ActionExecutionResult.Success(resultData);
            }
            catch (BusinessException ex)
            {
                Logger.LogError(ex, "Business error during due date update");
                return ActionExecutionResult.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error executing due date update action");
                return ActionExecutionResult.Failure($"Due date update failed: {ex.Message}");
            }
        }
    }
}
