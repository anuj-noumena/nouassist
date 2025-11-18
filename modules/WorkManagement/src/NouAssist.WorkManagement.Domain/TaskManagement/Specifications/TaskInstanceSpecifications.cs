using System;
using System.Linq.Expressions;
using NouAssist.SharedKernel.Enums;
using NouAssist.SharedKernel.Specifications;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance;

namespace NouAssist.WorkManagement.TaskManagement.Specifications
{
    public class ActiveTaskInstancesSpecification : BaseSpecification<TaskInstance>
    {
        public override Expression<Func<TaskInstance, bool>> ToExpression() =>
            ti => !ti.IsDeleted && !ti.Schedule.IsCompleted;
    }

    public class OverdueTaskInstancesSpecification : BaseSpecification<TaskInstance>
    {
        private readonly DateTime _now;

        public OverdueTaskInstancesSpecification()
        {
            _now = DateTime.UtcNow;
        }

        public override Expression<Func<TaskInstance, bool>> ToExpression() =>
            ti => ti.Schedule.ActualDueDate.HasValue &&
                  ti.Schedule.ActualDueDate.Value < _now &&
                  !ti.Schedule.IsCompleted &&
                  !ti.IsDeleted;
    }

    public class TaskInstancesByAssigneeSpecification : BaseSpecification<TaskInstance>
    {
        private readonly Guid _assigneeId;

        public TaskInstancesByAssigneeSpecification(Guid assigneeId)
        {
            _assigneeId = assigneeId;
        }

        public override Expression<Func<TaskInstance, bool>> ToExpression() =>
            ti => ti.AssigneeId == _assigneeId && !ti.Schedule.IsCompleted && !ti.IsDeleted;
    }

    public class HighPriorityTaskInstancesSpecification : BaseSpecification<TaskInstance>
    {
        public override Expression<Func<TaskInstance, bool>> ToExpression() =>
            ti => ti.Priority.Quadrant == EisenhowerQuadrant.Q1DoFirst &&
                  !ti.Schedule.IsCompleted;
    }

    public class BlockedTaskInstancesSpecification : BaseSpecification<TaskInstance>
    {
        public override Expression<Func<TaskInstance, bool>> ToExpression() =>
            ti => ti.IsBlocked && !ti.Schedule.IsCompleted;
    }

    public class NextActionTasksSpecification : BaseSpecification<TaskInstance>
    {
        public override Expression<Func<TaskInstance, bool>> ToExpression() =>
            ti => ti.WorkTracking.IsNextAction && 
                  !ti.Schedule.IsCompleted && 
                  !ti.IsBlocked;
    }

    // Removed DelegatedTasksSpecification as DelegationInfo doesn't exist in current model
    // If delegation is needed, it should be added back to the TaskInstance aggregate

    public class CompletedInDateRangeSpecification : BaseSpecification<TaskInstance>
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public CompletedInDateRangeSpecification(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        public override Expression<Func<TaskInstance, bool>> ToExpression() =>
            ti => ti.Schedule.IsCompleted &&
                  ti.Schedule.CompletedAt.HasValue &&
                  ti.Schedule.CompletedAt.Value >= _startDate &&
                  ti.Schedule.CompletedAt.Value <= _endDate;
    }

    public class TaskInstancesByProjectSpecification : BaseSpecification<TaskInstance>
    {
        private readonly Guid _projectId;

        public TaskInstancesByProjectSpecification(Guid projectId)
        {
            _projectId = projectId;
        }

        public override Expression<Func<TaskInstance, bool>> ToExpression() =>
            ti => ti.ProjectId == _projectId && !ti.Schedule.IsCompleted && !ti.IsDeleted;
    }
}