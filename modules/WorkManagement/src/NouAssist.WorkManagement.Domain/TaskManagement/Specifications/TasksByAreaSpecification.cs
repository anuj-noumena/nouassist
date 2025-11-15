using System;
using System.Linq.Expressions;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.TaskManagement.Specifications
{
    public class TasksByAreaSpecification : Specification<Task>
    {
        private readonly Guid _areaId;

        public TasksByAreaSpecification(Guid areaId)
        {
            _areaId = areaId;
        }

        public override Expression<Func<Task, bool>> ToExpression()
        {
            return task => task.AreaId == _areaId && !task.IsDeleted;
        }
    }
}
