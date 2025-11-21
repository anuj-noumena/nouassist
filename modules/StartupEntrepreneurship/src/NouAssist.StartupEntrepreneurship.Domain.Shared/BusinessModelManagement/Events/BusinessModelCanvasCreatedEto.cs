using System;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Events
{
    /// <summary>
    /// Event published when a business model canvas is created
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.BusinessModelManagement.BusinessModelCanvasCreated")]
    public class BusinessModelCanvasCreatedEto : BusinessModelManagementEtoBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
