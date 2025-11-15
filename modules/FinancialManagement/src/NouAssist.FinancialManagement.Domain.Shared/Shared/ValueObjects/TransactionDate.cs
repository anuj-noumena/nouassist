using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.Shared.ValueObjects
{
    public class TransactionDate : ValueObject
    {
        public DateTime Date { get; private set; }

        protected TransactionDate() { }

        public TransactionDate(DateTime date)
        {
            if (date > DateTime.UtcNow.AddDays(1))
                throw new BusinessException("Financial:InvalidTransactionDate", 
                    "Transaction date cannot be more than 1 day in the future");
            
            Date = date.Date; // Normalize to date only
        }

        public bool IsInFuture() => Date > DateTime.UtcNow.Date;
        public bool IsPast() => Date < DateTime.UtcNow.Date;
        public bool IsToday() => Date == DateTime.UtcNow.Date;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Date;
        }

        public override string ToString() => Date.ToString("yyyy-MM-dd");
    }
}
