using System;
using System.Collections.Generic;
using NouAssist.WorkflowOrchestration.ApprovalWorkflows.Enums;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Values;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows.ValueObjects
{
    /// <summary>
    /// Defines an allowable or automatic action associated with an approval step.
    /// </summary>
    public class ApprovalActionDefinition : ValueObject
    {
        /// <summary>
        /// The type of action (e.g., Approve, Reject, Delegate, Escalate, AutoApprove).
        /// </summary>
        public ApprovalActionType ActionType { get; private set; }

        /// <summary>
        /// Optional description or display label for the action (for UI or logs).
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// Indicates whether this action can be taken manually by an approver.
        /// </summary>
        public bool IsUserAction { get; private set; }

        /// <summary>
        /// Indicates whether this action is triggered automatically (e.g., on timeout).
        /// </summary>
        public bool IsAutomatic { get; private set; }

        /// <summary>
        /// Optional rule or condition key to evaluate before the action is allowed.
        /// </summary>
        public string ConditionKey { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ActionType;
            yield return Label;
            yield return IsUserAction;
            yield return IsAutomatic;
            yield return ConditionKey;
        }

        protected ApprovalActionDefinition() { }

        public ApprovalActionDefinition(
            ApprovalActionType actionType,
            string label = null,
            bool isUserAction = true,
            bool isAutomatic = false,
            string conditionKey = null)
        {
            ActionType = actionType;
            Label = label ?? actionType.ToString();
            IsUserAction = isUserAction;
            IsAutomatic = isAutomatic;
            ConditionKey = conditionKey;
        }
    }
}
