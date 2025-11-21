using System;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Events
{
    /// <summary>
    /// Event published when a revenue model is created
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.BusinessModelManagement.RevenueModelCreated")]
    public class RevenueModelCreatedEto : BusinessModelManagementEtoBase
    {
        public string Name { get; set; }
        public RevenueModelType Type { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
