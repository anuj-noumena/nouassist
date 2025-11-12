using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.WorkManagement.Workflow.Entities;
using NouAssist.WorkManagement.Workflow.Enums;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkManagement.Workflow.Services.Actions
{
    /// <summary>
    /// Executes script/custom function actions.
    /// Follows Single Responsibility Principle - only handles script execution
    /// Note: This is a base implementation that logs script execution intent.
    /// Integrate with your script execution engine (e.g., Roslyn, JavaScript engine, etc.) as needed.
    /// SECURITY WARNING: Script execution can be dangerous. Ensure proper sandboxing and validation.
    /// </summary>
    public class ExecuteScriptActionExecutor : DomainService, IWorkflowActionExecutor, ITransientDependency
    {
        public bool CanExecute(WorkflowAction action)
        {
            return action?.ActionType == WorkflowActionType.ExecuteScript;
        }

        public async Task<ActionExecutionResult> ExecuteAsync(
            WorkflowAction action,
            WorkflowActionContext context)
        {
            try
            {
                var scriptIdObj = action.Configuration.GetValueOrDefault("scriptId");
                var scriptContent = action.Configuration.GetValueOrDefault("scriptContent")?.ToString();

                if (scriptIdObj == null && string.IsNullOrWhiteSpace(scriptContent))
                {
                    return ActionExecutionResult.Failure("Either scriptId or scriptContent is required");
                }

                Guid? scriptId = null;
                if (scriptIdObj != null)
                {
                    scriptId = Guid.Parse(scriptIdObj.ToString());
                }

                // Get script language (optional, default to C#)
                var scriptLanguage = action.Configuration.GetValueOrDefault("language")?.ToString() ?? "csharp";

                // Get script parameters (optional)
                var parametersObj = action.Configuration.GetValueOrDefault("parameters");
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    ["taskInstanceId"] = context.TaskInstanceId,
                    ["workflowId"] = context.WorkflowId,
                    ["executedBy"] = context.ExecutedBy,
                    ["context"] = context
                };

                if (parametersObj is Dictionary<string, object> paramsDict)
                {
                    foreach (var kvp in paramsDict)
                    {
                        parameters[kvp.Key] = kvp.Value;
                    }
                }

                // Get timeout (optional, default to 30 seconds)
                var timeoutObj = action.Configuration.GetValueOrDefault("timeoutSeconds");
                int timeoutSeconds = timeoutObj != null ? Convert.ToInt32(timeoutObj) : 30;

                // TODO: Integrate with actual script execution engine
                // SECURITY CONSIDERATIONS:
                // 1. Validate and sanitize script content
                // 2. Run in sandboxed environment with limited permissions
                // 3. Implement timeout to prevent infinite loops
                // 4. Restrict access to sensitive APIs
                // 5. Log all script executions for audit purposes
                //
                // Example using Roslyn for C#:
                // var scriptRunner = LazyServiceProvider.LazyGetRequiredService<IScriptRunner>();
                // var result = await scriptRunner.ExecuteAsync(scriptContent, parameters, TimeSpan.FromSeconds(timeoutSeconds));
                //
                // Example for JavaScript:
                // var jsEngine = LazyServiceProvider.LazyGetRequiredService<IJavaScriptEngine>();
                // var result = await jsEngine.ExecuteAsync(scriptContent, parameters);

                Logger.LogWarning(
                    "Script execution requested for task {TaskInstanceId}. Language: {Language}, ScriptId: {ScriptId}. " +
                    "Note: Actual script execution is not implemented - this is a placeholder.",
                    context.TaskInstanceId, scriptLanguage, scriptId);

                var resultData = new Dictionary<string, object>
                {
                    ["scriptId"] = scriptId,
                    ["language"] = scriptLanguage,
                    ["executedAt"] = Clock.Now,
                    ["timeoutSeconds"] = timeoutSeconds,
                    ["status"] = "NotImplemented",
                    ["message"] = "Script execution engine not configured. Integrate with Roslyn, JavaScript engine, or other scripting runtime."
                };

                // For now, return success indicating the script was acknowledged
                // In production, you would integrate with a real script execution engine
                return ActionExecutionResult.Success(resultData);
            }
            catch (BusinessException ex)
            {
                Logger.LogError(ex, "Business error during script execution");
                return ActionExecutionResult.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error executing script action");
                return ActionExecutionResult.Failure($"Script execution failed: {ex.Message}");
            }
        }
    }
}
