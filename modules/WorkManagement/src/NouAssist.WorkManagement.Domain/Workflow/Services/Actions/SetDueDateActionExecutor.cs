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

                DateTime? newDueDate = null;
                DateTime? newStartDate = null;
                var oldDueDate = taskInstance.Schedule.PlannedDueDate;
                var oldStartDate = taskInstance.Schedule.PlannedStartDate;

                // Check if a specific due date is provided
                var dueDateObj = action.Configuration.GetValueOrDefault("dueDate");
                if (dueDateObj != null)
                {
                    if (dueDateObj is DateTime dt)
                    {
                        newDueDate = dt;
                    }
                    else if (DateTime.TryParse(dueDateObj.ToString(), out var parsedDate))
                    {
                        newDueDate = parsedDate;
                    }
                }

                // Check if an offset in days is provided (relative to current time)
                var offsetDaysObj = action.Configuration.GetValueOrDefault("offsetDays");
                if (offsetDaysObj != null && newDueDate == null)
                {
                    var offsetDays = Convert.ToInt32(offsetDaysObj);
                    newDueDate = Clock.Now.AddDays(offsetDays);
                }

                // Check if offset in hours is provided (relative to current time)
                var offsetHoursObj = action.Configuration.GetValueOrDefault("offsetHours");
                if (offsetHoursObj != null && newDueDate == null)
                {
                    var offsetHours = Convert.ToInt32(offsetHoursObj);
                    newDueDate = Clock.Now.AddHours(offsetHours);
                }

                // Check if offset in minutes is provided (relative to current time)
                var offsetMinutesObj = action.Configuration.GetValueOrDefault("offsetMinutes");
                if (offsetMinutesObj != null && newDueDate == null)
                {
                    var offsetMinutes = Convert.ToInt32(offsetMinutesObj);
                    newDueDate = Clock.Now.AddMinutes(offsetMinutes);
                }

                // Optional: Also set start date
                var startDateObj = action.Configuration.GetValueOrDefault("startDate");
                if (startDateObj != null)
                {
                    if (startDateObj is DateTime startDt)
                    {
                        newStartDate = startDt;
                    }
                    else if (DateTime.TryParse(startDateObj.ToString(), out var parsedStartDate))
                    {
                        newStartDate = parsedStartDate;
                    }
                }

                // Optional: Keep existing start date flag
                var keepExistingStartDate = action.Configuration.GetValueOrDefault("keepExistingStartDate");
                bool shouldKeepStartDate = keepExistingStartDate != null && Convert.ToBoolean(keepExistingStartDate);

                if (shouldKeepStartDate && newStartDate == null)
                {
                    newStartDate = oldStartDate;
                }

                // Validate: start date should be before due date
                if (newStartDate.HasValue && newDueDate.HasValue && newStartDate.Value > newDueDate.Value)
                {
                    return ActionExecutionResult.Failure(
                        $"Start date ({newStartDate.Value:yyyy-MM-dd HH:mm}) cannot be after due date ({newDueDate.Value:yyyy-MM-dd HH:mm})");
                }

                // Update the schedule with the new dates
                // SetPlannedDates method signature: SetPlannedDates(DateTime? start, DateTime? due)
                taskInstance.SetPlannedDates(newStartDate, newDueDate);

                await taskInstanceRepo.UpdateAsync(taskInstance);

                Logger.LogInformation(
                    "Updated schedule for task instance {TaskInstanceId}. Start: {OldStart} -> {NewStart}, Due: {OldDue} -> {NewDue}",
                    context.TaskInstanceId, 
                    oldStartDate?.ToString("yyyy-MM-dd HH:mm") ?? "null",
                    newStartDate?.ToString("yyyy-MM-dd HH:mm") ?? "null",
                    oldDueDate?.ToString("yyyy-MM-dd HH:mm") ?? "null", 
                    newDueDate?.ToString("yyyy-MM-dd HH:mm") ?? "null");

                var resultData = new Dictionary<string, object>
                {
                    ["oldStartDate"] = oldStartDate,
                    ["newStartDate"] = newStartDate,
                    ["oldDueDate"] = oldDueDate,
                    ["newDueDate"] = newDueDate,
                    ["isOverdue"] = taskInstance.Schedule.IsOverdue,
                    ["timeRemaining"] = taskInstance.Schedule.TimeRemaining,
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