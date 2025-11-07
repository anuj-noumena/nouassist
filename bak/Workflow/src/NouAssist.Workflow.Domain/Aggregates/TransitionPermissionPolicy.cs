using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Workflow.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.Workflow.Aggregates
{
    /// <summary>
    /// Aggregate root representing a permission policy for workflow transitions.
    /// Separates permission concerns from workflow transition logic following SRP.
    /// Supports both global (workflow-wide) and project-specific permission rules.
    /// </summary>
    public class TransitionPermissionPolicy : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// Defines the scope of this policy (Global, Project, or Custom).
        /// </summary>
        public PermissionPolicyScope Scope { get; private set; }

        /// <summary>
        /// Project ID for project-specific policies. Null for global policies.
        /// </summary>
        public Guid? ProjectId { get; private set; }

        /// <summary>
        /// The workflow this policy applies to.
        /// </summary>
        public Guid WorkflowId { get; private set; }

        /// <summary>
        /// The specific transition this policy governs.
        /// </summary>
        public Guid TransitionId { get; private set; }

        /// <summary>
        /// Human-readable name for this policy (e.g., "Manager Approval Required").
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Optional description explaining the purpose of this policy.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Whether this policy is currently active and should be evaluated.
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Priority for policy evaluation when multiple policies exist.
        /// Higher values are evaluated first. Default is 0.
        /// </summary>
        public int Priority { get; private set; }

        /// <summary>
        /// Backing field for roles allowed to perform this transition.
        /// </summary>
        private readonly List<string> _allowedRoles = new();

        /// <summary>
        /// Read-only collection of role names that can perform this transition.
        /// Empty list means no role restrictions.
        /// </summary>
        public IReadOnlyCollection<string> AllowedRoles => _allowedRoles.AsReadOnly();

        /// <summary>
        /// Backing field for specific user IDs allowed to perform this transition.
        /// </summary>
        private readonly List<Guid> _allowedUserIds = new();

        /// <summary>
        /// Read-only collection of user IDs that can perform this transition.
        /// Empty list means no user-specific restrictions.
        /// </summary>
        public IReadOnlyCollection<Guid> AllowedUserIds => _allowedUserIds.AsReadOnly();

        /// <summary>
        /// Protected parameterless constructor for ORM.
        /// </summary>
        protected TransitionPermissionPolicy()
        {
        }

        /// <summary>
        /// Creates a new transition permission policy.
        /// </summary>
        /// <param name="id">Unique identifier for the policy.</param>
        /// <param name="workflowId">The workflow this policy applies to.</param>
        /// <param name="transitionId">The transition this policy governs.</param>
        /// <param name="name">Human-readable name for the policy.</param>
        /// <param name="scope">Scope of the policy (Global or Project).</param>
        /// <param name="projectId">Required if scope is Project, otherwise null.</param>
        /// <param name="priority">Priority for policy evaluation (higher = first).</param>
        /// <param name="description">Optional description.</param>
        public TransitionPermissionPolicy(
            Guid id,
            Guid workflowId,
            Guid transitionId,
            string name,
            PermissionPolicyScope scope = PermissionPolicyScope.Global,
            Guid? projectId = null,
            int priority = 0,
            string description = null) : base(id)
        {
            WorkflowId = workflowId;
            TransitionId = transitionId;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Scope = scope;
            ProjectId = projectId;
            Priority = priority;
            Description = description;
            IsActive = true;
            ValidateScopeAndProjectId();
        }

        /// <summary>
        /// Checks if a user can perform the transition based on this policy.
        /// Returns true if:
        /// - No restrictions exist (both lists empty) = allow all
        /// - User ID is in the allowed users list
        /// - User has any of the allowed roles
        /// </summary>
        /// <param name="userId">The ID of the user attempting the transition.</param>
        /// <param name="userRoles">List of role names the user belongs to.</param>
        /// <returns>True if user is authorized, false otherwise.</returns>
        public bool CanUserPerform(Guid userId, List<string> userRoles)
        {
            if (!IsActive)
            {
                return false;
            }

            // No restrictions = allow everyone
            if (!_allowedRoles.Any() && !_allowedUserIds.Any())
            {
                return true;
            }

            // Check user-specific permission
            if (_allowedUserIds.Contains(userId))
            {
                return true;
            }

            // Check role-based permission (OR logic - any matching role grants access)
            if (userRoles != null && _allowedRoles.Any(role => userRoles.Contains(role)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a role to the allowed roles list if not already present.
        /// </summary>
        /// <param name="role">Role name to add (e.g., "Manager", "Admin").</param>
        public void AddAllowedRole(string role)
        {
            Check.NotNullOrWhiteSpace(role, nameof(role));
            if (!_allowedRoles.Contains(role))
            {
                _allowedRoles.Add(role);
            }
        }

        /// <summary>
        /// Adds a user ID to the allowed users list if not already present.
        /// </summary>
        /// <param name="userId">User ID to add.</param>
        public void AddAllowedUser(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new BusinessException("Workflow:InvalidUserId", "User ID cannot be empty.");
            }

            if (!_allowedUserIds.Contains(userId))
            {
                _allowedUserIds.Add(userId);
            }
        }

        /// <summary>
        /// Removes a role from the allowed roles list.
        /// </summary>
        /// <param name="role">Role name to remove.</param>
        public void RemoveAllowedRole(string role)
        {
            _allowedRoles.Remove(role);
        }

        /// <summary>
        /// Removes a user ID from the allowed users list.
        /// </summary>
        /// <param name="userId">User ID to remove.</param>
        public void RemoveAllowedUser(Guid userId)
        {
            _allowedUserIds.Remove(userId);
        }

        /// <summary>
        /// Clears all role restrictions.
        /// </summary>
        public void ClearAllowedRoles()
        {
            _allowedRoles.Clear();
        }

        /// <summary>
        /// Clears all user restrictions.
        /// </summary>
        public void ClearAllowedUsers()
        {
            _allowedUserIds.Clear();
        }

        /// <summary>
        /// Activates this policy for evaluation.
        /// </summary>
        public void Activate()
        {
            IsActive = true;
        }

        /// <summary>
        /// Deactivates this policy, preventing it from being evaluated.
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
        }

        /// <summary>
        /// Updates the priority of this policy.
        /// </summary>
        /// <param name="priority">New priority value (higher = evaluated first).</param>
        public void SetPriority(int priority)
        {
            Priority = priority;
        }

        /// <summary>
        /// Updates the scope and project ID of this policy.
        /// </summary>
        /// <param name="scope">New scope value.</param>
        /// <param name="projectId">Project ID (required if scope is Project).</param>
        public void UpdateScope(PermissionPolicyScope scope, Guid? projectId = null)
        {
            Scope = scope;
            ProjectId = projectId;
            ValidateScopeAndProjectId();
        }

        /// <summary>
        /// Indicates whether this policy has any restrictions defined.
        /// </summary>
        public bool HasRestrictions => _allowedRoles.Any() || _allowedUserIds.Any();

        /// <summary>
        /// Validates that project-scoped policies have a ProjectId.
        /// </summary>
        private void ValidateScopeAndProjectId()
        {
            if (Scope == PermissionPolicyScope.Project && !ProjectId.HasValue)
            {
                throw new BusinessException(
                    "Workflow:ProjectScopeMissingProjectId",
                    "Project-scoped permission policies must have a ProjectId.");
            }

            if (Scope == PermissionPolicyScope.Global && ProjectId.HasValue)
            {
                throw new BusinessException(
                    "Workflow:GlobalScopeCannotHaveProjectId",
                    "Global permission policies cannot have a ProjectId.");
            }
        }
    }
}