using System;
using Volo.Abp.EventBus;

namespace NouAssist.BusinessOperations.DocumentManagement.Events
{
    /// <summary>
    /// Event raised when a document is deleted
    /// </summary>
    [EventName("NouAssist.BusinessOperations.DocumentManagement.DocumentDeleted")]
    public class DocumentDeletedEto
    {
        public Guid Id { get; set; }
        public bool IsPhysicallyDeleted { get; set; }
        public Guid DeletedBy { get; set; }
        public DateTime DeletionTime { get; set; }
        public Guid? TenantId { get; set; }
    }
}
