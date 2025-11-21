using System;
using Volo.Abp.EventBus;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Events
{
    /// <summary>
    /// Event published when a portfolio roadmap is created
    /// </summary>
    [EventName("NouAssist.BusinessOperations.ContractPortfolioManagement.PortfolioRoadmapCreated")]
    public class PortfolioRoadmapCreatedEto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid PortfolioId { get; set; }
        public Guid? TenantId { get; set; }
    }
}
