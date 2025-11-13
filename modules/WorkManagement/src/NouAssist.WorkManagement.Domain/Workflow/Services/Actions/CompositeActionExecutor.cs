using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Workflow.Entities;
using NouAssist.WorkManagement.Workflow.Enums;
using Volo.Abp.DependencyInjection;

namespace NouAssist.WorkManagement.Workflow.Services.Actions;

/// <summary>
/// NEW: Composite action executor for executing multiple actions atomically
/// </summary>
public class CompositeActionExecutor : WorkflowActionExecutorBase, ITransientDependency
{
    private readonly WorkflowActionExecutorService _executorService;

    public CompositeActionExecutor(WorkflowActionExecutorService executorService)
    {
        _executorService = executorService;
    }

    public override bool CanExecute(WorkflowAction action)
    {
        return action?.ActionType == WorkflowActionType.CompositeAction;
    }

    protected override async Task<ActionExecutionResult> ExecuteInternalAsync(
        WorkflowAction action,
        WorkflowActionContext context)
    {
        var actionIds = GetRequiredConfig<List<Guid>>(action, "actionIds");
        var stopOnFirstFailure = GetOptionalConfig(action, "stopOnFirstFailure", true);

        var results = new List<ActionExecutionResult>();
        var allSuccessful = true;

        foreach (var actionId in actionIds)
        {
            // Load sub-action
            // Execute sub-action
            // Collect results

            if (!results.Last().IsSuccessful && stopOnFirstFailure)
            {
                allSuccessful = false;
                break;
            }
        }

        if (allSuccessful)
        {
            return ActionExecutionResult.Success(new Dictionary<string, object>
            {
                ["executedCount"] = results.Count,
                ["results"] = results
            });
        }

        return ActionExecutionResult.Failure("One or more actions failed");
    }
}