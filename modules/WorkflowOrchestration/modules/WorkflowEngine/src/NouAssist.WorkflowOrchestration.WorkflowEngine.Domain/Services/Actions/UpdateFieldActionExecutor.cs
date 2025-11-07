using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.Tasks.Data;
using NouAssist.WorkflowOrchestration.WorkflowEngine.Entities;
using NouAssist.WorkflowOrchestration.WorkflowEngine.Enums;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.Services.Actions
{
    /// <summary>
    /// Updates custom field values on task instances.
    /// Follows Single Responsibility Principle - only handles field updates
    /// </summary>
    public class UpdateFieldActionExecutor : DomainService, IWorkflowActionExecutor, ITransientDependency
    {
        public bool CanExecute(WorkflowAction action)
        {
            return action?.ActionType == WorkflowActionType.UpdateField ||
                   action?.ActionType == WorkflowActionType.SetCustomField;
        }

        public async Task<ActionExecutionResult> ExecuteAsync(
            WorkflowAction action,
            WorkflowActionContext context)
        {
            try
            {
                var fieldName = action.Configuration.GetValueOrDefault("fieldName")?.ToString();
                var fieldValue = action.Configuration.GetValueOrDefault("fieldValue");

                if (string.IsNullOrWhiteSpace(fieldName))
                {
                    return ActionExecutionResult.Failure("Field name is required");
                }

                var taskInstanceRepo = LazyServiceProvider.LazyGetRequiredService<ITaskInstanceRepository>();
                var taskInstance = await taskInstanceRepo.GetAsync(context.TaskInstanceId);

                // Update custom field value
                if (taskInstance.CustomFieldValues == null)
                {
                    taskInstance.CustomFieldValues = new System.Collections.Generic.Dictionary<string, object>();
                }

                var oldValue = taskInstance.CustomFieldValues.ContainsKey(fieldName)
                    ? taskInstance.CustomFieldValues[fieldName]
                    : null;

                taskInstance.CustomFieldValues[fieldName] = fieldValue;

                await taskInstanceRepo.UpdateAsync(taskInstance);

                Logger.LogInformation(
                    "Updated field {FieldName} on task instance {TaskInstanceId} from {OldValue} to {NewValue}",
                    fieldName, context.TaskInstanceId, oldValue, fieldValue);

                var resultData = new System.Collections.Generic.Dictionary<string, object>
                {
                    ["fieldName"] = fieldName,
                    ["oldValue"] = oldValue,
                    ["newValue"] = fieldValue,
                    ["updatedAt"] = Clock.Now
                };

                return ActionExecutionResult.Success(resultData);
            }
            catch (BusinessException ex)
            {
                Logger.LogError(ex, "Business error during field update");
                return ActionExecutionResult.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error executing field update action");
                return ActionExecutionResult.Failure($"Field update failed: {ex.Message}");
            }
        }
    }
}
