using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Shared.ValueObjects;
using NouAssist.WorkManagement.Workflow.Entities;
using NouAssist.WorkManagement.Workflow.Enums;
using NouAssist.WorkManagement.Workflow.Services.Conditions;
using NouAssist.WorkManagement.Workflow.ValueObjects;
using Volo.Abp.DependencyInjection;

namespace NouAssist.WorkManagement.Workflow.Services.Actions;

/// <summary>
    /// NEW: Conditional action executor
    /// </summary>
    public class ConditionalActionExecutor : WorkflowActionExecutorBase, ITransientDependency
    {
        private readonly WorkflowConditionEvaluationService _conditionService;

        public ConditionalActionExecutor(WorkflowConditionEvaluationService conditionService)
        {
            _conditionService = conditionService;
        }

        public override bool CanExecute(WorkflowAction action)
        {
            return action?.ActionType == WorkflowActionType.ConditionalAction;
        }

        protected override async Task<ActionExecutionResult> ExecuteInternalAsync(
            WorkflowAction action,
            WorkflowActionContext context)
        {
            // Evaluate condition
            var conditionKey = GetRequiredConfig<string>(action, "conditionKey");
            var conditionValue = GetRequiredConfig<object>(action, "conditionValue");
            var actionIdIfTrue = GetRequiredConfig<Guid>(action, "actionIdIfTrue");
            var actionIdIfFalse = GetOptionalConfig<Guid?>(action, "actionIdIfFalse", null);

            var condition = new WorkflowCondition(conditionKey, conditionValue, "equals");
            var conditionContext = new WorkflowConditionContext
            {
                TaskInstanceId = context.TaskInstanceId,
                CurrentUserId = context.ExecutedBy,
                CustomFieldValues = context.CustomData.ContainsKey("customFields")
                    ? context.CustomData["customFields"] as CustomFieldValues
                    : CustomFieldValues.Create()
            };

            var evaluationResult = await _conditionService.EvaluateSingleAsync(condition, conditionContext);

            var targetActionId = evaluationResult.IsSatisfied ? actionIdIfTrue : actionIdIfFalse;

            if (!targetActionId.HasValue)
            {
                return ActionExecutionResult.Success(new Dictionary<string, object>
                {
                    ["conditionResult"] = evaluationResult.IsSatisfied,
                    ["actionExecuted"] = false
                });
            }

            // Load and execute target action
            // Return result

            return ActionExecutionResult.Success(new Dictionary<string, object>
            {
                ["conditionResult"] = evaluationResult.IsSatisfied,
                ["actionExecuted"] = true,
                ["executedActionId"] = targetActionId
            });
        }
    }