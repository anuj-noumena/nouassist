using System;
using Volo.Abp.EventBus;

namespace NouAssist.BusinessOperations.DocumentManagement.Events
{
    /// <summary>
    /// Event raised when a document is created
    /// </summary>
    [EventName("NouAssist.BusinessOperations.DocumentManagement.DocumentCreated")]
    public class DocumentCreatedEto
    {
        public Guid Id { get; set; }
        public string Filename { get; set; }
        public string MimeType { get; set; }
        public long FileSize { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? TenantId { get; set; }
    }
}
