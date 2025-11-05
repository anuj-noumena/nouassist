using System;
using System.Linq.Expressions;
using NouAssist.SharedKernel.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.SharedKernel.Specifications
{
    public class UnprocessedInboxItemsSpecification : Specification<InboxItem>
    {
        public override Expression<Func<InboxItem, bool>> ToExpression() =>
            i => !i.Processed;
    }

    public class StaleInboxItemsSpecification : Specification<InboxItem>
    {
        private readonly DateTime _staleDate;

        public StaleInboxItemsSpecification(int daysToConsiderStale = 7)
        {
            _staleDate = DateTime.UtcNow.AddDays(-daysToConsiderStale);
        }

        public override Expression<Func<InboxItem, bool>> ToExpression() =>
            i => !i.Processed && i.CreationTime < _staleDate;
    }

    public class ProcessedInboxItemsByDateRangeSpecification : Specification<InboxItem>
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public ProcessedInboxItemsByDateRangeSpecification(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        public override Expression<Func<InboxItem, bool>> ToExpression() =>
            i => i.Processed && 
                 i.ProcessedAt.HasValue &&
                 i.ProcessedAt.Value >= _startDate &&
                 i.ProcessedAt.Value <= _endDate;
    }
}
