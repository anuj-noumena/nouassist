using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.InvestmentManagement.Events
{
    [EventName("NouAssist.InvestmentManagement.PortfolioCreated")]
    public class PortfolioCreatedEto : IMultiTenant
    {
        public Guid PortfolioId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public string PortfolioName { get; set; }
        public string RiskTolerance { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
