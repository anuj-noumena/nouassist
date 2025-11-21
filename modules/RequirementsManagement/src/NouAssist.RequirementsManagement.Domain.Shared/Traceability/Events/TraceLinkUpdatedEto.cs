using System;
using NouAssist.RequirementsManagement.Traceability.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.RequirementsManagement.Traceability.Events
{
    /// <summary>
    /// Event raised when an existing trace link is updated
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.Traceability.TraceLinkUpdated")]
    public class TraceLinkUpdatedEto : TraceLinkEtoBase
    {
        /// <summary>
        /// Last modification time of the trace link
        /// </summary>
        public DateTime LastModificationTime { get; set; }
    }
}
