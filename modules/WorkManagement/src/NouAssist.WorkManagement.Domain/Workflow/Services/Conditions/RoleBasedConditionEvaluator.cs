using System;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.WorkManagement.Workflow.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkManagement.Workflow.Services.Conditions
{
    /// <summary>
    /// Evaluates conditions based on user roles.
    /// Follows Single Responsibility Principle - only handles role-based conditions
    /// </summary>
    public class RoleBasedConditionEvaluator : DomainService, IWorkflowConditionEvaluator
    {
        public bool CanEvaluate(WorkflowCondition condition)
        {
            return condition?.Operator != null &&
                   (condition.Operator.Equals("hasRole", StringComparison.OrdinalIgnoreCase) ||
                    condition.Operator.Equals("hasAnyRole", StringComparison.OrdinalIgnoreCase) ||
                    condition.Operator.Equals("hasAllRoles", StringComparison.OrdinalIgnoreCase) ||
                    condition.Operator.Equals("doesNotHaveRole", StringComparison.OrdinalIgnoreCase));
        }

        public Task<ConditionEvaluationResult> EvaluateAsync(
            WorkflowCondition condition,
            WorkflowConditionContext context)
        {
            if (condition == null)
            {
                return Task.FromResult(ConditionEvaluationResult.Failure("Condition is null"));
            }

            if (context.UserRoles == null || !context.UserRoles.Any())
            {
                return Task.FromResult(
                    ConditionEvaluationResult.Failure("User roles not provided in context"));
            }

            var result = condition.Operator.ToLower() switch
            {
                "hasrole" => HasRole(context.UserRoles, condition.Value),
                "hasanyrole" => HasAnyRole(context.UserRoles, condition.Value),
                "hasallroles" => HasAllRoles(context.UserRoles, condition.Value),
                "doesnothаverole" => !HasRole(context.UserRoles, condition.Value),
                _ => false
            };

            return Task.FromResult(result
                ? ConditionEvaluationResult.Success()
                : ConditionEvaluationResult.Failure(
                    $"Role condition not satisfied: {condition.Operator} {condition.Value}"));
        }

        private bool HasRole(System.Collections.Generic.List<string> userRoles, object requiredRole)
        {
            if (requiredRole == null) return false;
            var role = requiredRole.ToString();
            return userRoles.Any(r => r.Equals(role, StringComparison.OrdinalIgnoreCase));
        }

        private bool HasAnyRole(System.Collections.Generic.List<string> userRoles, object requiredRoles)
        {
            if (requiredRoles == null) return false;

            var roles = requiredRoles.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(r => r.Trim())
                .ToList();

            return roles.Any(role => userRoles.Any(ur => ur.Equals(role, StringComparison.OrdinalIgnoreCase)));
        }

        private bool HasAllRoles(System.Collections.Generic.List<string> userRoles, object requiredRoles)
        {
            if (requiredRoles == null) return false;

            var roles = requiredRoles.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(r => r.Trim())
                .ToList();

            return roles.All(role => userRoles.Any(ur => ur.Equals(role, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
