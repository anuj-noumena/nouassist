using System;
using Volo.Abp.EventBus;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Events
{
    /// <summary>
    /// Event published when a portfolio is created
    /// </summary>
    [EventName("NouAssist.BusinessOperations.ContractPortfolioManagement.PortfolioCreated")]
    public class PortfolioCreatedEto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? TenantId { get; set; }
    }
}
