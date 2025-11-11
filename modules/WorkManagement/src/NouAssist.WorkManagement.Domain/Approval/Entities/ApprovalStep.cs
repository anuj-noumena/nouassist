using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkManagement.Approval.Enums;
using NouAssist.WorkManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.WorkManagement.Approval.Entities
{
    /// <summary>
    /// Represents a single step in an approval workflow.
    /// Each step defines approvers, approval requirements, conditions, and optional timeout or custom behavior.
    /// </summary>
    public class ApprovalStep : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// Identifier of the workflow this step belongs to.
        /// </summary>
        public Guid WorkflowId { get; private set; }

        /// <summary>
        /// The name or title of this step (e.g., “Manager Review” or “Finance Approval”).
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Optional description providing details about the purpose or logic of this step.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Defines the sequential order of this step within the workflow.
        /// Steps execute in ascending order.
        /// </summary>
        public int OrderIndex { get; private set; }

        /// <summary>
        /// Specifies the approval requirement strategy for this step (e.g., Any, All, Percentage).
        /// </summary>
        public ApprovalRequirement Requirement { get; private set; }

        /// <summary>
        /// Optional count defining how many approvals are required to pass this step (used for conditional approval types).
        /// </summary>
        public int? RequiredApprovals { get; private set; }

        /// <summary>
        /// Indicates whether the submitter (request owner) can approve their own request at this step.
        /// </summary>
        public bool AllowSelfApproval { get; private set; }

        /// <summary>
        /// Defines the timeout period (in hours) for completing this step before an automatic action occurs.
        /// </summary>
        public int? TimeoutHours { get; private set; }

        /// <summary>
        /// Specifies the action that should be automatically taken when the timeout elapses (e.g., auto-approve, escalate).
        /// </summary>
        public string TimeoutAction { get; private set; }

        /// <summary>
        /// A set of conditional expressions (as key–value pairs) that determine if this step should be executed 
        /// based on the runtime approval context.
        /// </summary>
        public Dictionary<string, object> Conditions { get; private set; }

        /// <summary>
        /// Represents user-defined, flexible custom fields for the step, 
        /// stored as a value object for immutability and validation.
        /// </summary>
        public CustomFieldValues CustomFields { get; private set; }

        /// <summary>
        /// Arbitrary metadata for system or integration-level tracking (e.g., source system, version info).
        /// </summary>
        public Dictionary<string, object> Metadata { get; private set; }

        /// <summary>
        /// List of individual user IDs who are assigned as approvers for this step.
        /// </summary>
        private readonly List<Guid> _approverUserIds = new();

        /// <summary>
        /// Read-only collection of all assigned approver user IDs.
        /// </summary>
        public IReadOnlyCollection<Guid> ApproverUserIds => _approverUserIds.AsReadOnly();

        /// <summary>
        /// List of role IDs whose members are approvers for this step.
        /// </summary>
        private readonly List<Guid> _approverRoleIds = new();

        /// <summary>
        /// Read-only collection of approver role IDs.
        /// </summary>
        public IReadOnlyCollection<Guid> ApproverRoleIds => _approverRoleIds.AsReadOnly();

        /// <summary>
        /// List of team or group IDs whose members are approvers for this step.
        /// </summary>
        private readonly List<Guid> _approverTeamIds = new();

        /// <summary>
        /// Read-only collection of approver team IDs.
        /// </summary>
        public IReadOnlyCollection<Guid> ApproverTeamIds => _approverTeamIds.AsReadOnly();

        /// <summary>
        /// Protected constructor for ORM frameworks.
        /// </summary>
        protected ApprovalStep() { /* ORM */ }

        /// <summary>
        /// Creates a new approval step associated with a workflow and establishes its base configuration.
        /// </summary>
        public ApprovalStep(
            Guid id,
            Guid workflowId,
            string name,
            int orderIndex,
            ApprovalRequirement requirement,
            string description = null
        ) : base(id)
        {
            WorkflowId = workflowId;
            SetName(name);
            OrderIndex = orderIndex;
            Requirement = requirement;
            Description = description;
            AllowSelfApproval = false;
            Conditions = new Dictionary<string, object>();
            CustomFields = CustomFieldValues.Create();
            Metadata = new Dictionary<string, object>();
        }

        /// <summary>
        /// Validates and sets the step name.
        /// </summary>
        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), 200);
        }

        /// <summary>
        /// Updates the step’s position within the workflow sequence.
        /// </summary>
        public void SetOrderIndex(int orderIndex)
        {
            if (orderIndex < 1)
            {
                throw new BusinessException("Approval:InvalidOrderIndex");
            }
            OrderIndex = orderIndex;
        }

        /// <summary>
        /// Defines the number of approvals required for this step to pass.
        /// </summary>
        public void SetRequiredApprovals(int count)
        {
            if (count < 1)
            {
                throw new BusinessException("Approval:InvalidRequiredApprovalsCount");
            }
            RequiredApprovals = count;
        }

        /// <summary>
        /// Enables self-approval capability for this step.
        /// </summary>
        public void AllowSelfApprovalForStep()
        {
            AllowSelfApproval = true;
        }

        /// <summary>
        /// Configures timeout duration and the action to take when the timeout expires.
        /// </summary>
        public void SetTimeout(int hours, string action)
        {
            if (hours <= 0)
            {
                throw new BusinessException("Approval:InvalidTimeoutHours");
            }
            TimeoutHours = hours;
            TimeoutAction = action;
        }

        #region Approver Management

        /// <summary>
        /// Adds a user as an approver for this step.
        /// </summary>
        public void AddApproverUser(Guid userId)
        {
            if (!_approverUserIds.Contains(userId))
            {
                _approverUserIds.Add(userId);
            }
        }

        /// <summary>
        /// Removes a user from the approvers list.
        /// </summary>
        public void RemoveApproverUser(Guid userId)
        {
            _approverUserIds.Remove(userId);
        }

        /// <summary>
        /// Adds a role as an approver group for this step.
        /// </summary>
        public void AddApproverRole(Guid roleId)
        {
            if (!_approverRoleIds.Contains(roleId))
            {
                _approverRoleIds.Add(roleId);
            }
        }

        /// <summary>
        /// Removes a role-based approver from this step.
        /// </summary>
        public void RemoveApproverRole(Guid roleId)
        {
            _approverRoleIds.Remove(roleId);
        }

        /// <summary>
        /// Adds a team as an approver group for this step.
        /// </summary>
        public void AddApproverTeam(Guid teamId)
        {
            if (!_approverTeamIds.Contains(teamId))
            {
                _approverTeamIds.Add(teamId);
            }
        }

        /// <summary>
        /// Removes a team from the list of approver groups.
        /// </summary>
        public void RemoveApproverTeam(Guid teamId)
        {
            _approverTeamIds.Remove(teamId);
        }

        /// <summary>
        /// Returns true if this step has at least one assigned approver (user, role, or team).
        /// </summary>
        public bool HasApprovers()
        {
            return _approverUserIds.Any() ||
                   _approverRoleIds.Any() ||
                   _approverTeamIds.Any();
        }

        /// <summary>
        /// Validates that the step has at least one approver.
        /// Throws a business exception if none exist.
        /// </summary>
        public void ValidateApprovers()
        {
            if (!HasApprovers())
            {
                throw new BusinessException("Approval:StepMustHaveApprovers");
            }
        }

        #endregion

        #region Conditions Management

        /// <summary>
        /// Sets or updates a condition key–value pair that must be met for this step to execute.
        /// </summary>
        public void SetCondition(string key, object value)
        {
            Conditions[key] = value;
        }

        /// <summary>
        /// Evaluates the step’s conditions against a given runtime context.
        /// Returns true if all conditions are satisfied or none exist.
        /// </summary>
        public bool EvaluateConditions(Dictionary<string, object> context)
        {
            if (Conditions == null || !Conditions.Any())
            {
                return true;
            }

            // Simple key–value match evaluation. Extend with expression parsing if needed.
            foreach (var condition in Conditions)
            {
                if (!context.TryGetValue(condition.Key, out var value) ||
                    !value.Equals(condition.Value))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
