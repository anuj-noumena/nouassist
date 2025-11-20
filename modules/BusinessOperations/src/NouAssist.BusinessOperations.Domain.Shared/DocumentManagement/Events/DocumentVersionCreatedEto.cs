using System;
using Volo.Abp.EventBus;

namespace NouAssist.BusinessOperations.DocumentManagement.Events
{
    /// <summary>
    /// Event raised when a new document version is created
    /// </summary>
    [EventName("NouAssist.BusinessOperations.DocumentManagement.DocumentVersionCreated")]
    public class DocumentVersionCreatedEto
    {
        public Guid DocumentId { get; set; }
        public int VersionNumber { get; set; }
        public string Filename { get; set; }
        public long FileSize { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? TenantId { get; set; }
    }
}
