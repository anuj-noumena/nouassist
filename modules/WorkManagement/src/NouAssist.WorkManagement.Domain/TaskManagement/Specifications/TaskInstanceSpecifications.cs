using System;
using System.Linq.Expressions;
using NouAssist.SharedKernel.Enums;
using NouAssist.SharedKernel.Specifications;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Enums;

namespace NouAssist.WorkManagement.TaskManagement.Specifications
{
    public class ActiveTaskInstancesSpecification : BaseSpecification<TaskInstance>
    {
        public override Expression<Func<TaskInstance, bool>> ToExpression() =>
            ti => !ti.IsDeleted && !ti.Schedule.IsCompleted && !ti.IsCancelled;
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
                  !ti.IsCancelled;
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
            ti => ti.PriorityMatrix.Quadrant == EisenhowerQuadrant.Q1DoFirst &&
                  !ti.Schedule.IsCompleted;
    }

    public class BlockedTaskInstancesSpecification : BaseSpecification<TaskInstance>
    {
        public override Expression<Func<TaskInstance, bool>> ToExpression() =>
            ti => ti.BlockingInfo.IsBlocked && !ti.Schedule.IsCompleted;
    }

    public class NextActionTasksSpecification : BaseSpecification<TaskInstance>
    {
        public override Expression<Func<TaskInstance, bool>> ToExpression() =>
            ti => ti.IsNextAction && !ti.Schedule.IsCompleted && !ti.BlockingInfo.IsBlocked;
    }

    public class DelegatedTasksSpecification : BaseSpecification<TaskInstance>
    {
        private readonly Guid _delegatedTo;

        public DelegatedTasksSpecification(Guid delegatedTo)
        {
            _delegatedTo = delegatedTo;
        }

        public override Expression<Func<TaskInstance, bool>> ToExpression() =>
            ti => ti.DelegationInfo != null &&
                  ti.DelegationInfo.DelegatedTo == _delegatedTo &&
                  ti.DelegationInfo.Status == DelegationStatus.Delegated;
    }

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
}
