using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.Shared.ValueObjects;

namespace NouAssist.FinancialManagement.FinancialAnalytics.ReadModels
{
    public class CashFlowProjection
    {
        public DateTime ProjectionDate { get; set; }
        public MonetaryValue CurrentBalance { get; set; }
        public MonetaryValue ProjectedInflows { get; set; }
        public MonetaryValue ProjectedOutflows { get; set; }
        public MonetaryValue ProjectedBalance { get; set; }
        public List<CashFlowItem> InflowDetails { get; set; }
        public List<CashFlowItem> OutflowDetails { get; set; }
    }

    public class CashFlowItem
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public MonetaryValue Amount { get; set; }
        public string Category { get; set; }
        public decimal Confidence { get; set; }
    }
}
