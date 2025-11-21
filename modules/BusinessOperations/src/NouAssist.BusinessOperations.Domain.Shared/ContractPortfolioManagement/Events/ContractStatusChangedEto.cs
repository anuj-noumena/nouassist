using System;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Enums;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Events
{
    /// <summary>
    /// Event published when a contract status is changed
    /// </summary>
    [EventName("NouAssist.BusinessOperations.ContractPortfolioManagement.ContractStatusChanged")]
    public class ContractStatusChangedEto : ContractEventBase
    {
        public ContractStatus PreviousStatus { get; set; }
        public DateTime StatusChangeDate { get; set; }
    }
}
