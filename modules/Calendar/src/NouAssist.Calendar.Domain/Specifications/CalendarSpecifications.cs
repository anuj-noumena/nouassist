using System;
using System.Linq.Expressions;
using NouAssist.Calendar.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.Calendar.Specifications
{
    public class ActiveCalendarsSpecification : Specification<Aggregates.Calendar>
    {
        public override Expression<Func<Aggregates.Calendar, bool>> ToExpression() =>
            c => c.IsActive && !c.IsDeleted;
    }

    public class CalendarsByOwnerSpecification : Specification<Aggregates.Calendar>
    {
        private readonly Guid _ownerId;

        public CalendarsByOwnerSpecification(Guid ownerId)
        {
            _ownerId = ownerId;
        }

        public override Expression<Func<Aggregates.Calendar, bool>> ToExpression() =>
            c => c.OwnerId == _ownerId;
    }

    public class DefaultCalendarSpecification : Specification<Aggregates.Calendar>
    {
        private readonly Guid _ownerId;

        public DefaultCalendarSpecification(Guid ownerId)
        {
            _ownerId = ownerId;
        }

        public override Expression<Func<Aggregates.Calendar, bool>> ToExpression() =>
            c => c.OwnerId == _ownerId && c.IsDefault && c.IsActive;
    }
}
