using System;
using System.Linq.Expressions;
using NouAssist.TimeManagement.Calendar.Aggregates;
using NouAssist.TimeManagement.Calendar.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.TimeManagement.Calendar.Specifications
{
    public class TimeblocksByStatusSpecification : Specification<Timeblock>
    {
        private readonly TimeblockStatus _status;

        public TimeblocksByStatusSpecification(TimeblockStatus status)
        {
            _status = status;
        }

        public override Expression<Func<Timeblock, bool>> ToExpression() =>
            tb => tb.Status == _status;
    }

    public class TimeblocksInDateRangeSpecification : Specification<Timeblock>
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public TimeblocksInDateRangeSpecification(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        public override Expression<Func<Timeblock, bool>> ToExpression() =>
            tb => tb.StartAt < _endDate && tb.EndAt > _startDate;
    }

    public class ActiveTimeblocksSpecification : Specification<Timeblock>
    {
        public override Expression<Func<Timeblock, bool>> ToExpression() =>
            tb => tb.Status == TimeblockStatus.Planned || tb.Status == TimeblockStatus.InProgress;
    }
}
