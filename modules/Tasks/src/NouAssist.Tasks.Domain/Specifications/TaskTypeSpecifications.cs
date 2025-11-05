using System;
using System.Linq.Expressions;
using NouAssist.SharedKernel.Specifications;
using NouAssist.Tasks.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.Tasks.Specifications
{
    public class ActiveTaskTypesSpecification : BaseSpecification<TaskType>
    {
        public override Expression<Func<TaskType, bool>> ToExpression() =>
            tt => tt.IsActive;
    }

    public class TaskTypeByNameSpecification : BaseSpecification<TaskType>
    {
        private readonly string _name;

        public TaskTypeByNameSpecification(string name)
        {
            _name = name;
        }

        public override Expression<Func<TaskType, bool>> ToExpression() =>
            tt => tt.Name == _name && tt.IsActive;
    }
}
