using System;
using System.Linq.Expressions;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.TaskManagement.Specifications
{
    public class TaskInstancesByContextSpecification : Specification<TaskInstance>
    {
        private readonly Guid _contextId;

        public TaskInstancesByContextSpecification(Guid contextId)
        {
            _contextId = contextId;
        }

        public override Expression<Func<TaskInstance, bool>> ToExpression()
        {
            return instance => instance.ContextId == _contextId && !instance.IsDeleted;
        }
    }
}