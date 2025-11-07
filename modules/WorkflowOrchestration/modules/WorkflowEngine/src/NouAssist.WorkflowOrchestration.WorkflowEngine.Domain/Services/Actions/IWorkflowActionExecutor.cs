using System.Threading.Tasks;
using NouAssist.WorkflowOrchestration.WorkflowEngine.Entities;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.Services.Actions
{
    /// <summary>
    /// Interface for executing individual workflow actions.
    /// Follows Single Responsibility Principle - each executor handles one action type
    /// Follows Open/Closed Principle - new executors can be added without modifying existing code
    /// Follows Dependency Inversion Principle - abstracts action execution
    /// </summary>
    public interface IWorkflowActionExecutor
    {
        /// <summary>
        /// Checks if this executor can handle the given action type
        /// </summary>
        bool CanExecute(WorkflowAction action);

        /// <summary>
        /// Executes the workflow action
        /// </summary>
        Task<ActionExecutionResult> ExecuteAsync(
            WorkflowAction action,
            WorkflowActionContext context);
    }

    /// <summary>
    /// Result of action execution
    /// </summary>
    public class ActionExecutionResult
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public System.Collections.Generic.Dictionary<string, object> ResultData { get; set; }

        public ActionExecutionResult()
        {
            ResultData = new System.Collections.Generic.Dictionary<string, object>();
        }

        public static ActionExecutionResult Success(
            System.Collections.Generic.Dictionary<string, object> resultData = null)
        {
            return new ActionExecutionResult
            {
                IsSuccessful = true,
                ResultData = resultData ?? new System.Collections.Generic.Dictionary<string, object>()
            };
        }

        public static ActionExecutionResult Failure(string errorMessage)
        {
            return new ActionExecutionResult
            {
                IsSuccessful = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
