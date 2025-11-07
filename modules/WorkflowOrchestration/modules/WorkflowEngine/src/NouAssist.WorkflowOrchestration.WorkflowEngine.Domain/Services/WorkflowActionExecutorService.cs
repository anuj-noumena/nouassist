using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.WorkflowOrchestration.WorkflowEngine.Entities;
using NouAssist.WorkflowOrchestration.WorkflowEngine.Services.Actions;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.Services
{
    /// <summary>
    /// Service for executing workflow actions using Strategy pattern.
    /// Follows Single Responsibility Principle - coordinates action execution
    /// Follows Open/Closed Principle - new action executors can be added without modifying this class
    /// Follows Dependency Inversion Principle - depends on IWorkflowActionExecutor abstraction
    /// </summary>
    public class WorkflowActionExecutorService : DomainService
    {
        private readonly IEnumerable<IWorkflowActionExecutor> _actionExecutors;

        public WorkflowActionExecutorService(IEnumerable<IWorkflowActionExecutor> actionExecutors)
        {
            _actionExecutors = actionExecutors;
        }

        /// <summary>
        /// Executes a list of workflow actions in order
        /// </summary>
        public async Task<ActionExecutionResult> ExecuteActionsAsync(
            List<WorkflowAction> actions,
            WorkflowActionContext context)
        {
            var result = new ActionExecutionResult { IsSuccessful = true };

            if (actions == null || !actions.Any())
            {
                return result;
            }

            foreach (var action in actions)
            {
                if (!action.IsEnabled)
                {
                    Logger.LogDebug("Skipping disabled action {ActionId} ({ActionName})", action.Id, action.Name);
                    continue;
                }

                try
                {
                    if (action.IsAsync)
                    {
                        // Queue for async execution
                        result.QueuedActions.Add(action.Id);
                        Logger.LogInformation(
                            "Queued async action {ActionId} ({ActionName}) for execution",
                            action.Id, action.Name);
                    }
                    else
                    {
                        var executionResult = await ExecuteSingleActionAsync(action, context);

                        if (executionResult.IsSuccessful)
                        {
                            result.ExecutedActions.Add(action.Id);
                            Logger.LogInformation(
                                "Successfully executed action {ActionId} ({ActionName})",
                                action.Id, action.Name);
                        }
                        else
                        {
                            result.FailedActions.Add(action.Id);
                            Logger.LogWarning(
                                "Action {ActionId} ({ActionName}) failed: {ErrorMessage}",
                                action.Id, action.Name, executionResult.ErrorMessage);

                            if (!action.ContinueOnError)
                            {
                                result.IsSuccessful = false;
                                result.ErrorMessage = $"Action '{action.Name}' failed: {executionResult.ErrorMessage}";
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Unexpected error executing workflow action {ActionId} ({ActionName})",
                        action.Id, action.Name);
                    result.FailedActions.Add(action.Id);

                    if (!action.ContinueOnError)
                    {
                        result.IsSuccessful = false;
                        result.ErrorMessage = $"Action '{action.Name}' failed with exception: {ex.Message}";
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Executes a single workflow action using the appropriate executor
        /// </summary>
        private async Task<Actions.ActionExecutionResult> ExecuteSingleActionAsync(
            WorkflowAction action,
            WorkflowActionContext context)
        {
            // Find the first executor that can handle this action
            var executor = _actionExecutors.FirstOrDefault(e => e.CanExecute(action));

            if (executor == null)
            {
                var errorMsg = $"No executor found for action type: {action.ActionType}";
                Logger.LogError(errorMsg);
                return Actions.ActionExecutionResult.Failure(errorMsg);
            }

            Logger.LogDebug(
                "Executing action {ActionId} ({ActionName}) of type {ActionType} using {ExecutorType}",
                action.Id, action.Name, action.ActionType, executor.GetType().Name);

            return await executor.ExecuteAsync(action, context);
        }
    }

    /// <summary>
    /// Context containing data needed for action execution
    /// </summary>
    public class WorkflowActionContext
    {
        public Guid TaskInstanceId { get; set; }
        public Guid WorkflowId { get; set; }
        public Guid TransitionId { get; set; }
        public Guid ExecutedBy { get; set; }
        public string Comment { get; set; }
        public Dictionary<string, object> CustomData { get; set; } = new();
    }

    /// <summary>
    /// Result of executing multiple workflow actions
    /// </summary>
    public class ActionExecutionResult
    {
        public bool IsSuccessful { get; set; }
        public List<Guid> ExecutedActions { get; set; } = new();
        public List<Guid> QueuedActions { get; set; } = new();
        public List<Guid> FailedActions { get; set; } = new();
        public string ErrorMessage { get; set; }

        public int TotalExecuted => ExecutedActions.Count;
        public int TotalQueued => QueuedActions.Count;
        public int TotalFailed => FailedActions.Count;
    }
}
