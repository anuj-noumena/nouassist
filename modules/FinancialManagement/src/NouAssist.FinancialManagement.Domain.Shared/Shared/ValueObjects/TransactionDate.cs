using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.Shared.ValueObjects
{
    public class TransactionDate : ValueObject
    {
        public DateTime Date { get; }
        public int FiscalYear { get; }
        public int FiscalPeriod { get; }

        protected TransactionDate() { }

        public TransactionDate(DateTime date, int fiscalYearStartMonth = 1)
        {
            if (date > DateTime.UtcNow.AddDays(1))
                throw new ArgumentException("Transaction date cannot be more than 1 day in the future");

            Date = date.Date; // Remove time component
            (FiscalYear, FiscalPeriod) = CalculateFiscalPeriod(date, fiscalYearStartMonth);
        }

        private (int Year, int Period) CalculateFiscalPeriod(DateTime date, int startMonth)
        {
            var year = date.Year;
            var month = date.Month;

            if (month < startMonth)
                year--;

            var period = ((month - startMonth + 12) % 12) + 1;
            return (year, period);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Date;
            yield return FiscalYear;
            yield return FiscalPeriod;
        }

        public override string ToString() => 
            $"{Date:yyyy-MM-dd} (FY{FiscalYear}-P{FiscalPeriod:D2})";
    }
}
