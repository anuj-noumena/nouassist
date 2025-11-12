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
using Volo.Abp.Users;

namespace NouAssist.WorkManagement.Workflow.Services.Actions
{
    /// <summary>
    /// Updates custom field values on task instances.
    /// Follows Single Responsibility Principle - only handles field updates
    /// </summary>
    public class UpdateFieldActionExecutor(ICurrentUser currentUser)
        : DomainService, IWorkflowActionExecutor, ITransientDependency
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

                var oldValue = taskInstance.CustomField.ContainsKey(fieldName)
                    ? taskInstance.CustomField.GetValue(fieldName)
                    : null;

                taskInstance.UpdateCustomFieldValue(fieldName, fieldValue, currentUser.GetId());

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
