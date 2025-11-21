using System;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Events
{
    /// <summary>
    /// Event raised when a new discussion thread is created.
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.CollaborationAccessControl.DiscussionThreadCreated")]
    public class DiscussionThreadCreatedEto : NotificationEtoBase
    {
        public Guid DiscussionBoardId { get; set; }
        public Guid ThreadId { get; set; }
        public string Subject { get; set; }
        public string InitialContent { get; set; }
    }
}
