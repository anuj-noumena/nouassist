using System;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Events
{
    /// <summary>
    /// Event published when a new version of business model canvas is created
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.BusinessModelManagement.BusinessModelCanvasVersionCreated")]
    public class BusinessModelCanvasVersionCreatedEto : BusinessModelManagementEtoBase
    {
        public Guid CanvasId { get; set; }
        public string VersionName { get; set; }
        public int VersionNumber { get; set; }
        public bool IsCurrentVersion { get; set; }
    }
}
