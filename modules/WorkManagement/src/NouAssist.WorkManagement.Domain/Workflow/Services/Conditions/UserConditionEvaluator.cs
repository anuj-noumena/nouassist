using System;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Data;
using NouAssist.WorkManagement.Workflow.ValueObjects;

namespace NouAssist.WorkManagement.Workflow.Services.Conditions;

    /// <summary>
    /// NEW: User-based condition evaluator
    /// </summary>
    public class UserConditionEvaluator : ConditionEvaluatorBase
    {
        protected override string[] SupportedOperators => new[]
        {
            "isCurrentUser",
            "isOwner",
            "isAssignee",
            "isReporter",
            "isInTeam",
            "isWatcher"
        };

        public override async Task<ConditionEvaluationResult> EvaluateAsync(
            WorkflowCondition condition,
            WorkflowConditionContext context)
        {
            var taskInstanceRepo = LazyServiceProvider.LazyGetRequiredService<ITaskInstanceRepository>();
            var taskInstance = await taskInstanceRepo.GetAsync(context.TaskInstanceId);

            var result = condition.Operator.ToLower() switch
            {
                "iscurrentuser" => CheckIsCurrentUser(taskInstance, condition, context),
                "isowner" => context.CurrentUserId == taskInstance.OwnerId,
                "isassignee" => taskInstance.AssigneeId.HasValue && context.CurrentUserId == taskInstance.AssigneeId,
                "isreporter" => taskInstance.ReporterId.HasValue && context.CurrentUserId == taskInstance.ReporterId,
                "isinteam" => CheckIsInTeam(taskInstance, condition, context),
                "iswatcher" => taskInstance.Collaboration.Watchers.Contains(context.CurrentUserId),
                _ => false
            };

            return result ? Success() : Failure($"User condition not satisfied: {condition.Operator}");
        }

        private bool CheckIsCurrentUser(TaskInstance taskInstance, WorkflowCondition condition, WorkflowConditionContext context)
        {
            var userIdToCheck = condition.Value?.ToString();
            if (string.IsNullOrEmpty(userIdToCheck))
            {
                return false;
            }

            if (Guid.TryParse(userIdToCheck, out var userId))
            {
                return context.CurrentUserId == userId;
            }

            return false;
        }

        private bool CheckIsInTeam(TaskInstance taskInstance, WorkflowCondition condition, WorkflowConditionContext context)
        {
            if (!taskInstance.TeamId.HasValue)
            {
                return false;
            }

            // This would require a team membership check - simplified here
            var teamIdToCheck = condition.Value?.ToString();
            if (string.IsNullOrEmpty(teamIdToCheck))
            {
                return false;
            }

            if (Guid.TryParse(teamIdToCheck, out var teamId))
            {
                return taskInstance.TeamId == teamId;
            }

            return false;
        }
    }