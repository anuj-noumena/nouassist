using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Executes field validation actions.
    /// Follows Single Responsibility Principle - only handles field validation
    /// Validates that required fields are populated before allowing workflow transition.
    /// </summary>
    public class ValidateFieldsActionExecutor : DomainService, IWorkflowActionExecutor, ITransientDependency
    {
        public bool CanExecute(WorkflowAction action)
        {
            return action?.ActionType == WorkflowActionType.ValidateFields;
        }

        public async Task<ActionExecutionResult> ExecuteAsync(
            WorkflowAction action,
            WorkflowActionContext context)
        {
            try
            {
                var fieldNamesObj = action.Configuration.GetValueOrDefault("fieldNames");
                if (fieldNamesObj == null)
                {
                    return ActionExecutionResult.Failure("Field names list is required for validation");
                }

                // Extract field names to validate
                List<string> fieldNames = new List<string>();

                if (fieldNamesObj is IEnumerable<object> fieldList)
                {
                    fieldNames = fieldList
                        .Select(f => f.ToString())
                        .Where(f => !string.IsNullOrWhiteSpace(f))
                        .ToList();
                }
                else if (fieldNamesObj is string fieldNamesStr)
                {
                    // Support comma-separated string
                    fieldNames = fieldNamesStr
                        .Split(',')
                        .Select(f => f.Trim())
                        .Where(f => !string.IsNullOrWhiteSpace(f))
                        .ToList();
                }

                if (!fieldNames.Any())
                {
                    return ActionExecutionResult.Failure("At least one field name is required for validation");
                }

                // Get validation rules (optional)
                var allowEmptyObj = action.Configuration.GetValueOrDefault("allowEmpty");
                bool allowEmpty = allowEmptyObj != null && Convert.ToBoolean(allowEmptyObj);

                var taskInstanceRepo = LazyServiceProvider.LazyGetRequiredService<ITaskInstanceRepository>();
                var taskInstance = await taskInstanceRepo.GetAsync(context.TaskInstanceId);

                // Validate each field
                List<string> missingFields = new List<string>();
                List<string> emptyFields = new List<string>();
                Dictionary<string, object> fieldValues = new Dictionary<string, object>();

                foreach (var fieldName in fieldNames)
                {
                    // Check if field exists in custom fields
                    if (!taskInstance.CustomField.ContainsKey(fieldName))
                    {
                        missingFields.Add(fieldName);
                        continue;
                    }

                    var fieldValue = taskInstance.CustomField.GetValue(fieldName);
                    fieldValues[fieldName] = fieldValue;

                    // Check if field is empty (null or empty string)
                    if (!allowEmpty)
                    {
                        if (fieldValue == null ||
                            (fieldValue is string strValue && string.IsNullOrWhiteSpace(strValue)))
                        {
                            emptyFields.Add(fieldName);
                        }
                    }
                }

                // Build validation result
                bool isValid = !missingFields.Any() && !emptyFields.Any();

                if (!isValid)
                {
                    var errorMessages = new List<string>();

                    if (missingFields.Any())
                    {
                        errorMessages.Add($"Missing fields: {string.Join(", ", missingFields)}");
                    }

                    if (emptyFields.Any())
                    {
                        errorMessages.Add($"Empty fields: {string.Join(", ", emptyFields)}");
                    }

                    var errorMessage = string.Join("; ", errorMessages);

                    Logger.LogWarning(
                        "Field validation failed for task instance {TaskInstanceId}: {ErrorMessage}",
                        context.TaskInstanceId, errorMessage);

                    return ActionExecutionResult.Failure($"Field validation failed: {errorMessage}");
                }

                Logger.LogInformation(
                    "Field validation passed for task instance {TaskInstanceId}. Validated {FieldCount} fields",
                    context.TaskInstanceId, fieldNames.Count);

                var resultData = new Dictionary<string, object>
                {
                    ["validatedFields"] = fieldNames,
                    ["fieldValues"] = fieldValues,
                    ["isValid"] = true,
                    ["validatedAt"] = Clock.Now
                };

                return ActionExecutionResult.Success(resultData);
            }
            catch (BusinessException ex)
            {
                Logger.LogError(ex, "Business error during field validation");
                return ActionExecutionResult.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error executing field validation action");
                return ActionExecutionResult.Failure($"Field validation failed: {ex.Message}");
            }
        }
    }
}
