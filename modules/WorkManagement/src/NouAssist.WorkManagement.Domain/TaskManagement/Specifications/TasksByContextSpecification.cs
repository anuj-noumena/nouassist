using System;
using System.Linq.Expressions;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.TaskManagement.Specifications
{
    public class TasksByContextSpecification : Specification<Task>
    {
        private readonly Guid _contextId;

        public TasksByContextSpecification(Guid contextId)
        {
            _contextId = contextId;
        }

        public override Expression<Func<Task, bool>> ToExpression()
        {
            return task => task.ContextId == _contextId && !task.IsDeleted;
        }
    }
}

