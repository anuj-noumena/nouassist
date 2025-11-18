using System;
using System.Linq.Expressions;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.TaskManagement.Specifications
{
    public class TaskInstancesByAreaSpecification : Specification<TaskInstance>
    {
        private readonly Guid _areaId;

        public TaskInstancesByAreaSpecification(Guid areaId)
        {
            _areaId = areaId;
        }

        public override Expression<Func<TaskInstance, bool>> ToExpression()
        {
            return instance => instance.AreaId == _areaId && !instance.IsDeleted;
        }
    }
}