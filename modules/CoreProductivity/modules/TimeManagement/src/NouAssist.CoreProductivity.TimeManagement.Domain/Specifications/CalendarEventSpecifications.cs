using System;
using System.Linq;
using System.Linq.Expressions;
using NouAssist.CoreProductivity.TimeManagement.Aggregates;
using NouAssist.CoreProductivity.TimeManagement.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.CoreProductivity.TimeManagement.Specifications
{
    public class EventsInDateRangeSpecification : Specification<CalendarEvent>
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public EventsInDateRangeSpecification(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        public override Expression<Func<CalendarEvent, bool>> ToExpression() =>
            e => e.StartAt < _endDate && e.EndAt > _startDate && !e.IsCancelled;
    }

    public class RecurringEventsSpecification : Specification<CalendarEvent>
    {
        public override Expression<Func<CalendarEvent, bool>> ToExpression() =>
            e => e.IsRecurring && !e.IsCancelled;
    }

    public class UpcomingEventsSpecification : Specification<CalendarEvent>
    {
        private readonly DateTime _referenceDate;

        public UpcomingEventsSpecification(DateTime? referenceDate = null)
        {
            _referenceDate = referenceDate ?? DateTime.UtcNow;
        }

        public override Expression<Func<CalendarEvent, bool>> ToExpression() =>
            e => e.StartAt > _referenceDate && !e.IsCancelled;
    }

    public class EventsByLinkedEntitySpecification : Specification<CalendarEvent>
    {
        private readonly Guid _entityId;
        private readonly LinkedEntityType _entityType;

        public EventsByLinkedEntitySpecification(Guid entityId, LinkedEntityType entityType)
        {
            _entityId = entityId;
            _entityType = entityType;
        }

        public override Expression<Func<CalendarEvent, bool>> ToExpression() =>
            e => e.LinkedEntities.Any(le => le.EntityId == _entityId && le.EntityType == _entityType);
    }
}
