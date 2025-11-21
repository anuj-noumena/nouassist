using System;
using NouAssist.RequirementsManagement.Traceability.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.RequirementsManagement.Traceability.Events
{
    /// <summary>
    /// Event raised when a new trace link is created
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.Traceability.TraceLinkCreated")]
    public class TraceLinkCreatedEto : TraceLinkEtoBase
    {
        /// <summary>
        /// Creation time of the trace link
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
