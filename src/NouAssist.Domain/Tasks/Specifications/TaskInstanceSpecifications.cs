using System;
using System.Linq.Expressions;
using NouAssist.Core.Specifications;
using NouAssist.Tasks.Aggregates;
using NouAssist.Tasks.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.Tasks.Specifications
{
    public class ActiveTaskInstancesSpecification : BaseSpecification<TaskInstance>
    {
        public override Expression<Func<TaskInstance, bool>> ToExpression() =>
            ti => !ti.IsDeleted && !ti.IsCompleted && !ti.IsCancelled;
    }

    public class OverdueTaskInstancesSpecification : BaseSpecification<TaskInstance>
    {
        private readonly DateTime _now;

        public OverdueTaskInstancesSpecification()
        {
            _now = DateTime.UtcNow;
        }

        public override Expression<Func<TaskInstance, bool>> ToExpression() =>
            ti => ti.ActualDueDate.HasValue &&
                  ti.ActualDueDate.Value < _now &&
                  !ti.IsCompleted &&
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
            ti => ti.AssigneeId == _assigneeId && !ti.IsCompleted && !ti.IsDeleted;
    }

    public class HighPriorityTaskInstancesSpecification : BaseSpecification<TaskInstance>
    {
        public override Expression<Func<TaskInstance, bool>> ToExpression() =>
            ti => ti.PriorityMatrix.Quadrant == EisenhowerQuadrant.Q1DoFirst &&
                  !ti.IsCompleted;
    }

    public class BlockedTaskInstancesSpecification : BaseSpecification<TaskInstance>
    {
        public override Expression<Func<TaskInstance, bool>> ToExpression() =>
            ti => ti.IsBlocked && !ti.IsCompleted;
    }

    public class NextActionTasksSpecification : BaseSpecification<TaskInstance>
    {
        public override Expression<Func<TaskInstance, bool>> ToExpression() =>
            ti => ti.IsNextAction && !ti.IsCompleted && !ti.IsBlocked;
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
            ti => ti.IsCompleted &&
                  ti.CompletedAt.HasValue &&
                  ti.CompletedAt.Value >= _startDate &&
                  ti.CompletedAt.Value <= _endDate;
    }
}
