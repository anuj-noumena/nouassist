using System;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Enums;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Events.Base
{
    /// <summary>
    /// Base class for contract-related events
    /// </summary>
    [Serializable]
    public abstract class ContractEventBase
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public ContractStatus Status { get; set; }
        public Guid? TenantId { get; set; }
    }
}
