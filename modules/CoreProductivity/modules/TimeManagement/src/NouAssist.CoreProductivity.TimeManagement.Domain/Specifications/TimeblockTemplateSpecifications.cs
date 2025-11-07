using System;
using System.Linq;
using System.Linq.Expressions;
using NouAssist.CoreProductivity.TimeManagement.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.CoreProductivity.TimeManagement.Specifications
{
    public class ActiveTimeblockTemplatesSpecification : Specification<TimeblockTemplate>
    {
        public override Expression<Func<TimeblockTemplate, bool>> ToExpression() =>
            t => t.IsActive && !t.IsDeleted;
    }

    public class TimeblockTemplatesForDaySpecification : Specification<TimeblockTemplate>
    {
        private readonly int _dayOfWeek;

        public TimeblockTemplatesForDaySpecification(int dayOfWeek)
        {
            _dayOfWeek = dayOfWeek;
        }

        public override Expression<Func<TimeblockTemplate, bool>> ToExpression() =>
            t => t.IsActive && t.DaysOfWeek.Contains(_dayOfWeek);
    }
}
