using System;
using System.Linq.Expressions;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.FinancialManagement.GeneralLedger.Specifications
{
    public class PostedJournalEntriesSpecification : Specification<JournalEntryBatch>
    {
        public override Expression<Func<JournalEntryBatch, bool>> ToExpression()
        {
            return batch => batch.Status == BatchStatus.Posted;
        }
    }

    public class JournalEntriesInDateRangeSpecification : Specification<JournalEntryBatch>
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public JournalEntriesInDateRangeSpecification(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        public override Expression<Func<JournalEntryBatch, bool>> ToExpression()
        {
            return batch => batch.PostingDate.Date >= _startDate.Date &&
                           batch.PostingDate.Date <= _endDate.Date;
        }
    }
}
