using System;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Events
{
    /// <summary>
    /// Event published when a contract is versioned
    /// </summary>
    [EventName("NouAssist.BusinessOperations.ContractPortfolioManagement.ContractVersioned")]
    public class ContractVersionedEto : ContractEventBase
    {
        public Guid DocumentId { get; set; }
        public int VersionNumber { get; set; }
        public string VersionLabel { get; set; }
        public DateTime VersionDate { get; set; }
    }
}
