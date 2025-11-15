using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;

namespace NouAssist.FinancialManagement.FinancialAnalytics.ReadModels
{
    public class AccountBalanceSummary
    {
        public Guid AccountId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public MonetaryValue CurrentBalance { get; set; }
        public MonetaryValue MonthStartBalance { get; set; }
        public MonetaryValue MonthEndBalance { get; set; }
        public MonetaryValue YearToDateChange { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
