using System;
using System.Linq.Expressions;
using NouAssist.Core.Specifications;
using NouAssist.Tasks.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.Tasks.Specifications
{
    public class ActiveTasksSpecification : BaseSpecification<Task>
    {
        public override Expression<Func<Task, bool>> ToExpression() =>
            t => t.IsActive && !t.IsDeleted;
    }

    public class RecurringTasksSpecification : BaseSpecification<Task>
    {
        public override Expression<Func<Task, bool>> ToExpression() =>
            t => t.IsRecurring && t.IsActive;
    }

    public class TasksByOwnerSpecification : BaseSpecification<Task>
    {
        private readonly Guid _ownerId;

        public TasksByOwnerSpecification(Guid ownerId)
        {
            _ownerId = ownerId;
        }

        public override Expression<Func<Task, bool>> ToExpression() =>
            t => t.OwnerId == _ownerId && !t.IsDeleted;
    }

    public class TasksByProjectSpecification : BaseSpecification<Task>
    {
        private readonly Guid _projectId;

        public TasksByProjectSpecification(Guid projectId)
        {
            _projectId = projectId;
        }

        public override Expression<Func<Task, bool>> ToExpression() =>
            t => t.ProjectId == _projectId && t.IsActive;
    }
}
