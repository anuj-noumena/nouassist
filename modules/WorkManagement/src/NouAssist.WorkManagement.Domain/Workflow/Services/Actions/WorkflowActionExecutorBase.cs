using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.WorkManagement.Workflow.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkManagement.Workflow.Services.Actions
{
    /// <summary>
    /// Base class for action executors with common functionality
    /// </summary>
    public abstract class WorkflowActionExecutorBase : DomainService, IWorkflowActionExecutor
    {
        protected WorkflowActionExecutorBase()
        {
        }

        public abstract bool CanExecute(WorkflowAction action);

        public async Task<ActionExecutionResult> ExecuteAsync(
            WorkflowAction action,
            WorkflowActionContext context)
        {
            try
            {
                // Validate action configuration
                var validationResult = ValidateConfiguration(action);
                if (!validationResult.IsValid)
                {
                    return ActionExecutionResult.Failure(validationResult.ErrorMessage);
                }

                // Execute with logging
                Logger.LogInformation(
                    "Executing workflow action {ActionType} for task {TaskInstanceId}",
                    action.ActionType, context.TaskInstanceId);

                var result = await ExecuteInternalAsync(action, context);

                if (result.IsSuccessful)
                {
                    Logger.LogInformation(
                        "Successfully executed action {ActionType} for task {TaskInstanceId}",
                        action.ActionType, context.TaskInstanceId);
                }
                else
                {
                    Logger.LogWarning(
                        "Action {ActionType} failed for task {TaskInstanceId}: {Error}",
                        action.ActionType, context.TaskInstanceId, result.ErrorMessage);
                }

                return result;
            }
            catch (BusinessException ex)
            {
                Logger.LogError(ex, "Business error executing action {ActionType}", action.ActionType);
                return ActionExecutionResult.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unexpected error executing action {ActionType}", action.ActionType);
                return ActionExecutionResult.Failure($"Action execution failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Validates action configuration before execution
        /// </summary>
        protected virtual ConfigurationValidationResult ValidateConfiguration(WorkflowAction action)
        {
            return ConfigurationValidationResult.Valid();
        }

        /// <summary>
        /// Internal execution method to be implemented by derived classes
        /// </summary>
        protected abstract Task<ActionExecutionResult> ExecuteInternalAsync(
            WorkflowAction action,
            WorkflowActionContext context);

        /// <summary>
        /// Helper to get required configuration value
        /// </summary>
        protected T GetRequiredConfig<T>(WorkflowAction action, string key)
        {
            if (!action.Configuration.TryGetValue(key, out var value))
            {
                throw new BusinessException($"Workflow:MissingConfiguration",
                    $"Action {action.Name} requires configuration key: {key}");
            }

            try
            {
                if (value is T typedValue)
                {
                    return typedValue;
                }

                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                throw new BusinessException("Workflow:InvalidConfigurationType",
                    $"Configuration key {key} must be of type {typeof(T).Name}");
            }
        }

        /// <summary>
        /// Helper to get optional configuration value
        /// </summary>
        protected T GetOptionalConfig<T>(WorkflowAction action, string key, T defaultValue = default)
        {
            if (!action.Configuration.TryGetValue(key, out var value))
            {
                return defaultValue;
            }

            try
            {
                if (value is T typedValue)
                {
                    return typedValue;
                }

                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }
    }

    public class ConfigurationValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }

        public static ConfigurationValidationResult Valid() => 
            new ConfigurationValidationResult { IsValid = true };

        public static ConfigurationValidationResult Invalid(string message) => 
            new ConfigurationValidationResult { IsValid = false, ErrorMessage = message };
    }
}