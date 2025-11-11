using System;
using System.Linq.Expressions;
using NouAssist.Core.Enums;
using NouAssist.GTD.Aggregates;
using NouAssist.GTD.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.GTD.Specifications
{
    public class DueTicklersSpecification : Specification<TicklerItem>
    {
        private readonly DateTime _currentDate;

        public DueTicklersSpecification(DateTime currentDate)
        {
            _currentDate = currentDate;
        }

        public override Expression<Func<TicklerItem, bool>> ToExpression() =>
            t => t.TickleDate <= _currentDate && 
                 t.Status == TicklerStatus.Scheduled;
    }

    public class SurfacedTicklersSpecification : Specification<TicklerItem>
    {
        public override Expression<Func<TicklerItem, bool>> ToExpression() =>
            t => t.Status == TicklerStatus.Surfaced;
    }

    public class SnoozedTicklersReadyToSurfaceSpecification : Specification<TicklerItem>
    {
        private readonly DateTime _currentDate;

        public SnoozedTicklersReadyToSurfaceSpecification(DateTime currentDate)
        {
            _currentDate = currentDate;
        }

        public override Expression<Func<TicklerItem, bool>> ToExpression() =>
            t => t.Status == TicklerStatus.Snoozed &&
                 t.SnoozeUntil.HasValue &&
                 t.SnoozeUntil.Value <= _currentDate;
    }

    public class RecurringTicklersSpecification : Specification<TicklerItem>
    {
        public override Expression<Func<TicklerItem, bool>> ToExpression() =>
            t => t.IsRecurring && t.RecurrencePattern != null;
    }
}
